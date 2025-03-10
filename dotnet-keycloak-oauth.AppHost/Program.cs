var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.dotnet_keycloak_oauth_Auth>("Auth");

builder.Build().Run();
