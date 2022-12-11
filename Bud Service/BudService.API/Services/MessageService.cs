using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using BudService.Models;

namespace BudService.Services;

/// <summary>
/// Consumes messages from the common message queue.
/// </summary>
public class MessageService : BackgroundService
{
    private readonly ILogger<MessageService> _logger;
    private readonly IConnection _connection;

    private readonly IDataService _dataservice;

    private static readonly string queueName = "bud";
    private static int _nextId = 1;

    /// <summary>
    /// Create a worker service that receives a ILogger and 
    /// environment configuration instance.
    /// <link>https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0</link>
    /// </summary>
    /// <param name="logger"></param>
    public MessageService(ILogger<MessageService> logger, IConfiguration configuration, IDataService dataservice)
    {
        _logger = logger;
        _dataservice = dataservice;

        var mqhostname = configuration["BidBrokerHost"];

        // Hvis mphostname er tom, så falder vi tilbage på localhost. Dette er "dårlig" fejlhåndtering, og er den hurtige løsning.
        if (string.IsNullOrEmpty(mqhostname))
        {
            mqhostname = "localhost";
        }

        var factory = new ConnectionFactory() { HostName = mqhostname };
        _connection = factory.CreateConnection();

        _logger.LogInformation($"Bid worker listening on host at {mqhostname}");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Opretter en channel til at sende beskeder gennem
        var channel = _connection.CreateModel();
        {
            // Opretter en kø
            channel.QueueDeclare(queue: queueName,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
        }

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            // Deserialisering af datastrømmen til et Booking-objekt
            var bud = JsonSerializer.Deserialize<Bud>(message);

            if (bud is not null)
            {
                _logger.LogInformation("Processing booking {id} from {customer} ", bud.BidId, bud.AuctionId);

                _dataservice.Create(bud);

            }
            else
            {
                _logger.LogWarning($"Could not deserialize message with body: {message}");
            }

        };

        channel.BasicConsume(queue: queueName,
                             autoAck: true,
                             consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}