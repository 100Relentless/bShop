namespace eShop.DigitalAthletes.API.Model;

/// <summary>
/// Digital athlete product in the marketplace catalog
/// </summary>
public class DigitalAthleteProduct
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    /// <summary>
    /// URL to the preview image/thumbnail
    /// </summary>
    public string? PictureFileName { get; set; }

    /// <summary>
    /// Type of athlete (e.g., "Runner", "Fighter", "Racer", "Jumper")
    /// </summary>
    public required string AthleteType { get; set; }

    /// <summary>
    /// Category or brand of the digital athlete
    /// </summary>
    public int CategoryId { get; set; }
    public AthleteCategory? Category { get; set; }

    /// <summary>
    /// Version of the character file format
    /// </summary>
    public required string Version { get; set; }

    /// <summary>
    /// Relative path to the character data file
    /// </summary>
    public required string CharacterFilePath { get; set; }

    /// <summary>
    /// Relative path to the proto definition file
    /// </summary>
    public required string ProtoFilePath { get; set; }

    /// <summary>
    /// Size of the character file in bytes
    /// </summary>
    public long FileSizeBytes { get; set; }

    /// <summary>
    /// Available stock for this digital product
    /// (Usually unlimited for digital, but can limit sales)
    /// </summary>
    public int AvailableStock { get; set; }

    /// <summary>
    /// Supported game modes
    /// </summary>
    public string? SupportedGameModes { get; set; } // JSON array or comma-separated

    /// <summary>
    /// Maximum number of players that can use this character in a session
    /// </summary>
    public int MaxPlayersPerSession { get; set; }

    /// <summary>
    /// Featured flag for homepage display
    /// </summary>
    public bool IsFeatured { get; set; }

    /// <summary>
    /// When the product was added to the catalog
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// When the product was last updated
    /// </summary>
    public DateTime? UpdatedDate { get; set; }

    /// <summary>
    /// Number of times this athlete has been downloaded
    /// </summary>
    public int DownloadCount { get; set; }

    /// <summary>
    /// Average rating from users (0-5 stars)
    /// </summary>
    public decimal? AverageRating { get; set; }

    /// <summary>
    /// Whether this product is currently available for purchase
    /// </summary>
    public bool IsActive { get; set; }
}

/// <summary>
/// Category for grouping digital athletes
/// </summary>
public class AthleteCategory
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}

/// <summary>
/// Tracks which users own which digital athletes
/// </summary>
public class UserOwnedAthlete
{
    public int Id { get; set; }

    /// <summary>
    /// User identifier (from Identity service)
    /// </summary>
    public required string UserId { get; set; }

    /// <summary>
    /// The digital athlete product owned
    /// </summary>
    public int AthleteProductId { get; set; }
    public DigitalAthleteProduct? AthleteProduct { get; set; }

    /// <summary>
    /// When the user acquired this athlete (purchase date)
    /// </summary>
    public DateTime AcquiredDate { get; set; }

    /// <summary>
    /// Order ID that granted this athlete
    /// </summary>
    public int? OrderId { get; set; }

    /// <summary>
    /// Secure download token for this user's copy
    /// </summary>
    public string? DownloadToken { get; set; }

    /// <summary>
    /// Token expiration (null = never expires)
    /// </summary>
    public DateTime? TokenExpiration { get; set; }

    /// <summary>
    /// Number of times the user has downloaded this athlete
    /// </summary>
    public int DownloadCount { get; set; }

    /// <summary>
    /// Last download timestamp
    /// </summary>
    public DateTime? LastDownloadDate { get; set; }
}

/// <summary>
/// Download history for analytics and abuse prevention
/// </summary>
public class DownloadHistory
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public int AthleteProductId { get; set; }
    public DateTime DownloadDate { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public bool Successful { get; set; }
    public string? ErrorMessage { get; set; }
}
