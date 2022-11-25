using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VareService.Models;

namespace MyApp.Namespace
{
	public class VareListeModel : PageModel
	{
		public List<Vare>? VareListe { get; set; }

		public void OnGet()
		{
			using HttpClient client = new()
			{
				BaseAddress = new Uri("http://localhost:80/")
			};

			// Henter vareliste. 
			VareListe = client.GetFromJsonAsync<List<Vare>>("api/vare").Result;
		}
	}

}
