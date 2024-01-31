using KafkaFlow;
using KafkaFlow.Serializer;
using StorageService.Application.Configurations;
using StorageService.Application.Handlers;
using StorageService.Application.Interfaces;
using StorageService.Infrastructure.Data;

var builder = Host.CreateApplicationBuilder(args);

ISettings settings = builder.Configuration
    .GetSection(nameof(Settings))
    .Get<Settings>()
    ?? new Settings();

builder.Services.AddSingleton(settings);

builder.Services.AddSingleton<IVisitorStorage, VisitorStorage>();

builder.Services.AddKafka(
    kafka => kafka
    .AddCluster(
        cluster => cluster
                .WithBrokers(settings.Kafka.Brokers)
                .CreateTopicIfNotExists(settings.Kafka.Topic, 1, 1)
    .AddConsumer(
        consumer => consumer
            .Topic(settings.Kafka.Topic)
            .WithGroupId(settings.Kafka.ConsumerGroup)
            .WithBufferSize(100)
            .WithWorkersCount(1)
            .AddMiddlewares(
                middlewares => middlewares
                    .AddDeserializer<ProtobufNetDeserializer>()
                    .AddTypedHandlers(h => h.AddHandler<AddVisitorCommandHandler>())))));

var provider = builder.Services.BuildServiceProvider();

var bus = provider.CreateKafkaBus();

await bus.StartAsync();

var host = builder.Build();
host.Run();
