using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace eShop.DigitalAthletes.API.Apis;

public static class DigitalAthletesApi
{
    public static IEndpointRouteBuilder MapDigitalAthletesApi(this IEndpointRouteBuilder app)
    {
        var vApi = app.NewVersionedApi("DigitalAthletes");
        var api = vApi.MapGroup("api/digital-athletes").HasApiVersion(1, 0);

        // Browse and search routes
        api.MapGet("/", GetAllAthletes)
            .WithName("ListDigitalAthletes")
            .WithSummary("List digital athletes")
            .WithDescription("Get a paginated list of digital athletes in the catalog")
            .WithTags("Athletes");

        api.MapGet("/{id:int}", GetAthleteById)
            .WithName("GetDigitalAthlete")
            .WithSummary("Get digital athlete details")
            .WithDescription("Get detailed information about a digital athlete")
            .WithTags("Athletes");

        api.MapGet("/by", GetAthletesByIds)
            .WithName("BatchGetAthletes")
            .WithSummary("Batch get digital athletes")
            .WithDescription("Get multiple digital athletes by their IDs")
            .WithTags("Athletes");

        api.MapGet("/type/{athleteType}", GetAthletesByType)
            .WithName("GetAthletesByType")
            .WithSummary("Get athletes by type")
            .WithDescription("Get all digital athletes of a specific type (e.g., Runner, Fighter)")
            .WithTags("Athletes");

        api.MapGet("/category/{categoryId:int}", GetAthletesByCategory)
            .WithName("GetAthletesByCategory")
            .WithSummary("Get athletes by category")
            .WithDescription("Get all digital athletes in a specific category")
            .WithTags("Athletes");

        api.MapGet("/featured", GetFeaturedAthletes)
            .WithName("GetFeaturedAthletes")
            .WithSummary("Get featured athletes")
            .WithDescription("Get featured digital athletes for homepage display")
            .WithTags("Athletes");

        // Download routes (require authentication)
        api.MapGet("/{id:int}/download", DownloadCharacterFile)
            .WithName("DownloadCharacter")
            .WithSummary("Download character file")
            .WithDescription("Download the character data file for a purchased digital athlete")
            .WithTags("Downloads")
            .RequireAuthorization();

        api.MapGet("/{id:int}/proto", GetProtoFile)
            .WithName("GetProtoFile")
            .WithSummary("Get proto definition")
            .WithDescription("Get the protocol buffer definition for a digital athlete")
            .WithTags("Downloads");

        api.MapGet("/{id:int}/preview", GetPreviewImage)
            .WithName("GetPreviewImage")
            .WithSummary("Get preview image")
            .WithDescription("Get the preview/thumbnail image for a digital athlete")
            .WithTags("Athletes");

        // User library routes
        api.MapGet("/my-athletes", GetMyAthletes)
            .WithName("GetMyAthletes")
            .WithSummary("Get my digital athletes")
            .WithDescription("Get all digital athletes owned by the current user")
            .WithTags("Library")
            .RequireAuthorization();

        api.MapGet("/my-athletes/{athleteId:int}/download-token", GetDownloadToken)
            .WithName("GetDownloadToken")
            .WithSummary("Get download token")
            .WithDescription("Get a secure download token for a purchased athlete")
            .WithTags("Library")
            .RequireAuthorization();

        // Category management routes
        api.MapGet("/categories", GetCategories)
            .WithName("ListCategories")
            .WithSummary("List categories")
            .WithDescription("Get all athlete categories")
            .WithTags("Categories");

        return app;
    }

    // Implementation methods

    public static async Task<Results<Ok<PaginatedItems<DigitalAthleteProduct>>, BadRequest<string>>> GetAllAthletes(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] DigitalAthletesServices services)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var totalItems = await services.Context.DigitalAthleteProducts
            .Where(p => p.IsActive)
            .LongCountAsync();

        var itemsOnPage = await services.Context.DigitalAthleteProducts
            .Where(p => p.IsActive)
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return TypedResults.Ok(new PaginatedItems<DigitalAthleteProduct>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    public static async Task<Results<Ok<DigitalAthleteProduct>, NotFound, BadRequest<string>>> GetAthleteById(
        [AsParameters] DigitalAthletesServices services,
        int id)
    {
        if (id <= 0)
        {
            return TypedResults.BadRequest("Id is not valid.");
        }

        var item = await services.Context.DigitalAthleteProducts
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (item == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(item);
    }

    public static async Task<Ok<List<DigitalAthleteProduct>>> GetAthletesByIds(
        [AsParameters] DigitalAthletesServices services,
        [FromQuery] int[] ids)
    {
        var items = await services.Context.DigitalAthleteProducts
            .Where(item => ids.Contains(item.Id))
            .Include(p => p.Category)
            .ToListAsync();

        return TypedResults.Ok(items);
    }

    public static async Task<Ok<PaginatedItems<DigitalAthleteProduct>>> GetAthletesByType(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] DigitalAthletesServices services,
        string athleteType)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var totalItems = await services.Context.DigitalAthleteProducts
            .Where(p => p.IsActive && p.AthleteType == athleteType)
            .LongCountAsync();

        var itemsOnPage = await services.Context.DigitalAthleteProducts
            .Where(p => p.IsActive && p.AthleteType == athleteType)
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return TypedResults.Ok(new PaginatedItems<DigitalAthleteProduct>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    public static async Task<Ok<PaginatedItems<DigitalAthleteProduct>>> GetAthletesByCategory(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] DigitalAthletesServices services,
        int categoryId)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var totalItems = await services.Context.DigitalAthleteProducts
            .Where(p => p.IsActive && p.CategoryId == categoryId)
            .LongCountAsync();

        var itemsOnPage = await services.Context.DigitalAthleteProducts
            .Where(p => p.IsActive && p.CategoryId == categoryId)
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return TypedResults.Ok(new PaginatedItems<DigitalAthleteProduct>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    public static async Task<Ok<List<DigitalAthleteProduct>>> GetFeaturedAthletes(
        [AsParameters] DigitalAthletesServices services,
        [FromQuery] int count = 10)
    {
        var items = await services.Context.DigitalAthleteProducts
            .Where(p => p.IsActive && p.IsFeatured)
            .Include(p => p.Category)
            .OrderByDescending(p => p.AverageRating)
            .ThenByDescending(p => p.DownloadCount)
            .Take(count)
            .ToListAsync();

        return TypedResults.Ok(items);
    }

    public static async Task<Results<PhysicalFileHttpResult, NotFound, BadRequest<string>, UnauthorizedHttpResult>> DownloadCharacterFile(
        [AsParameters] DigitalAthletesServices services,
        HttpContext httpContext,
        int id)
    {
        // Get user ID from claims
        var userId = httpContext.User.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return TypedResults.Unauthorized();
        }

        if (id <= 0)
        {
            return TypedResults.BadRequest("Id is not valid.");
        }

        // Check if user owns this athlete
        var ownership = await services.Context.UserOwnedAthletes
            .Include(u => u.AthleteProduct)
            .FirstOrDefaultAsync(u => u.UserId == userId && u.AthleteProductId == id);

        if (ownership == null)
        {
            return TypedResults.BadRequest("You do not own this digital athlete.");
        }

        var product = ownership.AthleteProduct;
        if (product == null)
        {
            return TypedResults.NotFound();
        }

        var filePath = Path.Combine(services.GetCharacterFilesPath(), product.CharacterFilePath);

        if (!File.Exists(filePath))
        {
            services.Logger.LogError("Character file not found: {FilePath}", filePath);
            return TypedResults.NotFound();
        }

        // Track download
        ownership.DownloadCount++;
        ownership.LastDownloadDate = DateTime.UtcNow;

        if (services.Options.Value.EnableDownloadTracking)
        {
            services.Context.DownloadHistory.Add(new DownloadHistory
            {
                UserId = userId,
                AthleteProductId = id,
                DownloadDate = DateTime.UtcNow,
                IpAddress = httpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = httpContext.Request.Headers.UserAgent.ToString(),
                Successful = true
            });
        }

        await services.Context.SaveChangesAsync();

        return TypedResults.PhysicalFile(filePath, "application/octet-stream", $"{product.Name}_v{product.Version}.dat");
    }

    public static async Task<Results<PhysicalFileHttpResult, NotFound, BadRequest<string>>> GetProtoFile(
        [AsParameters] DigitalAthletesServices services,
        int id)
    {
        if (id <= 0)
        {
            return TypedResults.BadRequest("Id is not valid.");
        }

        var product = await services.Context.DigitalAthleteProducts
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return TypedResults.NotFound();
        }

        var filePath = Path.Combine(services.GetProtoFilesPath(), product.ProtoFilePath);

        if (!File.Exists(filePath))
        {
            services.Logger.LogError("Proto file not found: {FilePath}", filePath);
            return TypedResults.NotFound();
        }

        return TypedResults.PhysicalFile(filePath, "text/plain", $"{product.Name}_v{product.Version}.proto");
    }

    public static async Task<Results<PhysicalFileHttpResult, NotFound, BadRequest<string>>> GetPreviewImage(
        [AsParameters] DigitalAthletesServices services,
        int id)
    {
        if (id <= 0)
        {
            return TypedResults.BadRequest("Id is not valid.");
        }

        var product = await services.Context.DigitalAthleteProducts
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null || string.IsNullOrEmpty(product.PictureFileName))
        {
            return TypedResults.NotFound();
        }

        var picturePath = Path.Combine(services.Environment.ContentRootPath, "DigitalAssets", "Pictures", product.PictureFileName);

        if (!File.Exists(picturePath))
        {
            services.Logger.LogError("Picture file not found: {PicturePath}", picturePath);
            return TypedResults.NotFound();
        }

        var mimeType = product.PictureFileName.EndsWith(".webp") ? "image/webp" : "image/jpeg";
        return TypedResults.PhysicalFile(picturePath, mimeType);
    }

    public static async Task<Results<Ok<List<UserOwnedAthlete>>, UnauthorizedHttpResult>> GetMyAthletes(
        [AsParameters] DigitalAthletesServices services,
        HttpContext httpContext)
    {
        var userId = httpContext.User.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return TypedResults.Unauthorized();
        }

        var ownedAthletes = await services.Context.UserOwnedAthletes
            .Include(u => u.AthleteProduct)
            .ThenInclude(p => p!.Category)
            .Where(u => u.UserId == userId)
            .OrderByDescending(u => u.AcquiredDate)
            .ToListAsync();

        return TypedResults.Ok(ownedAthletes);
    }

    public static async Task<Results<Ok<DownloadTokenResponse>, NotFound, UnauthorizedHttpResult, BadRequest<string>>> GetDownloadToken(
        [AsParameters] DigitalAthletesServices services,
        HttpContext httpContext,
        int athleteId)
    {
        var userId = httpContext.User.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return TypedResults.Unauthorized();
        }

        var ownership = await services.Context.UserOwnedAthletes
            .FirstOrDefaultAsync(u => u.UserId == userId && u.AthleteProductId == athleteId);

        if (ownership == null)
        {
            return TypedResults.BadRequest("You do not own this digital athlete.");
        }

        // Generate new token if needed
        if (string.IsNullOrEmpty(ownership.DownloadToken) ||
            (ownership.TokenExpiration.HasValue && ownership.TokenExpiration.Value < DateTime.UtcNow))
        {
            ownership.DownloadToken = Guid.NewGuid().ToString("N");

            if (services.Options.Value.DownloadTokenExpirationDays.HasValue)
            {
                ownership.TokenExpiration = DateTime.UtcNow.AddDays(services.Options.Value.DownloadTokenExpirationDays.Value);
            }

            await services.Context.SaveChangesAsync();
        }

        return TypedResults.Ok(new DownloadTokenResponse
        {
            Token = ownership.DownloadToken,
            ExpiresAt = ownership.TokenExpiration
        });
    }

    public static async Task<Ok<List<AthleteCategory>>> GetCategories(
        [AsParameters] DigitalAthletesServices services)
    {
        var categories = await services.Context.AthleteCategories
            .OrderBy(c => c.Name)
            .ToListAsync();

        return TypedResults.Ok(categories);
    }
}

public record PaginationRequest(int PageSize = 10, int PageIndex = 0);

public class PaginatedItems<T>(int pageIndex, int pageSize, long count, IEnumerable<T> data)
{
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize;
    public long Count { get; } = count;
    public IEnumerable<T> Data { get; } = data;
}

public record DownloadTokenResponse
{
    public required string Token { get; init; }
    public DateTime? ExpiresAt { get; init; }
}
