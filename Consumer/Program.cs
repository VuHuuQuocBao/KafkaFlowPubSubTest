
using Confluent.Kafka;
using EventBus.Events;
using KafkaFlow;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

const string topic = "EmailEvent";
const string producerName = "EmailEventProducer";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddKafka(
        kafka => kafka
            .AddCluster(
                cluster => cluster
                    .WithBrokers(new[] { "localhost:9092" })
                    .CreateTopicIfNotExists(topic, 1, 1)
                    .AddConsumer(
                        consumer => consumer
                            .Topic(topic)
                            .WithGroupId("quoc-bao")
                            .WithBufferSize(100)
                            .WithWorkersCount(3)
                            .AddMiddlewares(middleware => middleware
                            .AddDeserializer<KafkaFlow.Serializer.NewtonsoftJsonDeserializer>()
                            .AddTypedHandlers(handlers => handlers
                            //.WithHandlerLifetime(InstanceLifetime.Singleton)
                            .AddHandler<EmailEventHandler>()))
                            )));
/*var provider = builder.Services.BuildServiceProvider();
var bus = provider.CreateKafkaBus();
await bus.StartAsync();*/



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*app.MapGet("/api/consume", () => )*/


app.UseHttpsRedirection();

var provider = builder.Services.BuildServiceProvider();

var bus = provider.CreateKafkaBus();

await bus.StartAsync();

await app.RunAsync();


internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class EmailEventHandler : IMessageHandler<EmailEvents>
{
    public Task Handle(IMessageContext context, EmailEvents message)
    {
        Console.WriteLine($"id: {message.id}, message: {message.message}");
        var x = 1;
        var y = message;

        return Task.CompletedTask;
    }

    /* public Task Invoke(IMessageContext context, MiddlewareDelegate next)
     {
         Console.WriteLine(
             "Topic: {0} | Partition: {1} | Offset: {2} | Message: {3}",
             context.ConsumerContext.Topic,
             context.ConsumerContext.Partition,
             context.ConsumerContext.Offset,
             Encoding.UTF8.GetString(
                 (byte[])context.Message.Value));

         return next(context);
     }*/
}
