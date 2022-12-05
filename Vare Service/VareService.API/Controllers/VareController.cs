using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using VareService.Models;
using VareService.Services;

namespace VareService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VareController : ControllerBase
{
	private readonly ILogger<VareController> _logger;
	private readonly IDataService _dataService;
	private readonly IMemoryCache _memoryCache;

	public VareController(ILogger<VareController> logger, IDataService dataService, IMemoryCache memoryCache)
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

	// GET: api/Vare
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Vare>>> Get()
	{
		return await _dataService
			.Get();
	}

	// GET: api/Vare/5
	[HttpGet("{id}", Name = "Get")]
	public async Task<ActionResult<Vare>> Get(string id)
	{
		var vare = GetFromCache(id);

		if (vare is null)
		{
			vare = await _dataService
				.Get(id);
			if (vare is null)
			{
				return NotFound();
			}
			SetInCache(vare);
		}
		return vare;
	}

	// POST: api/Vare
	[HttpPost]
	public async Task<ActionResult<Vare>> Post([FromBody] VareDTO vareDTO)
	{
		Vare vare = new()
		{
			Title = vareDTO.Title,
			Description = vareDTO.Description,
			ShowRoomId = vareDTO.ShowRoomId,
			Valuation = vareDTO.Valuation,
			AuctionStart = vareDTO.AuctionStart,
			Images = vareDTO.Images
		};

		await _dataService
			.Create(vare);

		return vare;
	}

	// PUT: api/Vare/5
	[HttpPut("{id}")]
	public async Task<ActionResult<Vare>> Put(string id, [FromBody] VareDTO vareDTO)
	{
		var vare = await _dataService
			.Get(id);

		if (vare is null)
		{
			return NotFound();
		}

		vare.Title = vareDTO.Title;
		vare.Description = vareDTO.Description;
		vare.ShowRoomId = vareDTO.ShowRoomId;
		vare.Valuation = vareDTO.Valuation;
		vare.AuctionStart = vareDTO.AuctionStart;
		vare.Images = vareDTO.Images;

		await _dataService
			.Update(id, vare);

		return vare;
	}

	// DELETE: api/Vare/5
	[HttpDelete("{id}")]
	public async Task<ActionResult<Vare>> Delete(string id)
	{
		var vare = await _dataService
			.Get(id);

		if (vare is null)
		{
			return NotFound();
		}

		await _dataService
			.Delete(id);

		return NoContent();
	}


	// -----------------------------------------------
	// Cache -----------------------------------------

	private void SetInCache(Vare vare)
	{
		var cacheExpiryOptions = new MemoryCacheEntryOptions
		{
			AbsoluteExpiration = DateTime.Now.AddHours(1),
			SlidingExpiration = TimeSpan.FromMinutes(10),
			Priority = CacheItemPriority.High
		};
		_memoryCache.Set(vare.ProductId, vare, cacheExpiryOptions);
	}

	private Vare GetFromCache(string id)
	{
		_memoryCache.TryGetValue(id, out Vare vare);
		return vare;
	}

	private void RemoveFromCache(string id)
	{
		_memoryCache.Remove(id);
	}

	public record VareDTO(string? Title, string? Description, int ShowRoomId, double Valuation, string AuctionStart, string[]? Images);
}
