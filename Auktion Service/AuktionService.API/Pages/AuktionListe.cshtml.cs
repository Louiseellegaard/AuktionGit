using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System;
using AuktionService.Models;

namespace MyApp.Namespace
{
	public class AuktionListeModel : PageModel
	{
		private readonly ILogger<AuktionListeModel> _logger;
		private readonly IHttpClientFactory? _clientFactory = null;
		public List<Auktion>? AuktionListe { get; set; }

		public AuktionListeModel(ILogger<AuktionListeModel> logger, IHttpClientFactory clientFactory)
		{
            _logger = logger;
			_clientFactory = clientFactory;
		}

		public async Task OnGetAsync()
		{
			using HttpClient? client = _clientFactory?.CreateClient("gateway");

			_logger.LogInformation("Forsøger at hente liste med auktioner");

			try
			{
				// Henter auktionliste
				AuktionListe = await client.GetFromJsonAsync<List<Auktion>>("api/auktion");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Fejl i OnGet af Auktioner", ex.Message);
			}

		}
	}
}