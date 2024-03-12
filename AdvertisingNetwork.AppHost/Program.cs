var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedisContainer("redis-cache");
var rabbitmq = builder.AddRabbitMQContainer("rabbitmq-container");

var dataservice = builder.AddProject<Projects.DataService>("dataservice")
    .WithReplicas(5)
    .WithReference(redis);

builder.AddProject<Projects.AdService>("adservice")
    .WithReplicas(3)
    .WithReference(redis)
    .WithReference(dataservice);

builder.AddProject<Projects.ContentService>("contentservice")
    .WithReplicas(3)
    .WithReference(redis)
    .WithReference(dataservice);

builder.AddProject<Projects.AdSelectorService>("adselectorservice")
    .WithReplicas(3)
    .WithReference(redis)
    .WithReference(dataservice);

builder.AddProject<Projects.BiddingService>("biddingservice")
    .WithReplicas(3)
    .WithReference(dataservice);

builder.AddProject<Projects.AdProviderService>("adproviderservice")
    .WithReplicas(3)
    .WithReference(dataservice);

builder.AddProject<Projects.CampaignEvaluationConsumerService>("campaignevaluationconsumerservice")
    .WithReplicas(5)
    .WithReference(redis)
    .WithReference(rabbitmq)
    .WithReference(dataservice);


builder.AddProject<Projects.IndentityServer>("indentityserver")
    .WithHttpsEndpoint(7113, "identityserver");


builder.AddProject<Projects.weatherapi>("weatherapi")
    .WithHttpsEndpoint(7264, "weatherapi");


builder.AddProject<Projects.Test>("test")
    .WithHttpsEndpoint(7002, "test");


builder.AddProject<Projects.BlazorApp1>("blazorapp1");


builder.AddProject<Projects.BlazorApp2>("blazorapp2");


builder.Build().Run();
