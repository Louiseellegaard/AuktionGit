using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VareApi.Models;

namespace MyApp.Namespace
{
    public class CatalogListModel : PageModel
    {
        public List<Vare>? VareListe{get; set;}
         public async void OnGet() 
    { 
        using HttpClient client = new() 
        { 
            BaseAddress = new Uri("http://localhost:80/") 
        }; 
 
        // Get the user information. 
        VareListe = client.GetFromJsonAsync<List<Vare>>("api/vare").Result; 
    } 
    }
     
}
