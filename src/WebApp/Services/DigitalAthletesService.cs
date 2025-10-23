namespace eShop.WebApp.Services;

public class DigitalAthletesService(HttpClient httpClient)
{
    private readonly string baseUrl = "/api/digital-athletes/";

    public Task<PaginatedItems<DigitalAthleteProduct>> GetAthletes(int pageIndex = 0, int pageSize = 10)
    {
        var url = $"{baseUrl}?PageIndex={pageIndex}&PageSize={pageSize}";
        return httpClient.GetFromJsonAsync<PaginatedItems<DigitalAthleteProduct>>(url)!;
    }

    public Task<DigitalAthleteProduct?> GetAthleteById(int id)
    {
        return httpClient.GetFromJsonAsync<DigitalAthleteProduct>($"{baseUrl}{id}");
    }

    public Task<List<DigitalAthleteProduct>> GetAthletesByType(string athleteType)
    {
        return httpClient.GetFromJsonAsync<List<DigitalAthleteProduct>>($"{baseUrl}type/{athleteType}")!;
    }

    public Task<List<DigitalAthleteProduct>> GetFeaturedAthletes(int count = 10)
    {
        return httpClient.GetFromJsonAsync<List<DigitalAthleteProduct>>($"{baseUrl}featured?count={count}")!;
    }

    public Task<List<UserOwnedAthlete>> GetMyAthletes()
    {
        return httpClient.GetFromJsonAsync<List<UserOwnedAthlete>>($"{baseUrl}my-athletes")!;
    }

    public Task<DownloadTokenResponse?> GetDownloadToken(int athleteId)
    {
        return httpClient.GetFromJsonAsync<DownloadTokenResponse>($"{baseUrl}my-athletes/{athleteId}/download-token");
    }

    public string GetDownloadUrl(int athleteId)
    {
        return $"{baseUrl}{athleteId}/download";
    }

    public string GetPreviewImageUrl(int athleteId)
    {
        return $"{baseUrl}{athleteId}/preview";
    }

    public string GetProtoFileUrl(int athleteId)
    {
        return $"{baseUrl}{athleteId}/proto";
    }

    public Task<List<AthleteCategory>> GetCategories()
    {
        return httpClient.GetFromJsonAsync<List<AthleteCategory>>($"{baseUrl}categories")!;
    }
}

public record PaginatedItems<T>(
    int PageIndex,
    int PageSize,
    long Count,
    IEnumerable<T> Data);

public record DigitalAthleteProduct
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public string? PictureFileName { get; init; }
    public string AthleteType { get; init; } = string.Empty;
    public int CategoryId { get; init; }
    public AthleteCategory? Category { get; init; }
    public string Version { get; init; } = string.Empty;
    public long FileSizeBytes { get; init; }
    public string? SupportedGameModes { get; init; }
    public int MaxPlayersPerSession { get; init; }
    public bool IsFeatured { get; init; }
    public decimal? AverageRating { get; init; }
}

public record AthleteCategory
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}

public record UserOwnedAthlete
{
    public int Id { get; init; }
    public string UserId { get; init; } = string.Empty;
    public int AthleteProductId { get; init; }
    public DigitalAthleteProduct? AthleteProduct { get; init; }
    public DateTime AcquiredDate { get; init; }
    public int? OrderId { get; init; }
    public int DownloadCount { get; init; }
    public DateTime? LastDownloadDate { get; init; }
}

public record DownloadTokenResponse
{
    public string Token { get; init; } = string.Empty;
    public DateTime? ExpiresAt { get; init; }
}
