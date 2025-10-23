namespace eShop.DigitalAthletes.API.Infrastructure.EntityConfigurations;

class UserOwnedAthleteEntityTypeConfiguration
    : IEntityTypeConfiguration<UserOwnedAthlete>
{
    public void Configure(EntityTypeBuilder<UserOwnedAthlete> builder)
    {
        builder.ToTable("UserOwnedAthletes");

        builder.Property(u => u.UserId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.DownloadToken)
            .HasMaxLength(500);

        builder.HasOne(u => u.AthleteProduct)
            .WithMany()
            .HasForeignKey(u => u.AthleteProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ensure a user can only own one copy of each athlete
        builder.HasIndex(u => new { u.UserId, u.AthleteProductId })
            .IsUnique();

        builder.HasIndex(u => u.UserId);
        builder.HasIndex(u => u.DownloadToken);
    }
}
