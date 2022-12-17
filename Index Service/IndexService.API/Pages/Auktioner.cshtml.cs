using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IndexService.Models;

namespace IndexService.Pages
{
	public class AuktionerPageModel : PageModel
	{
		private readonly IHttpClientFactory? _clientFactory = null;
		public List<AuktionVare>? Auktioner { get; set; }

		public AuktionerPageModel(IHttpClientFactory clientFactory)
		{
			_clientFactory = clientFactory;
		}

		public void OnGet()
		{
			using HttpClient? client = _clientFactory?.CreateClient("gateway");

			try
			{
				// Henter auktioner
				Auktioner = client?.GetFromJsonAsync<List<AuktionVare>>(
					"api/index/auktion").Result;

			}
			catch (Exception ex)
			{
				Console.WriteLine("Fejl i OnGet af auktioner: " + ex.Message);
			}
		}
	}

}
