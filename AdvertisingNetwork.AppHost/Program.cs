var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedisContainer("redis-cache");
var rabbitmq = builder.AddRabbitMQContainer("rabbitmq-container");

var dataservice = builder.AddProject<Projects.DataService>("dataservice")
    .WithReference(redis);

builder.AddProject<Projects.AdService>("adservice")
    .WithHttpsEndpoint(7100)
    .WithReference(redis)
    .WithReference(dataservice);

builder.AddProject<Projects.ContentService>("contentservice")
    .WithHttpsEndpoint(7200)
    .WithReference(redis)
    .WithReference(dataservice);

//builder.AddProject<Projects.AdSelectorService>("adselectorservice")
//    .WithHttpsEndpoint(7300)
//    .WithReference(redis)
//    .WithReference(dataservice);

//builder.AddProject<Projects.BiddingService>("biddingservice")
//    .WithHttpsEndpoint(7400)
//    .WithReference(dataservice);

builder.AddProject<Projects.AdProviderService>("adproviderservice")
    .WithHttpsEndpoint(7500, "adproviderservice")
    .WithReference(dataservice);


builder.AddProject<Projects.IndentityServer>("indentityserver")
    .WithHttpsEndpoint(7113, "identityserver");


builder.AddProject<Projects.weatherapi>("weatherapi")
    .WithHttpsEndpoint(7264, "weatherapi");


builder.AddProject<Projects.Test>("test")
    .WithHttpsEndpoint(7002, "test");


builder.AddProject<Projects.TestPublisherProject>("testpublisherproject")
    .WithHttpsEndpoint(7600, "testpublisherproject");


builder.AddProject<Projects.AdEventingSignalRService>("adeventingsignalrservice")
    .WithHttpsEndpoint(7700, "adeventingsignalrservice");


builder.AddProject<Projects.AdvertisersWebService>("advertiserswebservice");


builder.Build().Run();
