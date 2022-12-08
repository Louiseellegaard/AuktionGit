using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VareService.Models;

namespace MyApp.Namespace
{
	public class VareListeModel : PageModel
	{
		private readonly IHttpClientFactory? _clientFactory = null;
		public List<Vare>? VareListe { get; set; }

		public VareListeModel(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

		public void OnGet()
		{
			using HttpClient? client = _clientFactory?.CreateClient("gateway");

			try
			{
				// Henter vareliste
				VareListe = client?.GetFromJsonAsync<List<Vare>>(
					"api/vare").Result;

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}

}
