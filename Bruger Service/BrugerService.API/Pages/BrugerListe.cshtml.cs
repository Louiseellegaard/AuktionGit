using Microsoft.AspNetCore.Mvc.RazorPages;
using BrugerService.Models;

namespace MyApp.Namespace
{
	public class BrugerListeModel : PageModel
    {
		private readonly IHttpClientFactory? _clientFactory = null;
		public List<Bruger>? BrugerListe { get; set; }

		public BrugerListeModel(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

		public void OnGet()
		{
			using HttpClient? client = _clientFactory?.CreateClient("gateway");

			try
			{
				// Henter brugerliste
				BrugerListe = client?.GetFromJsonAsync<List<Bruger>>(
					"api/bruger").Result;

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

		}
	}
}