using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System;
using BudService.Models;

namespace MyApp.Namespace
{
	public class BudListeModel : PageModel
	{
		private readonly ILogger<BudListeModel> _logger;
		private readonly IHttpClientFactory? _clientFactory = null;
		public List<Bud>? BudListe { get; set; }

		public BudListeModel(ILogger<BudListeModel> logger, IHttpClientFactory clientFactory)
		{
            _logger = logger;
			_clientFactory = clientFactory;
		}

		public async Task OnGetAsync()
		{
			using HttpClient? client = _clientFactory?.CreateClient("gateway");

			_logger.LogInformation("Forsøger at hente liste med bud");

			try
			{
				// Henter budliste
				BudListe = await client.GetFromJsonAsync<List<Bud>>("api/bud");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Fejl i OnGet af Bud: ", ex.Message);
			}

		}
	}
}