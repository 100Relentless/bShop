namespace eShop.DigitalAthletes.API.Infrastructure.EntityConfigurations;

class DigitalAthleteProductEntityTypeConfiguration
    : IEntityTypeConfiguration<DigitalAthleteProduct>
{
    public void Configure(EntityTypeBuilder<DigitalAthleteProduct> builder)
    {
        builder.ToTable("DigitalAthleteProducts");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.Price)
            .HasPrecision(18, 2);

        builder.Property(p => p.AthleteType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Version)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.CharacterFilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.ProtoFilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.PictureFileName)
            .HasMaxLength(200);

        builder.Property(p => p.SupportedGameModes)
            .HasMaxLength(500);

        builder.Property(p => p.AverageRating)
            .HasPrecision(3, 2);

        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.AthleteType);
        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.IsActive);
    }
}
