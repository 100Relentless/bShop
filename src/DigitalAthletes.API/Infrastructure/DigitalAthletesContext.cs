namespace eShop.DigitalAthletes.API.Infrastructure;

/// <remarks>
/// Add migrations using the following command inside the 'DigitalAthletes.API' project directory:
///
/// dotnet ef migrations add --context DigitalAthletesContext [migration-name]
/// </remarks>
public class DigitalAthletesContext : DbContext
{
    public DigitalAthletesContext(DbContextOptions<DigitalAthletesContext> options) : base(options)
    {
    }

    public required DbSet<DigitalAthleteProduct> DigitalAthleteProducts { get; set; }
    public required DbSet<AthleteCategory> AthleteCategories { get; set; }
    public required DbSet<UserOwnedAthlete> UserOwnedAthletes { get; set; }
    public required DbSet<DownloadHistory> DownloadHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new DigitalAthleteProductEntityTypeConfiguration());
        builder.ApplyConfiguration(new AthleteCategoryEntityTypeConfiguration());
        builder.ApplyConfiguration(new UserOwnedAthleteEntityTypeConfiguration());
        builder.ApplyConfiguration(new DownloadHistoryEntityTypeConfiguration());

        // Add the outbox table to this context for integration events
        builder.UseIntegrationEventLogs();
    }
}
