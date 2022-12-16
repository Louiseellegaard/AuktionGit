using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using IndexService.Models;
using IndexService.Services;

namespace IndexService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IndexController : ControllerBase
{
	private readonly ILogger<IndexController> _logger;
	private readonly IMemoryCache _memoryCache;
	private readonly IHttpClientFactory? _clientFactory = null;
	private readonly IMessageService _messageService;

	public IndexController(ILogger<IndexController> logger, IMemoryCache memoryCache, IHttpClientFactory clientFactory, IMessageService messageService)
	{
		_logger = logger;
		_memoryCache = memoryCache;
		_clientFactory = clientFactory;
		_messageService = messageService;
	}

	// GET: api/index/Auktion
	[HttpGet("Auktion")]
	public async Task<ActionResult<IEnumerable<AuktionVare>>> Get()
	{
		using HttpClient? client = _clientFactory?.CreateClient("gateway")!;

		// Laver og instansierer en tom liste, som vi skal bruge til at gemme 'AuktionFuld'-objekter i
		List<AuktionVare> auktionVareListe = new();

		// Laver en liste over alle auktioner
		var auktionListe = await client.GetFromJsonAsync<List<Auktion>>("api/auktion");

		// Foreach-loop, der løber gennem listen med auktioner
		foreach (var auktion in auktionListe!)
		{
			// Henter den tilsvarende vare, der knytter sig til den gældende auktion
			var vare = await client.GetFromJsonAsync<Vare>($"api/vare/{auktion.ProductId}");

			// Opretter et nyt objekt, der 'merger' auktion og varen i ét
			var auktionVare = new AuktionVare()
			{
				AuctionId = auktion.AuctionId,
				AuctionDescription = auktion.Description,
				ProductId = auktion.ProductId,
				ProductCategory = vare!.Category,
				ProductTitle = vare!.Title,
				ProductDescription = vare.Description,
				ProductValuation = vare.Valuation,
				MinimumPrice = auktion.MinimumPrice,
				AuctionStart = vare.AuctionStart,
				AuctionEnd = auktion.AuctionEnd,
				Images = vare.Images
			};

			// Tilføjer det nye objekt til auktionFuldListen
			auktionVareListe.Add(auktionVare);

		}
		return auktionVareListe;
	}

	// GET: api/index/Auktion/5
	[HttpGet("Auktion/{id}")]
	public async Task<ActionResult<AuktionFuld>> Get(string id)
	{
		using HttpClient? client = _clientFactory?.CreateClient("gateway")!;

		// Henter auktion
		var auktion = await client.GetFromJsonAsync<Auktion>($"api/auktion/{id}");

		// Henter den tilsvarende vare, der knytter sig til auktionen
		var vare = await client.GetFromJsonAsync<Vare>($"api/vare/{auktion!.ProductId}");

		var budListe = await client.GetFromJsonAsync<List<Bud>>($"api/bud/auction={id}");

		// Opretter et nyt objekt, der 'merger' auktion og varen i ét
		var auktionFuld = new AuktionFuld()
		{
			AuctionId = auktion.AuctionId,
			AuctionDescription = auktion.Description,
			ProductId = auktion.ProductId,
			ProductTitle = vare!.Title,
			ProductDescription = vare.Description,
			ProductValuation = vare.Valuation,
			MinimumPrice = auktion.MinimumPrice,
			BuyerId = auktion.BuyerId,
			ShowRoom = (ShowRoom)vare.ShowRoomId,
			AuctionStart = vare.AuctionStart,
			AuctionEnd = auktion.AuctionEnd,
			Images = vare.Images,
			Bids = budListe
		};
		return auktionFuld;
	}

	// POST: api/index/Bud
	[HttpPost("Bud")]
	public void Post(BudDTO bud)
	{
		Console.WriteLine("Forsøger at poste til kø");
		_messageService.Enqueue(bud);
	}
}