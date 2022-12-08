using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using BudService.Models;
using BudService.Services;

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
	public IEnumerable<string> GetVersion()
	{
		var properties = new List<string>();
		var assembly = typeof(Program).Assembly;
		foreach (var attribute in assembly.GetCustomAttributesData())
		{
			properties.Add($"{attribute.AttributeType.Name} - {attribute}");
		}
		return properties;
	}

	// GET: api/bud
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

	// POST: api/bud
	[HttpPost]
	public async Task<ActionResult<Bud>> Post([FromBody] budDTO budDTO)
	{
		Bud bud = new()
		{
			AuctionId = budDTO.AuctionId,
			BuyerId = budDTO.BuyerId,
			Date = budDTO.Date,
			Bid = budDTO.Bid,
		};

		await _dataService
			.Create(bud);

		return bud;
	}

	// PUT: api/bud/5
	[HttpPut("{id}")]
	public async Task<ActionResult<Bud>> Put(string id, [FromBody] budDTO budDTO)
	{
		var bud = await _dataService
			.Get(id);

		if (bud is null)
		{
			return NotFound();
		}

	        bud.AuctionId = budDTO.AuctionId;
			bud.BuyerId = budDTO.BuyerId;
			bud.Date = budDTO.Date;
			bud.Bid = budDTO.Bid;

		await _dataService
			.Update(id, bud);

		return bud;
	}

	// DELETE: api/bud/5
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

	public record budDTO(string? AuctionId, string? BuyerId, DateTime Date, double Bid);
}
