using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Caching.Memory;

using KundeService.Models;
using KundeService.Services;

namespace KundeService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KundeController : ControllerBase
{
	private readonly ILogger<KundeController> _logger;
	private readonly IDataService _dataService;
	private readonly IMemoryCache _memoryCache;

	public KundeController(ILogger<KundeController> logger, IDataService dataService, IMemoryCache memoryCache)
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

	// GET: api/Kunde
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Kunde>>> Get()
	{
		_logger.LogDebug("Henter liste over alle kunder.");

		return await _dataService
			.Get();
	}

	// GET: api/Kunde/5
	[HttpGet("{id}", Name = "Get")]
	public async Task<ActionResult<Kunde>> Get(string id)
	{
		_logger.LogDebug("Leder efter kunde med id: {id}.", id);

		var kunde = GetFromCache(id);

		if (kunde is null)
		{
			_logger.LogDebug($"Kunde findes ikke i cache. Henter fra database.");

			kunde = await _dataService
				.Get(id);

			if (kunde is null)
			{
				return NotFound();
			}

			_logger.LogDebug($"Gemmer kunde i cache.");

			SetInCache(kunde);
		}
		return kunde;
	}

	// POST: api/Kunde
	[HttpPost]
	public async Task<ActionResult<Kunde>> Post([FromBody] KundeDTO kundeDTO)
	{
		_logger.LogDebug("Opretter ny kunde.");

		Kunde kunde = new()
		{
			Name = kundeDTO.Name,
			PhoneNumber = kundeDTO.PhoneNumber,
			Email = kundeDTO.Email,
			City = kundeDTO.City,
			ZipCode = kundeDTO.ZipCode,
			Country = kundeDTO.Country,
			Address = kundeDTO.Address
		};

		await _dataService
			.Create(kunde);

		return kunde;
	}

	// PUT: api/Kunde/5
	[HttpPut("{id}")]
	public async Task<ActionResult<Kunde>> Put(string id, [FromBody] KundeDTO kundeDTO)
	{
		_logger.LogDebug("Leder efter kunde med id: {id}.", id);

		var kunde = await _dataService
			.Get(id);

		if (kunde is null)
		{
			return NotFound();
		}

		kunde.Name = kundeDTO.Name;
		kunde.PhoneNumber = kundeDTO.PhoneNumber;
		kunde.Email = kundeDTO.Email;
		kunde.City = kundeDTO.City;
		kunde.ZipCode = kundeDTO.ZipCode;
		kunde.Country = kundeDTO.Country;
		kunde.Address = kundeDTO.Address;

		_logger.LogDebug("Opdaterer kunde med nye værdier.");

		await _dataService
			.Update(id, kunde);

		return kunde;
	}

	// DELETE: api/Kunde/5
	[HttpDelete("{id}")]
	public async Task<ActionResult<Kunde>> Delete(string id)
	{
		_logger.LogDebug("Leder efter kunde med id: {id}.", id);

		var kunde = await _dataService
			.Get(id);

		if (kunde is null)
		{
			return NotFound();
		}

		_logger.LogDebug("Fjerner kunde fra database.");

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

	private void SetInCache(Kunde kunde)
	{
		var cacheExpiryOptions = new MemoryCacheEntryOptions
		{
			AbsoluteExpiration = DateTime.Now.AddHours(1),
			SlidingExpiration = TimeSpan.FromMinutes(10),
			Priority = CacheItemPriority.High
		};
		_memoryCache.Set(kunde.CustomerId, kunde, cacheExpiryOptions);
		_logger.LogDebug("Gemmer {kunde} i cache.", kunde);
	}

	private Kunde GetFromCache(string id)
	{
		_memoryCache.TryGetValue(id, out Kunde kunde);
		_logger.LogDebug("Henter {kunde} fra cache.", kunde);
		return kunde;
	}

	private void RemoveFromCache(string id)
	{
		_memoryCache.Remove(id);
		_logger.LogDebug("Fjerner kunde fra cache.");
	}

	public record KundeDTO(string Name, string PhoneNumber, string Email, string City, int ZipCode, string Country, string Address);
}
