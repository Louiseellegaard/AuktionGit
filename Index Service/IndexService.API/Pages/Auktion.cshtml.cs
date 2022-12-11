using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IndexService.Models;
using Microsoft.AspNetCore.Components;

namespace IndexService.Pages
{
	public class AuktionPageModel : PageModel
	{
		private readonly IHttpClientFactory? _clientFactory = null;
		public string AuktionId { get; set; }
		public AuktionFuld? Auktion { get; set; }

		public AuktionPageModel(IHttpClientFactory clientFactory, string auktionId)
		{
			_clientFactory = clientFactory;
			AuktionId = auktionId;
		}

		public void OnGet()
		{
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
