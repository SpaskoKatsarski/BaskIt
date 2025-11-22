var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume(isReadOnly: false)
    .WithPgWeb(config => config.WithHostPort(5050));

var postgresdb = postgres.AddDatabase("baskitdb");

builder.AddProject<Projects.BaskIt_API>("baskit-api")
    .WithReference(postgresdb);

builder.Build().Run();