using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IndexService.Models;

namespace MyApp.Namespace
{
	public class AuktionerModel : PageModel
	{
		private readonly IHttpClientFactory? _clientFactory = null;
		public List<AuktionFuld>? Auktioner { get; set; }

		public AuktionerModel(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

		public void OnGet()
		{
			using HttpClient? client = _clientFactory?.CreateClient("gateway");

			try
			{
				// Henter vareliste
				Auktioner = client?.GetFromJsonAsync<List<AuktionFuld>>(
					"api/index/auktioner").Result;

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}

}
