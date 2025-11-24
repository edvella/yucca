var builder = DistributedApplication.CreateBuilder(args);

var sqldb = builder.AddConnectionString("YuccaDbConnection");

var webapi = builder.AddProject <Projects.Yucca_WebAPI> ("webapi")
    .WaitFor(sqldb)
    .WithReference(sqldb);

builder.AddProject <Projects.Yucca_Web> ("webui")
    .WithExternalHttpEndpoints()
    .WithReference(webapi)
    .WaitFor(webapi);

builder.Build().Run();
