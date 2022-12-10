using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using IndexService.Models;

namespace IndexService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IndexController : ControllerBase
{
	private readonly ILogger<IndexController> _logger;
	private readonly IMemoryCache _memoryCache;
	private readonly IHttpClientFactory? _clientFactory = null;

	public IndexController(ILogger<IndexController> logger, IMemoryCache memoryCache, IHttpClientFactory clientFactory)
	{
		_clientFactory = clientFactory;
		_logger = logger;
		_memoryCache = memoryCache;
	}


	// GET: api/AuktionFuld
	[HttpGet]
	public async Task<ActionResult<IEnumerable<AuktionFuld>>> Get()
	{
		using HttpClient? client = _clientFactory?.CreateClient("gateway")!;

		// Laver og instansierer en tom liste, som vi skal bruge til at gemme 'AuktionFuld'-objekter i
		List<AuktionFuld> auktionFuldListe = new();

		// Laver en liste over alle auktioner
		var auktionListe = await client.GetFromJsonAsync<List<Auktion>>("api/auktion");

		// Foreach-loop, der løber gennem listen med auktioner
		foreach (var auktion in auktionListe!)
		{
			// Henter den tilsvarende vare, der knytter sig til den gældende auktion
			Vare? vare = await client.GetFromJsonAsync<Vare>($"api/vare/{auktion.ProductId}");
			
			// Opretter et nyt objekt, der 'merger' auktion og varen i ét
			AuktionFuld auktionFuld = new()
			{ 
				AuctionId = auktion.AuctionId,
				AuctionDescription = auktion.Description,
				ProductId = auktion.ProductId,
				ProductTitle = vare.Title,
				ProductDescription = vare.Description,
				ProductValuation = vare.Valuation,
				MinimumPrice = auktion.MinimumPrice,
				BuyerId = auktion.BuyerId,
				ShowRoomId = vare.ShowRoomId,
				AuctionStart = vare.AuctionStart,
				AuctionEnd = auktion.EndTime,
				Images = vare.Images
			};

			// Tilføjer det nye objekt til auktionFuldListen
			auktionFuldListe.Add(auktionFuld);
			
		}
		return auktionFuldListe;
	}
}