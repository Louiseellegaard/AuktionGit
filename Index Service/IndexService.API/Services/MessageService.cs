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

        private static readonly string queueName = "bids";

        public MessageService(IConfiguration configuration)
        {
            var mqhostname = configuration["BidBrokerHost"];

            // Hvis mphostname er tom, så falder vi tilbage på localhost. Dette er "dårlig" fejlhåndtering, og er den hurtige løsning.
            if (string.IsNullOrEmpty(mqhostname))
            {
                mqhostname = "localhost";
            }

            var factory = new ConnectionFactory { HostName = mqhostname };
            _connection = factory.CreateConnection();
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

                // Serialisering af Booking-objekt
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