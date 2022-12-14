using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using IndexService.Models;

namespace IndexService.Services
{
    public interface IMessageService
    {
        void Enqueue(BudDTO bud);
    }

    public class MessageService : IMessageService
    {
        private readonly IConnection _connection;
        private readonly ILogger<MessageService> _logger;
        private static readonly string queueName = "bids";

        public MessageService(ILogger<MessageService> logger, IConfiguration configuration)
        {
            _logger = logger;

            var mqhostname = configuration["BidBrokerHost"];

            // Hvis 'mphostname' er tom, så falder vi tilbage på localhost.
            // Dette er "dårlig" fejlhåndtering, og er den hurtige løsning.
            if (string.IsNullOrEmpty(mqhostname))
            {
                _logger.LogInformation("Kan ikke hente 'hostname' fra miljø.");

                mqhostname = "localhost";
            }

            var factory = new ConnectionFactory
            {
                HostName = mqhostname,
                Port = 5672
            };

			_logger.LogInformation("Forsøger at oprette forbindelse til hostname '{mqhostname}' på port '{factory.Port}'.", mqhostname, factory.Port);

			_connection = factory.CreateConnection();

            _logger.LogInformation("Har oprettet forbindelse til RabbitMQ gennem hostname '{mqhostname}' på port '{factory.Port}'.", mqhostname, factory.Port);
        }

        public void Enqueue(BudDTO bud)
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

                // Serialisering af Bud-objekt
                var body = JsonSerializer.SerializeToUtf8Bytes(bud);

                // Sender 'body'-datastrømmen ind i køen
                channel.BasicPublish(exchange: "",
                                        routingKey: queueName,
                                        basicProperties: null,
                                        body: body);

				// Udskriver til logger, at vi har sendt en booking
				_logger.LogInformation("[x] Publiseret bud på auktion: {bud.AuctionId}", bud.AuctionId);
            }
        }
    }
}