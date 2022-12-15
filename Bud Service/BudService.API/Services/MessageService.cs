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

	//private static readonly int retryCount = 3;
	//private static readonly TimeSpan delay = TimeSpan.FromSeconds(5);

	public MessageService(ILogger<MessageService> logger, IDataService dataService, IConfiguration configuration)
	{
		_logger = logger;
		_dataService = dataService;

		var mqhostname = configuration["BidBrokerHost"];

		// Hvis mphostname er tom, så falder vi tilbage på localhost.
		// Dette er "dårlig" fejlhåndtering, og er den hurtige løsning.
		if (string.IsNullOrEmpty(mqhostname))
		{
			_logger.LogInformation("Kan ikke hente 'hostname' fra miljø.");
			mqhostname = "localhost";
		}

		var factory = new ConnectionFactory()
		{
			HostName = mqhostname,
			Port = 5672
		};

		_logger.LogInformation("Forsøger at oprette forbindelse til hostname '{mqhostname}' på port '{factory.Port}'.", mqhostname, factory.Port);

		//// Retry-pattern start 
		//for (; ; )
		//{
		//	int currentRetry = 0;

		//	try
		//	{
				// Call external service.
				_connection = factory.CreateConnection();

				_logger.LogInformation("Har oprettet forbindelse til RabbitMQ gennem hostname '{mqhostname}' på port '{factory.Port}'.", mqhostname, factory.Port);
			//}
			//catch (Exception ex)
			//{
			//	_logger.LogTrace("Kunne ikke oprette forbindelse {ex}", ex.Message);

			//	currentRetry++;
			//	if (currentRetry >= retryCount || !IsTransient(ex))
			//	{
			//		_logger.LogCritical("{es}", ex.Message);
			//	}
			//}
			//Task.Delay(delay);
		//}
		// Retry-pattern end
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


	//private bool IsTransient(Exception ex)
	//{
	//	_logger.LogDebug("Checking if exception {ex} is transient", ex.GetType().ToString());
	//	return true;
	//}
}