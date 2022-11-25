using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KundeApi.Models;

namespace MyApp.Namespace
{
    public class CustomerListModel : PageModel
    {
        public List<Kunde>? kundeListe{get; set;}
         public async void OnGet() 
    { 
        using HttpClient client = new() 
        { 
            BaseAddress = new Uri("http://localhost:80/") 
        }; 
 
        // Get the user information. 
        kundeListe = client.GetFromJsonAsync<List<Kunde>>("api/kunde").Result; 
    } 
    }
     
}
