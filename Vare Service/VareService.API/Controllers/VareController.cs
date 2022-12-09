using Microsoft.AspNetCore.Http.Features;
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

	// GET: api/Vare
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Vare>>> Get()
	{
		_logger.LogDebug("Henter liste over alle varer.");

		return await _dataService
			.Get();
	}

	// GET: api/Vare/5
	[HttpGet("{id}", Name = "Get")]
	public async Task<ActionResult<Vare>> Get(string id)
	{
		_logger.LogDebug("Leder efter vare med id: {id}.", id);

		var vare = GetFromCache(id);

		if (vare is null)
		{
			_logger.LogDebug($"Vare findes ikke i cache. Henter fra database.");

			vare = await _dataService
				.Get(id);

			if (vare is null)
			{
				return NotFound();
			}

			_logger.LogDebug($"Gemmer vare i cache.");

			SetInCache(vare);
		}
		return vare;
	}

	// POST: api/Vare
	[HttpPost]
	public async Task<ActionResult<Vare>> Post([FromBody] VareDTO vareDTO)
	{
		_logger.LogDebug("Opretter ny vare.");

		Vare vare = new()
		{
			Category = vareDTO.Category,
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
		_logger.LogDebug("Leder efter vare med id: {id}.", id);

		var vare = await _dataService
			.Get(id);

		if (vare is null)
		{
			return NotFound();
		}

		vare.Category = vareDTO.Category;
		vare.Title = vareDTO.Title;
		vare.Description = vareDTO.Description;
		vare.ShowRoomId = vareDTO.ShowRoomId;
		vare.Valuation = vareDTO.Valuation;
		vare.AuctionStart = vareDTO.AuctionStart;
		vare.Images = vareDTO.Images;

		_logger.LogDebug("Opdaterer vare med nye værdier.");

		await _dataService
			.Update(id, vare);

		return vare;
	}

	// DELETE: api/Vare/5
	[HttpDelete("{id}")]
	public async Task<ActionResult<Vare>> Delete(string id)
	{
		_logger.LogDebug("Leder efter vare med id: {id}.", id);

		var vare = await _dataService
			.Get(id);

		if (vare is null)
		{
			return NotFound();
		}

		_logger.LogDebug("Fjerner vare fra database.");

		await _dataService
			.Delete(id);

		if (GetFromCache(id) is not null)
		{
			RemoveFromCache(id);
		}

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
		_logger.LogDebug("Gemmer {vare} i cache.", vare);
	}

	private Vare GetFromCache(string id)
	{
		_memoryCache.TryGetValue(id, out Vare vare);
		_logger.LogDebug("Henter {vare} fra cache.", vare);
		return vare;
	}

	private void RemoveFromCache(string id)
	{
		_memoryCache.Remove(id);
		_logger.LogDebug("Fjerner vare fra cache.");
	}

	public record VareDTO(ProductCategory Category, string? Title, string? Description, int ShowRoomId, double Valuation, string AuctionStart, string[]? Images);
}
