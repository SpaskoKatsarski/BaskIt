var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume(isReadOnly: false)
    .WithPgWeb(config => config.WithHostPort(5050));

var postgresdb = postgres.AddDatabase("baskitdb");

var api = builder.AddProject<Projects.BaskIt_API>("baskit-api")
    .WithReference(postgresdb);

builder.AddNpmApp("baskit-web", "../BaskIt.Web", "dev")
    .WithReference(api)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .WithEnvironment("BROWSER", "none")
    .WithEnvironment(context =>
    {
        var apiEndpoint = api.GetEndpoint("http");
        context.EnvironmentVariables["VITE_API_URL"] = $"{apiEndpoint.Url}/api";
    });

builder.Build().Run();