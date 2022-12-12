using System.Text;
using System.Text.Json;
using IndexService.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace IndexService.Services
{
    public interface IMessageService
    {
        void Enqueue(Bud bud);
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

            // Hvis 'mphostname' er tom, så falder vi tilbage på localhost. Dette er "dårlig" fejlhåndtering, og er den hurtige løsning.
            if (string.IsNullOrEmpty(mqhostname))
            {
                Console.WriteLine("Kan ikke hente 'hostname' fra miljø.");
                _logger.LogInformation("Kan ikke hente 'hostname' fra miljø.");

                mqhostname = "localhost";
            }

            Console.WriteLine($"Benytter hostname '{mqhostname}'.");
            _logger.LogInformation("Benytter hostname '{mqhostname}'.", mqhostname);

            var factory = new ConnectionFactory { HostName = mqhostname };
            _connection = factory.CreateConnection();

            Console.WriteLine($"Har oprettet forbindelse til RabbitMQ gennem hostname '{mqhostname}'.");
            _logger.LogInformation("Har oprettet forbindelse til RabbitMQ gennem hostname '{mqhostname}'.", mqhostname);
        }

        public void Enqueue(Bud bud)
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

                // Udskriver til konsolen, at vi har sendt en booking
                Console.WriteLine(" [x] Published bids: {0}", bud.BidId);
            }
        }
    }
}