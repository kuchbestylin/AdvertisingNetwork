using CampaignEvaluationConsumerService;
using CampaignEvaluationConsumerService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateSlimBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();


var app = builder.Build();

app.UseRouting();
app.Run();
