using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KundeService.Models;

namespace MyApp.Namespace
{
    public class KundeListeModel : PageModel
    {
		public List<Kunde>? KundeListe { get; set; }

		public void OnGet()
		{
			using HttpClient client = new()
			{
				BaseAddress = new Uri("http://localhost:80/")
			};
			
			// Henter kundeliste. 
			KundeListe = client.GetFromJsonAsync<List<Kunde>>("api/kunde").Result;
		}
	}
}
