using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IndexService.Models;
using IndexService.Services;

namespace IndexService.Pages
{
	public class AuktionPageModel : PageModel
	{
		private readonly ILogger<AuktionPageModel> _logger;
		private readonly IHttpClientFactory? _clientFactory = null;
		private readonly IMessageService _messageService;

		[BindProperty(SupportsGet = true)]
		public string? AuktionId { get; set; }


		public AuktionFuld? Auktion { get; set; }
		public List<Kunde>? Kunder { get; set; }

		public double BidPrice { get; set; }


		public AuktionPageModel(ILogger<AuktionPageModel> logger, IHttpClientFactory clientFactory, IMessageService messageService)
		{
			_logger = logger;
			_clientFactory = clientFactory;
			_messageService = messageService;
		}


		public void OnGet()
		{
			using HttpClient? client = _clientFactory?.CreateClient("gateway");

			_logger.LogInformation("Henter auktion '{AuktionId}'.", AuktionId);

			try
			{
				// Henter auktion
				Auktion = client?.GetFromJsonAsync<AuktionFuld>(
					$"api/index/auktion/{AuktionId}").Result;

				var bidCount = Auktion!.Bids!.Count;

				if (bidCount is 0)
				{
					_logger.LogInformation("Auktion '{AuktionId}' indeholder ingen bud, sætter pris til mindstepris.", AuktionId);
					BidPrice = Auktion.MinimumPrice;
				}
				else
				{
					_logger.LogInformation("Auktion '{AuktionId}' indeholder {bidCount} bud.", AuktionId, bidCount);
					BidPrice = Auktion.Bids!.FirstOrDefault()!.Bid;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Fejl i OnGet af auktion: " + ex.Message);
			}

			try
			{
				// Henter kunder
				Kunder = client?.GetFromJsonAsync<List<Kunde>>(
					$"api/kunde").Result;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Fejl i OnGet af kunder: " + ex.Message);
			}
		}

		[BindProperty(SupportsGet = true)]
		public BudDTO? Bid { get; set; }

		public IActionResult OnPost()
		{
			using HttpClient? client = _clientFactory?.CreateClient("gateway");

			_logger.LogInformation("Poster bud fra bruger '{BuyerId}' på auction '{AuktionId}'.", Bid!.BuyerId, AuktionId);

			try
			{
				Bid!.AuctionId = AuktionId!;
				Bid.Date = DateTime.UtcNow;

				_messageService.Enqueue(Bid!);

			}
			catch (Exception ex)
			{
				Console.WriteLine("Fejl i OnPost: " + ex.Message);
			}

			return Redirect($"/indexservice/auktion/{AuktionId}");
		}
	}
}
