using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IndexService.Models;

namespace IndexService.Pages
{
	public class AuktionPageModel : PageModel
	{
		[BindProperty(SupportsGet = true)]
		public string? AuktionId { get; set; }
		public AuktionFuld? Auktion { get; set; }

		private readonly IHttpClientFactory? _clientFactory = null;

		public AuktionPageModel(IHttpClientFactory clientFactory)
		{
			_clientFactory = clientFactory;
		}

		public void OnGet()
		{
			Console.WriteLine("AuktionId: " + AuktionId);

			using HttpClient? client = _clientFactory?.CreateClient("gateway");

			try
			{
				// Henter auktion
				Auktion = client?.GetFromJsonAsync<AuktionFuld>(
					$"api/index/auktion/{AuktionId}").Result;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}

}
