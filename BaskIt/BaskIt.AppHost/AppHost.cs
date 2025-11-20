var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL database with a named database
var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()  // Optional: Adds PgAdmin for database management UI
    .AddDatabase("baskitdb");

// Add API project and pass database reference
builder.AddProject<Projects.BaskIt_API>("baskit-api")
    .WithReference(postgres);

builder.Build().Run();
