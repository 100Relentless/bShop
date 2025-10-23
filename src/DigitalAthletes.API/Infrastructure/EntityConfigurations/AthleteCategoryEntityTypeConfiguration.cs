namespace eShop.DigitalAthletes.API.Infrastructure.EntityConfigurations;

class AthleteCategoryEntityTypeConfiguration
    : IEntityTypeConfiguration<AthleteCategory>
{
    public void Configure(EntityTypeBuilder<AthleteCategory> builder)
    {
        builder.ToTable("AthleteCategories");

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.HasIndex(c => c.Name)
            .IsUnique();
    }
}
