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


	//[HttpGet("version")]
	//public Dictionary<string, string> GetVersion()
	//{
	//	var properties = new Dictionary<string, string>();
	//	var assembly = typeof(Program).Assembly;

	//	var service = assembly.GetName().Name;

	//	properties.Add("service", service);
	//	var ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location).ProductVersion ?? "Undefined";
	//	properties.Add("version", ver);

	//	var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
	//	var localIPAddr = feature?.LocalIpAddress?.ToString() ?? "N/A";
	//	properties.Add("local-host-address", localIPAddr);

	//	return properties;
	//}

	//public async Task OnGetAsync()
	//		{
	//			using HttpClient? client = _clientFactory?.CreateClient("gateway")!;

	//			try
	//			{
	//				// Henter auktionliste
	//				AuktionListe = await client.GetFromJsonAsync<List<Auktion>>("api/auktion");
	//			}
	//			catch (Exception ex)
	//			{
	//				Console.WriteLine(ex.Message);
	//			}

	//}

	//// GET: api/Auktion
	//[HttpGet]
	//public async Task<ActionResult<IEnumerable<Auktion>>> Get()
	//{
	//	_logger.LogDebug("Henter liste over alle auktion.");

	//	return await _dataService
	//		.Get();
	//}

	// GET: api/FuldAuktion
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Models.Index>>> Get()
	{
		using HttpClient? client = _clientFactory?.CreateClient("gateway")!;

		List<Models.Index> IndexListe = null;

		var AuktionListe = await client.GetFromJsonAsync<List<Auktion>>("api/auktion");

		foreach (var auktion in AuktionListe) {
			var Vare = await client.GetFromJsonAsync<Vare>($"api/vare/{auktion.ProductId}")!;
			Models.Index auktionVare = new Models.Index() { AuctionId = auktion.AuctionId, ProductId = auktion.ProductId, BuyerId = auktion.BuyerId, ProductDescription = Vare.Description, EndTime = auktion.EndTime, MinimumPrice = auktion.MinimumPrice, Title = Vare.Title, AuctionDescription = auktion.Description, ShowRoomId = Vare.ShowRoomId, Valuation = Vare.Valuation, AuctionStart = Vare.AuctionStart, Images = Vare.Images };
			IndexListe.Add(auktionVare);
			}
		return IndexListe;
	}
}
	

//	// GET: api/Auktion/5
//	[HttpGet("{id}", Name = "Get")]
//	public async Task<ActionResult<Auktion>> Get(string id)
//	{
//		_logger.LogDebug("Leder efter auktion med id: {id}.", id);

//		var auktion = GetFromCache(id);

//		if (auktion is null)
//		{
//			_logger.LogDebug($"Auktion findes ikke i cache. Henter fra database.");

//			auktion = await _dataService
//				.Get(id);

//			if (auktion is null)
//			{
//				return NotFound();
//			}
//			_logger.LogDebug($"Gemmer auktion i cache.");

//			SetInCache(auktion);
//		}
//		return auktion;
//	}

//	// POST: api/Auktion
//	[HttpPost]
//	public async Task<ActionResult<Auktion>> Post([FromBody] AuktionDTO AuktionDTO)
//	{
//		_logger.LogDebug("Opretter ny auktion.");

//		Auktion auktion = new()
//		{
	
//			ProductId = AuktionDTO.ProductId,
//			BuyerId = AuktionDTO.BuyerId,
//			Description = AuktionDTO.Description,
//			EndTime = AuktionDTO.EndTime,
//			MinimumPrice = AuktionDTO.MinimumPrice,
//		};

//		await _dataService
//			.Create(auktion);

//		return auktion;
//	}

//	// PUT: api/Auktion/5
//	[HttpPut("{id}")]
//	public async Task<ActionResult<Auktion>> Put(string id, [FromBody] AuktionDTO AuktionDTO)
//	{
//		_logger.LogDebug("Leder efter auktion med id: {id}.", id);

//		var auktion = await _dataService
//			.Get(id);

//		if (auktion is null)
//		{
//			return NotFound();
//		}

//			auktion.ProductId = AuktionDTO.ProductId;
//			auktion.BuyerId = AuktionDTO.BuyerId;
//			auktion.Description = AuktionDTO.Description;
//			auktion.EndTime = AuktionDTO.EndTime;
//			auktion.MinimumPrice = AuktionDTO.MinimumPrice;

//		_logger.LogDebug("Opdaterer auktion med nye værdier.");

//		await _dataService
//			.Update(id, auktion);

//		return auktion;
//	}

//	// DELETE: api/Auktion/5
//	[HttpDelete("{id}")]
//	public async Task<ActionResult<Auktion>> Delete(string id)
//	{
//		_logger.LogDebug("Leder efter auktion med id: {id}.", id);

//		var auktion = await _dataService
//			.Get(id);

//		if (auktion is null)
//		{
//			return NotFound();
//		}

//		_logger.LogDebug("Fjerner auktion fra database.");

//		await _dataService
//			.Delete(id);

//		if (GetFromCache(id) is not null)
//		{
//			RemoveFromCache(id);
//		}

//		return NoContent();
//	}



//	// -----------------------------------------------
//	// Cache -----------------------------------------

//	private void SetInCache(Auktion auktion)
//	{
//		var cacheExpiryOptions = new MemoryCacheEntryOptions
//		{
//			AbsoluteExpiration = DateTime.Now.AddHours(1),
//			SlidingExpiration = TimeSpan.FromMinutes(10),
//			Priority = CacheItemPriority.High
//		};
//		_memoryCache.Set(auktion.AuctionId, auktion, cacheExpiryOptions);
//		_logger.LogDebug("Gemmer {auktion} i cache.", auktion);
//	}

//	private Auktion GetFromCache(string id)
//	{
//		_memoryCache.TryGetValue(id, out Auktion auktion);
//		_logger.LogDebug("Henter {auktion} fra cache.", auktion);
//		return auktion;
//	}

//	private void RemoveFromCache(string id)
//	{
//		_memoryCache.Remove(id);
//		_logger.LogDebug("Fjerner auktion fra cache.");
//	}

//	public record AuktionDTO(string? ProductId, string? BuyerId, string? Description, DateTime EndTime, double MinimumPrice);
//}
