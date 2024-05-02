using EventBus.Events;
using KafkaFlow;
using KafkaFlow.Producers;
using KafkaFlow.Serializer;

//using KafkaFlow.Serializer;

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
                    .AddProducer(
                        producerName,
                        producer => producer
                            .DefaultTopic(topic)
                            .AddMiddlewares(m => m.AddSerializer<KafkaFlow.Serializer.NewtonsoftJsonSerializer>())

                            )));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/api/KafkaProduce", async (IProducerAccessor producerAccessor) =>
{ 
    var @event = new EmailEvents()
    {
        id = 1,
        message = "123"
    };
    Console.Write(@event);
    var x = producerAccessor.GetProducer(producerName);
    await x.ProduceAsync(topic, @event.id.ToString(), @event);
});


app.UseHttpsRedirection();

await app.RunAsync();

public class MySerializer : ISerializer
{
    public Task SerializeAsync(object message, Stream output, ISerializerContext context)
    {
        var x = 1;
        return Task.FromResult(x);
        // Serialization logic here
    }

    public async Task<object> DeserializeAsync(Stream input, Type type, ISerializerContext context)
    {
        var x = 1;
        return Task.FromResult(x);
        // Deserialization logic here
    }
}
