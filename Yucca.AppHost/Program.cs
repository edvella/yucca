var builder = DistributedApplication.CreateBuilder(args);

var webapi = builder.AddProject <Projects.Yucca_WebAPI> ("webapi");

builder.AddProject <Projects.Yucca_Web> ("webui")
    .WithExternalHttpEndpoints()
    .WithReference(webapi)
    .WaitFor(webapi);

builder.Build().Run();
