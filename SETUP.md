# BaskIt - Aspire Setup with ASP.NET Core Identity & PostgreSQL

## What You Have Now

### Architecture
- **BaskIt.AppHost**: Aspire orchestrator that manages your services
- **BaskIt.API**: Web API with ASP.NET Core Identity authentication
- **PostgreSQL**: Database running in Docker (managed by Aspire)
- **PgAdmin**: Database management UI (optional, accessible via Aspire Dashboard)

### Authentication Setup
- ASP.NET Core Identity with custom `ApplicationUser`
- Cookie-based authentication (sessions)
- Password requirements: 8+ chars, upper/lower case, digit, special character
- Account lockout after 5 failed attempts
- Unique email requirement

## How to Run

### 1. Start the Aspire AppHost
```bash
cd BaskIt/BaskIt.AppHost
dotnet run
```

This will:
- Start the Aspire Dashboard (usually at http://localhost:15888)
- Spin up PostgreSQL in Docker automatically
- Spin up PgAdmin for database management
- Start your API

### 2. Apply Database Migrations
The migrations are created but need to be applied. You have two options:

**Option A: Manual (before first run)**
```bash
cd BaskIt/BaskIt.API
dotnet ef database update
```

**Option B: Automatic (on app startup)**
Add this to `Program.cs` after `var app = builder.Build();`:
```csharp
// Auto-apply migrations on startup (dev only)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
}
```

### 3. Access the Aspire Dashboard
Open the dashboard URL shown in the console (typically http://localhost:15888)

From here you can:
- See all running services (API, PostgreSQL, PgAdmin)
- View logs in real-time
- Monitor metrics and traces
- Access each service directly

## Testing the API

### Register a New User
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "SecurePass123!",
    "confirmPassword": "SecurePass123!"
  }'
```

### Login
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -c cookies.txt \
  -d '{
    "email": "user@example.com",
    "password": "SecurePass123!",
    "rememberMe": false
  }'
```

### Get Current User (Authenticated)
```bash
curl -X GET http://localhost:5000/api/auth/me \
  -H "Content-Type: application/json" \
  -b cookies.txt
```

### Logout
```bash
curl -X POST http://localhost:5000/api/auth/logout \
  -b cookies.txt
```

## Database Access

### Via PgAdmin (Web UI)
1. Open Aspire Dashboard
2. Click on the PgAdmin service
3. Default credentials are shown in the dashboard
4. Add server connection:
   - Host: postgres (service name)
   - Port: 5432
   - Database: basketdb
   - Username: postgres
   - Password: (shown in Aspire Dashboard)

### Via Connection String
Check the Aspire Dashboard for the exact connection string, or use:
```
Host=localhost;Port=<dynamic_port>;Database=basketdb;Username=postgres;Password=<generated>
```

## Project Structure

```
BaskIt/
├── BaskIt.AppHost/
│   └── AppHost.cs              # Aspire orchestration
├── BaskIt.ServiceDefaults/
│   └── Extensions.cs           # Shared services (telemetry, health checks)
└── BaskIt.API/
    ├── Controllers/
    │   └── AuthController.cs   # Authentication endpoints
    ├── Data/
    │   ├── ApplicationDbContext.cs
    │   └── ApplicationUser.cs  # Custom user model
    ├── Models/
    │   ├── LoginRequest.cs
    │   └── RegisterRequest.cs
    ├── Migrations/             # EF Core migrations
    └── Program.cs              # App configuration
```

## Next Steps

### Add Custom User Properties
Edit `Data/ApplicationUser.cs`:
```csharp
public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

Then create a new migration:
```bash
dotnet ef migrations add AddUserProperties
dotnet ef database update
```

### Add JWT Tokens (for mobile/SPA)
If you need token-based auth instead of cookies:
1. Install: `dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer`
2. Configure JWT in `Program.cs`
3. Update `AuthController` to return tokens

### Add OAuth Providers (Google, Microsoft, etc.)
```bash
dotnet add package Microsoft.AspNetCore.Authentication.Google
```

### Add Email Confirmation
1. Configure email service (SendGrid, SMTP, etc.)
2. Use `UserManager.GenerateEmailConfirmationTokenAsync()`
3. Send confirmation link to user
4. Verify with `UserManager.ConfirmEmailAsync()`

## Aspire Benefits You're Using

1. **Service Discovery**: API automatically connects to PostgreSQL via service name
2. **OpenTelemetry**: Automatic logging, metrics, and distributed tracing
3. **Health Checks**: Built-in `/health` and `/alive` endpoints
4. **Resilience**: Automatic retry policies and circuit breakers
5. **Dashboard**: Real-time monitoring of all services
6. **Docker Management**: No manual container orchestration needed

## Common Tasks

### View Logs
- Open Aspire Dashboard
- Click on any service to see structured logs in real-time

### Reset Database
```bash
dotnet ef database drop
dotnet ef database update
```

### Add a New Service
In `AppHost.cs`:
```csharp
// Add Redis
var redis = builder.AddRedis("cache");

// Add another API
builder.AddProject<Projects.BaskIt_Admin>("admin-api")
    .WithReference(postgres)
    .WithReference(redis);
```

### Deploy to Production
Aspire can generate:
- Docker Compose files
- Kubernetes manifests
- Azure Container Apps configuration

Run: `dotnet publish` and follow Aspire deployment guides.

## Troubleshooting

**PostgreSQL container not starting?**
- Make sure Docker is running
- Check ports aren't already in use

**Migrations failing?**
- Ensure connection string is correct
- Check database is running in Docker

**Authentication not working?**
- Verify cookies are being sent/received
- Check CORS settings if calling from different origin
- Ensure `UseAuthentication()` is before `UseAuthorization()`

## Resources

- [.NET Aspire Docs](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [EF Core with PostgreSQL](https://www.npgsql.org/efcore/)
