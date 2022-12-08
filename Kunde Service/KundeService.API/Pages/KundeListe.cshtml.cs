using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KundeService.Models;

namespace MyApp.Namespace
{
	public class KundeListeModel : PageModel
    {
		private readonly IHttpClientFactory? _clientFactory = null;
		public List<Kunde>? KundeListe { get; set; }

		public KundeListeModel(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

		public void OnGet()
		{
			using HttpClient? client = _clientFactory?.CreateClient("gateway");

			try
			{
				// Henter kundeliste
				KundeListe = client?.GetFromJsonAsync<List<Kunde>>(
					"api/kunde").Result;

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

		}
	}
}
