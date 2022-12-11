using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IndexService.Models;
using Microsoft.AspNetCore.Components;

namespace IndexService.Pages
{
	public class AuktionPageModel : PageModel
	{
		[Parameter]
		public string AuktionId { get; set; }
		public AuktionFuld? Auktion { get; set; }

		private readonly IHttpClientFactory? _clientFactory = null;

		public AuktionPageModel(IHttpClientFactory clientFactory)
		{
			_clientFactory = clientFactory;
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
