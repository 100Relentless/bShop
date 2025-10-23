using System.Text.Json;

namespace eShop.DigitalAthletes.API.Infrastructure;

public class DigitalAthletesContextSeed(
    IWebHostEnvironment env,
    ILogger<DigitalAthletesContextSeed> logger) : IDbSeeder<DigitalAthletesContext>
{
    public async Task SeedAsync(DigitalAthletesContext context)
    {
        var contentRootPath = env.ContentRootPath;

        if (!context.AthleteCategories.Any())
        {
            var categories = new[]
            {
                new AthleteCategory { Name = "Speed Champions", Description = "Lightning-fast athletes for racing and speed challenges" },
                new AthleteCategory { Name = "Combat Masters", Description = "Skilled fighters with powerful combat abilities" },
                new AthleteCategory { Name = "Platform Heroes", Description = "Agile jumpers perfect for platform adventures" },
                new AthleteCategory { Name = "Team Players", Description = "Cooperative athletes for team-based gameplay" },
                new AthleteCategory { Name = "All-Rounders", Description = "Balanced athletes suitable for any game mode" }
            };

            await context.AthleteCategories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {NumCategories} athlete categories", categories.Length);
        }

        if (!context.DigitalAthleteProducts.Any())
        {
            var categoryIds = await context.AthleteCategories.ToDictionaryAsync(c => c.Name, c => c.Id);

            var sampleAthletes = new[]
            {
                new DigitalAthleteProduct
                {
                    Name = "Lightning Bolt",
                    Description = "Tiny speedster with incredible acceleration. Perfect for racing games!",
                    Price = 4.99m,
                    AthleteType = "Runner",
                    CategoryId = categoryIds["Speed Champions"],
                    Version = "1.0.0",
                    CharacterFilePath = "lightning_bolt_v1.dat",
                    ProtoFilePath = "athlete_v1.proto",
                    FileSizeBytes = 512000, // 500 KB
                    AvailableStock = 999999,
                    SupportedGameModes = "race,time-trial,multiplayer-race",
                    MaxPlayersPerSession = 8,
                    IsFeatured = true,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    AverageRating = 4.8m,
                    PictureFileName = "lightning_bolt.webp"
                },
                new DigitalAthleteProduct
                {
                    Name = "Shadow Striker",
                    Description = "Stealthy combat athlete with quick reflexes and powerful strikes.",
                    Price = 5.99m,
                    AthleteType = "Fighter",
                    CategoryId = categoryIds["Combat Masters"],
                    Version = "1.0.0",
                    CharacterFilePath = "shadow_striker_v1.dat",
                    ProtoFilePath = "athlete_v1.proto",
                    FileSizeBytes = 768000, // 750 KB
                    AvailableStock = 999999,
                    SupportedGameModes = "battle,team-battle,tournament",
                    MaxPlayersPerSession = 4,
                    IsFeatured = true,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    AverageRating = 4.9m,
                    PictureFileName = "shadow_striker.webp"
                },
                new DigitalAthleteProduct
                {
                    Name = "Bounce Master",
                    Description = "Expert jumper who can reach incredible heights. Great for platformers!",
                    Price = 3.99m,
                    AthleteType = "Jumper",
                    CategoryId = categoryIds["Platform Heroes"],
                    Version = "1.0.0",
                    CharacterFilePath = "bounce_master_v1.dat",
                    ProtoFilePath = "athlete_v1.proto",
                    FileSizeBytes = 410000, // 400 KB
                    AvailableStock = 999999,
                    SupportedGameModes = "platformer,adventure,challenge",
                    MaxPlayersPerSession = 4,
                    IsFeatured = false,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    AverageRating = 4.6m,
                    PictureFileName = "bounce_master.webp"
                },
                new DigitalAthleteProduct
                {
                    Name = "Team Captain",
                    Description = "Balanced athlete with leadership abilities. Boosts nearby teammates!",
                    Price = 6.99m,
                    AthleteType = "AllRounder",
                    CategoryId = categoryIds["Team Players"],
                    Version = "1.0.0",
                    CharacterFilePath = "team_captain_v1.dat",
                    ProtoFilePath = "athlete_v1.proto",
                    FileSizeBytes = 890000, // 870 KB
                    AvailableStock = 999999,
                    SupportedGameModes = "team-battle,coop,tournament",
                    MaxPlayersPerSession = 8,
                    IsFeatured = true,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    AverageRating = 4.7m,
                    PictureFileName = "team_captain.webp"
                },
                new DigitalAthleteProduct
                {
                    Name = "Turbo Racer",
                    Description = "Built for speed! Dominates in racing and time trials.",
                    Price = 4.49m,
                    AthleteType = "Racer",
                    CategoryId = categoryIds["Speed Champions"],
                    Version = "1.0.0",
                    CharacterFilePath = "turbo_racer_v1.dat",
                    ProtoFilePath = "athlete_v1.proto",
                    FileSizeBytes = 620000, // 605 KB
                    AvailableStock = 999999,
                    SupportedGameModes = "race,time-trial,championship",
                    MaxPlayersPerSession = 12,
                    IsFeatured = false,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    AverageRating = 4.5m,
                    PictureFileName = "turbo_racer.webp"
                },
                new DigitalAthleteProduct
                {
                    Name = "Power Punch",
                    Description = "Heavy hitter with devastating special moves. A crowd favorite!",
                    Price = 5.49m,
                    AthleteType = "Fighter",
                    CategoryId = categoryIds["Combat Masters"],
                    Version = "1.0.0",
                    CharacterFilePath = "power_punch_v1.dat",
                    ProtoFilePath = "athlete_v1.proto",
                    FileSizeBytes = 710000, // 693 KB
                    AvailableStock = 999999,
                    SupportedGameModes = "battle,tournament,versus",
                    MaxPlayersPerSession = 4,
                    IsFeatured = true,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    AverageRating = 4.8m,
                    PictureFileName = "power_punch.webp"
                },
                new DigitalAthleteProduct
                {
                    Name = "Pixel Ranger",
                    Description = "Classic 8-bit style athlete. Nostalgic and versatile!",
                    Price = 2.99m,
                    AthleteType = "AllRounder",
                    CategoryId = categoryIds["All-Rounders"],
                    Version = "1.0.0",
                    CharacterFilePath = "pixel_ranger_v1.dat",
                    ProtoFilePath = "athlete_v1.proto",
                    FileSizeBytes = 256000, // 250 KB (small retro style)
                    AvailableStock = 999999,
                    SupportedGameModes = "platformer,adventure,battle,race",
                    MaxPlayersPerSession = 8,
                    IsFeatured = false,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    AverageRating = 4.4m,
                    PictureFileName = "pixel_ranger.webp"
                },
                new DigitalAthleteProduct
                {
                    Name = "Quantum Dash",
                    Description = "Futuristic athlete with teleportation abilities. Ultra-modern gameplay!",
                    Price = 7.99m,
                    AthleteType = "Runner",
                    CategoryId = categoryIds["Speed Champions"],
                    Version = "1.0.0",
                    CharacterFilePath = "quantum_dash_v1.dat",
                    ProtoFilePath = "athlete_v1.proto",
                    FileSizeBytes = 1024000, // 1 MB (advanced features)
                    AvailableStock = 999999,
                    SupportedGameModes = "race,time-trial,challenge,adventure",
                    MaxPlayersPerSession = 6,
                    IsFeatured = true,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    AverageRating = 4.9m,
                    PictureFileName = "quantum_dash.webp"
                }
            };

            await context.DigitalAthleteProducts.AddRangeAsync(sampleAthletes);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {NumAthletes} digital athlete products", sampleAthletes.Length);
        }
    }
}
