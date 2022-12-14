using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IndexService.Models;

namespace IndexService.Pages
{
	public class AuktionPageModel : PageModel
	{
		private readonly ILogger<AuktionPageModel> _logger;
		private readonly IHttpClientFactory? _clientFactory = null;

		[BindProperty(SupportsGet = true)]
		public string? AuktionId { get; set; }


		public AuktionFuld? Auktion { get; set; }
		public List<Kunde>? Kunder { get; set; }

		public double BidPrice { get; set; }


		public AuktionPageModel(ILogger<AuktionPageModel> logger, IHttpClientFactory clientFactory)
		{
			_logger = logger;
			_clientFactory = clientFactory;
		}


		public void OnGet()
		{
			_logger.LogInformation("Henter auktion '{AuktionId}'.", AuktionId);

			using HttpClient? client = _clientFactory?.CreateClient("gateway");

			try
			{
				// Henter auktion
				Auktion = client?.GetFromJsonAsync<AuktionFuld>(
					$"api/index/auktion/{AuktionId}").Result;

				if (Auktion!.Bids!.Count is 0)
				{
					_logger.LogInformation("Auktion '{AuktionId}' indeholder ingen bud, sætter pris til mindstepris.", AuktionId);
					BidPrice = Auktion.MinimumPrice;
				}
				else
				{
					_logger.LogInformation("Auktion '{AuktionId}' indeholder bud.", AuktionId);
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

		public void OnPost()
		{
			_logger.LogInformation("Forsøger at forbinde til client.");

			using HttpClient? client = _clientFactory?.CreateClient("gateway");

			try
			{
				Bid!.AuctionId = AuktionId!;
				Bid.Date = DateTime.UtcNow;

				client?.PostAsJsonAsync("api/index/bud", Bid);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Fejl i OnPost: " + ex.Message);
			}
		}
	}
}
