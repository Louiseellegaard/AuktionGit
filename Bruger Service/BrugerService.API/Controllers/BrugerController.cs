using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Caching.Memory;

using BrugerService.Models;
using BrugerService.Services;

namespace BrugerService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrugerController : ControllerBase
{
	private readonly ILogger<BrugerController> _logger;
	private readonly IDataService _dataService;
	private readonly IMemoryCache _memoryCache;

	public BrugerController(ILogger<BrugerController> logger, IDataService dataService, IMemoryCache memoryCache)
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

		var service = assembly.GetName().Name ?? "Undefined";
		properties.Add("service", service!);

		var ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion ?? "Undefined";
		properties.Add("version", ver);

		var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
		var localIPAddr = feature?.LocalIpAddress?.ToString() ?? "N/A";
		properties.Add("local-host-address", localIPAddr);

		return properties;
	}

	// GET: api/Bruger
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Bruger>>> Get()
	{
		_logger.LogDebug("Henter liste over alle brugere.");

		return await _dataService
			.Get();
	}

	// GET: api/Bruger/5
	[HttpGet("{id}", Name = "Get")]
	public async Task<ActionResult<Bruger>> Get(string id)
	{
		_logger.LogDebug("Leder efter bruger med id: {id}.", id);

		var bruger = GetFromCache(id);

		if (bruger is null)
		{
			_logger.LogDebug($"Bruger findes ikke i cache. Henter fra database.");

			bruger = await _dataService
				.Get(id);

			if (bruger is null)
			{
				return NotFound();
			}

			_logger.LogDebug($"Gemmer bruger i cache.");

			SetInCache(bruger);
		}
		return bruger;
	}

	// POST: api/Bruger
	[HttpPost]
	public async Task<ActionResult<Bruger>> Post([FromBody] BrugerDTO brugerDTO)
	{
		_logger.LogDebug("Opretter ny bruger.");

		Bruger bruger = new()
		{
			Name = brugerDTO.Name,
			PhoneNumber = brugerDTO.PhoneNumber,
			Email = brugerDTO.Email,
			City = brugerDTO.City,
			ZipCode = brugerDTO.ZipCode,
			Country = brugerDTO.Country,
			Address = brugerDTO.Address
		};

		await _dataService
			.Create(bruger);

		return bruger;
	}

	// PUT: api/Bruger/5
	[HttpPut("{id}")]
	public async Task<ActionResult<Bruger>> Put(string id, [FromBody] BrugerDTO brugerDTO)
	{
		_logger.LogDebug("Leder efter bruger med id: {id}.", id);

		var bruger = await _dataService
			.Get(id);

		if (bruger is null)
		{
			return NotFound();
		}

		bruger.Name = brugerDTO.Name;
		bruger.PhoneNumber = brugerDTO.PhoneNumber;
		bruger.Email = brugerDTO.Email;
		bruger.City = brugerDTO.City;
		bruger.ZipCode = brugerDTO.ZipCode;
		bruger.Country = brugerDTO.Country;
		bruger.Address = brugerDTO.Address;

		_logger.LogDebug("Opdaterer bruger med nye værdier.");

		await _dataService
			.Update(id, bruger);

		return bruger;
	}

	// DELETE: api/Bruger/5
	[HttpDelete("{id}")]
	public async Task<ActionResult<Bruger>> Delete(string id)
	{
		_logger.LogDebug("Leder efter bruger med id: {id}.", id);

		var bruger = await _dataService
			.Get(id);

		if (bruger is null)
		{
			return NotFound();
		}

		_logger.LogDebug("Fjerner bruger fra database.");

		await _dataService
			.Delete(id);

		if (GetFromCache(id) is not null)
		{
			RemoveFromCache(id);
		}

		return NoContent();
	}


	// -----------------------------------------------
	// Cache

	private void SetInCache(Bruger bruger)
	{
		var cacheExpiryOptions = new MemoryCacheEntryOptions
		{
			AbsoluteExpiration = DateTime.Now.AddHours(1),
			SlidingExpiration = TimeSpan.FromMinutes(10),
			Priority = CacheItemPriority.High
		};
		_memoryCache.Set(bruger.UserId, bruger, cacheExpiryOptions);
		_logger.LogDebug("Gemmer {bruger} i cache.", bruger);
	}

	private Bruger GetFromCache(string id)
	{
		_memoryCache.TryGetValue(id, out Bruger bruger);
		_logger.LogDebug("Henter {bruger} fra cache.", bruger);
		return bruger;
	}

	private void RemoveFromCache(string id)
	{
		_memoryCache.Remove(id);
		_logger.LogDebug("Fjerner bruger fra cache.");
	}

	public record BrugerDTO(string Name, string PhoneNumber, string Email, string City, int ZipCode, string Country, string Address);
}
