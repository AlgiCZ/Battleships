var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Battleships_ApiService>("apiservice");

builder.AddProject<Projects.Battleships_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
