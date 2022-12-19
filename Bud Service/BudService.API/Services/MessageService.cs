using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using BudService.Models;
using Microsoft.AspNetCore.Mvc;

namespace BudService.Services;

public class MessageService : BackgroundService
{
	private readonly ILogger<MessageService> _logger;
	private readonly IDataService _dataService;
	private IConnection _connection;

	private static readonly string queueName = "bids";

	public MessageService(ILogger<MessageService> logger, IDataService dataService, IConfiguration configuration)
	{
		_logger = logger;
		_dataService = dataService;

        var mqhostname = configuration["queue_hostname"];
		var mqport = configuration["queue_port"];

		// Hvis mphostname er tom, så falder vi tilbage på 'localhost'.
		// Dette er "dårlig" fejlhåndtering, og er den hurtige løsning.
		if (string.IsNullOrEmpty(mqhostname))
		{
            _logger.LogInformation("Kan ikke hente 'hostname' fra miljø.");
            mqhostname = "localhost";
		}

		// Hvis 'mqport' er tom, så falder vi tilbage på '5672'.
        if (string.IsNullOrEmpty(mqport))
        {
            _logger.LogInformation("Kan ikke hente 'port' fra miljø.");
            mqport = 5672;
        }

		var factory = new ConnectionFactory()
		{
			HostName = mqhostname,
			Port = mqport
		};

        _logger.LogInformation("Forsøger at oprette forbindelse til hostname '{factory.HostName}' på port '{factory.Port}'.", factory.HostName, factory.Port);

        _connection = factory.CreateConnection();

        _logger.LogInformation("Har oprettet forbindelse til RabbitMQ gennem hostname '{factory.HostName}' på port '{factory.Port}'.", mqhostname, factory.Port);
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

			// Deserialisering af datastrømmen til et Bud-objekt
			var bud = JsonSerializer.Deserialize<Bud>(message);

			if (bud is not null)
			{
				_logger.LogInformation("Behandler bud '{BidId}' fra auktion '{AuctionId}'.", bud.BidId, bud.AuctionId);

				_dataService.Create(bud);

			}
			else
			{
				_logger.LogWarning("Kunne ikke deserialize besked med body: {message}", message);
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