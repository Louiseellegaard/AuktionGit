using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using BudService.Models;
using BudService.Services;
using Microsoft.AspNetCore.Http.Features;

namespace BudService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BudController : ControllerBase
{
	private readonly ILogger<BudController> _logger;
	private readonly IDataService _dataService;
	private readonly IMemoryCache _memoryCache;

	public BudController(ILogger<BudController> logger, IDataService dataService, IMemoryCache memoryCache)
	{
		_logger = logger;
		_dataService = dataService;
		_memoryCache = memoryCache;
	}

	[HttpGet("version")]
	public Dictionary<string, string> GetVersion()
	{
		var properties = new Dictionary<string, string>();
		var assembly = typeof(Program).Assembly;

		properties.Add("service", "Catalog");
		var ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location).ProductVersion ?? "Undefined";
		properties.Add("version", ver);

		var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
		var localIPAddr = feature?.LocalIpAddress?.ToString() ?? "N/A";
		properties.Add("local-host-address", localIPAddr);

		return properties;
	}

	// GET: api/Bud
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Bud>>> Get()
	{
		return await _dataService
			.Get();
	}

	// GET: api/Bud/5
	[HttpGet("{id}", Name = "Get")]
	public async Task<ActionResult<Bud>> Get(string id)
	{
		var bud = GetFromCache(id);

		if (bud is null)
		{
			bud = await _dataService
				.Get(id);

			if (bud is null)
			{
				return NotFound();
			}
			SetInCache(bud);
		}
		return bud;
	}

	// POST: api/Bud
	[HttpPost]
	public async Task<ActionResult<Bud>> Post([FromBody] BudDTO BudDTO)
	{
		Bud bud = new()
		{
			AuctionId = BudDTO.AuctionId,
			BuyerId = BudDTO.BuyerId,
			Date = BudDTO.Date,
			Bid = BudDTO.Bid,
		};

		await _dataService
			.Create(bud);

		return bud;
	}

	// PUT: api/Bud/5
	[HttpPut("{id}")]
	public async Task<ActionResult<Bud>> Put(string id, [FromBody] BudDTO BudDTO)
	{
		var bud = await _dataService
			.Get(id);

		if (bud is null)
		{
			return NotFound();
		}

	        bud.AuctionId = BudDTO.AuctionId;
			bud.BuyerId = BudDTO.BuyerId;
			bud.Date = BudDTO.Date;
			bud.Bid = BudDTO.Bid;

		await _dataService
			.Update(id, bud);

		return bud;
	}

	// DELETE: api/Bud/5
	[HttpDelete("{id}")]
	public async Task<ActionResult<Bud>> Delete(string id)
	{
		var bud = await _dataService
			.Get(id);

		if (bud is null)
		{
			return NotFound();
		}

		await _dataService
			.Delete(id);

		return NoContent();
	}


	// -----------------------------------------------
	// Cache -----------------------------------------

	private void SetInCache(Bud bud)
	{
		var cacheExpiryOptions = new MemoryCacheEntryOptions
		{
			AbsoluteExpiration = DateTime.Now.AddHours(1),
			SlidingExpiration = TimeSpan.FromMinutes(10),
			Priority = CacheItemPriority.High
		};
		_memoryCache.Set(bud.BidId, bud, cacheExpiryOptions);
	}

	private Bud GetFromCache(string id)
	{
		_memoryCache.TryGetValue(id, out Bud bud);
		return bud;
	}

	private void RemoveFromCache(string id)
	{
		_memoryCache.Remove(id);
	}

	public record BudDTO(string? AuctionId, string? BuyerId, DateTime Date, double Bid);
}
