namespace eShop.DigitalAthletes.API.Model;

public class DigitalAthletesServices(
    DigitalAthletesContext context,
    IOptions<DigitalAthletesOptions> options,
    IWebHostEnvironment environment,
    ILogger<DigitalAthletesServices> logger)
{
    public DigitalAthletesContext Context { get; } = context;
    public IOptions<DigitalAthletesOptions> Options { get; } = options;
    public IWebHostEnvironment Environment { get; } = environment;
    public ILogger<DigitalAthletesServices> Logger { get; } = logger;

    public string GetCharacterFilesPath() =>
        Path.Combine(Environment.ContentRootPath, "DigitalAssets", "Characters");

    public string GetProtoFilesPath() =>
        Path.Combine(Environment.ContentRootPath, "DigitalAssets", "Protos");
}

public class DigitalAthletesOptions
{
    /// <summary>
    /// Maximum number of downloads per user per athlete (0 = unlimited)
    /// </summary>
    public int MaxDownloadsPerAthlete { get; set; } = 0;

    /// <summary>
    /// Download token expiration in days (null = never expires)
    /// </summary>
    public int? DownloadTokenExpirationDays { get; set; } = null;

    /// <summary>
    /// Whether to track download history
    /// </summary>
    public bool EnableDownloadTracking { get; set; } = true;

    /// <summary>
    /// Maximum file size for character files in MB
    /// </summary>
    public int MaxCharacterFileSizeMB { get; set; } = 50;
}
