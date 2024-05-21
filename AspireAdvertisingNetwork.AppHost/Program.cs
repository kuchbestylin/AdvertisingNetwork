var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis-cache");

var identityserver = builder.AddProject<Projects.IndentityServer>("identityserver")
    .WithReplicas(3);

var datastore = builder.AddProject<Projects.DataStore>("datastore")
    .WithReference(redis);

var sellside = builder.AddProject<Projects.AdvertisersWebService>("advertiserswebservice")
    .WithReference(identityserver)
    .WithReference(datastore);

var demandside = builder.AddProject<Projects.ContentService>("contentservice")
    .WithReference(identityserver)
    .WithReference(datastore);

    identityserver.WithReference(sellside).WithReference(demandside);

builder.AddProject<Projects.AdProviderService>("adprovider")
    .WithReference(datastore);

var testpublisher = builder.AddProject<Projects.TestPublisherProject>("textpublisher");

builder.AddProject<Projects.AdEventingSignalRService>("signalrservice")
    .WithReference(testpublisher);

builder.Build().Run();
