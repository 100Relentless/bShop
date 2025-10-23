# Digital Athletes Marketplace

A complete e-commerce microservice for selling downloadable digital athletes - tiny characters that players can use in networked multiplayer games.

## Overview

The Digital Athletes Marketplace is an integrated module for eShop that enables selling, purchasing, and downloading digital character files. These characters are designed for networked gameplay where friends can play together online.

## Key Features

### 🛍️ **E-Commerce Integration**
- Full catalog of digital athletes with descriptions, pricing, and previews
- Shopping cart integration with existing eShop basket
- Secure payment processing through existing ordering system
- Automatic delivery upon payment confirmation

### 🎮 **Character Management**
- Protocol Buffer-based character data format for efficient network transfer
- Compact character files optimized for fast downloads
- Version tracking for character updates
- Support for multiple game modes per character

### 🔒 **Security & Download Control**
- Secure download tokens for authenticated users
- Ownership tracking - users can only download what they own
- Download history and analytics
- Optional download limits and token expiration

### 🌐 **Multiplayer Support**
- Characters designed for networked gameplay
- Built-in synchronization settings (update frequency, prediction, interpolation)
- Support for multiple players per session
- Compatible with various game modes (racing, battle, cooperative, etc.)

## Architecture

### Database Schema

**DigitalAthleteProducts**
- Product catalog with pricing, descriptions, and metadata
- Links to character files and proto definitions
- Featured flag for homepage promotion
- Rating system

**AthleteCategories**
- Grouping for character types (Speed Champions, Combat Masters, etc.)

**UserOwnedAthletes**
- Tracks user ownership and grants access
- Secure download tokens
- Download statistics

**DownloadHistory**
- Analytics and abuse prevention
- Tracks successful/failed downloads

### API Endpoints

**Catalog Browsing**
- `GET /api/digital-athletes/` - Paginated athlete list
- `GET /api/digital-athletes/{id}` - Athlete details
- `GET /api/digital-athletes/type/{type}` - Filter by athlete type
- `GET /api/digital-athletes/category/{categoryId}` - Filter by category
- `GET /api/digital-athletes/featured` - Featured athletes

**Downloads & Access**
- `GET /api/digital-athletes/{id}/download` - Download character file (auth required)
- `GET /api/digital-athletes/{id}/proto` - Get proto definition
- `GET /api/digital-athletes/{id}/preview` - Preview image
- `GET /api/digital-athletes/my-athletes` - User's library (auth required)

### Character Data Format

Characters use **Protocol Buffers** for efficient serialization:

```proto
message DigitalAthlete {
  string id = 1;
  string name = 2;
  Appearance appearance = 4;      // Colors, size, visual style
  Attributes attributes = 5;       // Speed, strength, agility, etc.
  repeated Animation animations = 6;  // Movement, actions, celebrations
  SyncData sync_data = 7;         // Network synchronization settings
}
```

### Integration Events

**OrderStatusChangedToPaid**
- Automatically grants character ownership when order is paid
- Creates download tokens
- Enables immediate access to purchased athletes

## Sample Athletes

The seed data includes 8 diverse athletes:

1. **Lightning Bolt** (Runner) - $4.99
   - Tiny speedster with incredible acceleration
   - Perfect for racing games

2. **Shadow Striker** (Fighter) - $5.99
   - Stealthy combat athlete with quick reflexes
   - Ideal for battle modes

3. **Bounce Master** (Jumper) - $3.99
   - Expert jumper for platformers
   - Great height and control

4. **Team Captain** (All-Rounder) - $6.99
   - Leadership abilities that boost teammates
   - Balanced stats for team play

5. **Turbo Racer** (Racer) - $4.49
   - Built for speed in time trials
   - Supports up to 12 players

6. **Power Punch** (Fighter) - $5.49
   - Heavy hitter with devastating specials
   - Tournament ready

7. **Pixel Ranger** (All-Rounder) - $2.99
   - Classic 8-bit style
   - Versatile for all game modes

8. **Quantum Dash** (Runner) - $7.99
   - Futuristic with teleportation abilities
   - Premium features

## Configuration

**appsettings.json**
```json
{
  "DigitalAthletesOptions": {
    "MaxDownloadsPerAthlete": 0,              // 0 = unlimited
    "DownloadTokenExpirationDays": null,      // null = never expires
    "EnableDownloadTracking": true,
    "MaxCharacterFileSizeMB": 50
  }
}
```

## Development

### Running the Service

The service is automatically configured in the AppHost:

```csharp
var digitalAthletesApi = builder.AddProject<Projects.DigitalAthletes_API>("digital-athletes-api")
    .WithReference(rabbitMq).WaitFor(rabbitMq)
    .WithReference(digitalAthletesDb)
    .WithEnvironment("Identity__Url", identityEndpoint);
```

### Database Migrations

Create migrations from the DigitalAthletes.API directory:

```bash
dotnet ef migrations add InitialCreate --context DigitalAthletesContext
```

### Adding New Athletes

1. Create character data file in `DigitalAssets/Characters/`
2. Add proto definition in `DigitalAssets/Protos/`
3. Add preview image in `DigitalAssets/Pictures/`
4. Seed database or use admin API (future feature)

## Use Cases

### Player Perspective
1. Browse catalog of digital athletes
2. Add favorite athlete to shopping cart
3. Complete purchase through checkout
4. Receive instant access to download
5. Download character file and proto definition
6. Import into compatible game
7. Play with friends in multiplayer sessions

### Game Developer Perspective
1. Create compatible game using provided proto definitions
2. Deserialize character files at runtime
3. Use character attributes for gameplay mechanics
4. Leverage network sync settings for multiplayer
5. Support cross-platform play with standard format

## Technical Stack

- **.NET 10** - Modern C# features
- **ASP.NET Core** - Minimal APIs with versioning
- **Entity Framework Core** - PostgreSQL integration
- **Protocol Buffers** - Efficient data serialization
- **RabbitMQ** - Event-driven architecture
- **Aspire** - Orchestration and service discovery

## Future Enhancements

- [ ] Character customization API
- [ ] User reviews and ratings
- [ ] Bundle discounts
- [ ] Character trading marketplace
- [ ] Seasonal/limited edition athletes
- [ ] Character builder tool
- [ ] Game SDK for easy integration
- [ ] Mobile app for library management
- [ ] Social features (friend lists, leaderboards)
- [ ] Tournament support

## License

Part of eShop - see main repository for license details.
