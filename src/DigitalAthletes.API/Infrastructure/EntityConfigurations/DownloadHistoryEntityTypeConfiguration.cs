namespace eShop.DigitalAthletes.API.Infrastructure.EntityConfigurations;

class DownloadHistoryEntityTypeConfiguration
    : IEntityTypeConfiguration<DownloadHistory>
{
    public void Configure(EntityTypeBuilder<DownloadHistory> builder)
    {
        builder.ToTable("DownloadHistory");

        builder.Property(h => h.UserId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(h => h.IpAddress)
            .HasMaxLength(50);

        builder.Property(h => h.UserAgent)
            .HasMaxLength(500);

        builder.Property(h => h.ErrorMessage)
            .HasMaxLength(1000);

        builder.HasIndex(h => h.UserId);
        builder.HasIndex(h => h.AthleteProductId);
        builder.HasIndex(h => h.DownloadDate);
    }
}
