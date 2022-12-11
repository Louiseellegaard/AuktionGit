using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IndexService.Models;

namespace MyApp.Namespace
{
	public class AuktionsBudModel : PageModel
	{
		private readonly IHttpClientFactory? _clientFactory = null;
		public AuktionFuld? Auktion { get; set; }

		public AuktionsBudModel(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

		public void OnGet()
		{
			using HttpClient? client = _clientFactory?.CreateClient("gateway");

			try
			{
				// Henter vare
				Auktion = client?.GetFromJsonAsync<AuktionFuld>(
					"api/index/auktioner").Result;

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}

}
