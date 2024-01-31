using KafkaFlow;
using KafkaFlow.Producers;
using KafkaFlow.Serializer;
using Microsoft.AspNetCore.Mvc;
using PixelService.Config;
using PixelService.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

KafkaSettings kafkaSettings = new();
builder.Configuration.GetSection("Kafka").Bind(kafkaSettings);

builder.Services.AddKafka(
    kafka => kafka
    .AddCluster(
        cluster => cluster
                .WithBrokers(kafkaSettings.Brokers)
                .CreateTopicIfNotExists(kafkaSettings.DefaultTopic, 1, 1)
    .AddProducer(
        kafkaSettings.ProducerName,
        producer => producer
        .DefaultTopic(kafkaSettings.DefaultTopic)
        .AddMiddlewares(m => m.AddSerializer<ProtobufNetSerializer>()))));

var provider = builder.Services.BuildServiceProvider();

var bus = provider.CreateKafkaBus();

await bus.StartAsync();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/track", async (
    [FromHeader(Name = "User-Agent")] string userAgent,
    [FromHeader(Name = "Referrer")] string referrer,
    HttpContext context,
    [FromServices] IProducerAccessor accessor) =>
{
    var command = new AddVisitorCommand()
    {
         Date = DateTime.UtcNow.ToString("o"),
         Referrer =  string.IsNullOrEmpty(referrer) ? "null" : referrer,
         UserAgent = string.IsNullOrEmpty(userAgent) ? "null" : userAgent,
         IpAddress = context.Connection?.RemoteIpAddress?.ToString()
    };

    var producer = accessor.GetProducer(kafkaSettings.ProducerName);
    await producer.ProduceAsync(command.RoutingKey, command);

    var imageBytes = File.ReadAllBytes("./Images/image.gif");

    return Results.File(
        imageBytes,
        "image/gif",
        fileDownloadName: "image.gif");
})
.WithName("Track")
.WithOpenApi();

app.Run();
