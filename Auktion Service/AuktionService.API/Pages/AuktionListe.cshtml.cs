using Microsoft.AspNetCore.Mvc.RazorPages;
using AuktionService.Models;
using System.Text.Json;
using System;

namespace MyApp.Namespace
{
	public class AuktionListeModel : PageModel
	{
		private readonly IHttpClientFactory? _clientFactory = null;
		public List<Auktion>? AuktionListe { get; set; }
		public Vare? Vare { get; set; }

		public AuktionListeModel(IHttpClientFactory clientFactory)
		{
			_clientFactory = clientFactory;
		}

		public async Task OnGetAsync()
		{
			using HttpClient? client = _clientFactory?.CreateClient("gateway")!;

			try
			{
				// Henter auktionliste
				AuktionListe = await client.GetFromJsonAsync<List<Auktion>>("api/auktion");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

		}

		public async Task? HentVare(string id)
		{
			using HttpClient? client = _clientFactory?.CreateClient("gateway")!;

			try
			{
				Vare = await client.GetFromJsonAsync<Vare>($"api/vare/{id}")!;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

	}
}