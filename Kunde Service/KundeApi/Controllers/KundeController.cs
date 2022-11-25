using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using KundeApi.Models;
using KundeApi.Services;

namespace KundeApi.Controllers;

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

	/// <summary>
	/// Henter listen over alle kunder.
	/// </summary>
	/// <returns>Listen over kunder.</returns>
	// GET: api/Kunde
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Kunde>>> Get()
	{
		return await _dataService
			.Get();
	}

	// GET: api/Kunde/5
	[HttpGet("{id}", Name = "Get")]
	public async Task<ActionResult<Kunde>> Get(string id)
	{
		var kunde = GetFromCache(id);

		if (kunde is null)
		{
			kunde = await _dataService
				.Get(id);

			if (kunde is null)
			{
				return NotFound();
			}
			SetInCache(kunde);
		}
		return Ok(kunde);
	}

	// POST: api/Kunde
	[HttpPost]
	public async Task<ActionResult<Kunde>> Post([FromBody] KundeDTO kundeDTO)
	{
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

		return CreatedAtRoute("Get", new { id = kunde.CustomerId }, kunde);
	}

	// PUT: api/Kunde/5
	[HttpPut("{id}")]
	public async Task<ActionResult<Kunde>> Put(string id, [FromBody] KundeDTO kundeDTO)
	{
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

		await _dataService
			.Update(id, kunde);

		return CreatedAtRoute("Get", new { id = kunde.CustomerId }, kunde);
	}

	// DELETE: api/Kunde/5
	[HttpDelete("{id}")]
	public async Task<ActionResult<Kunde>> Delete(string id)
	{
		var kunde = await _dataService
			.Get(id);

		if (kunde is null)
		{
			return NotFound();
		}

		await _dataService
			.Delete(id);

		return NoContent();
	}


	// -----------------------------------------------
	// Cache -----------------------------------------

	private void SetInCache(Kunde kunde)
	{
		var cacheExpiryOptions = new MemoryCacheEntryOptions
		{
			AbsoluteExpiration = DateTime.Now.AddHours(1),
			SlidingExpiration = TimeSpan.FromMinutes(10),
			Priority = CacheItemPriority.High
		};
		_memoryCache.Set(kunde.CustomerId, kunde, cacheExpiryOptions);
	}

	private Kunde GetFromCache(string id)
	{
		_memoryCache.TryGetValue(id, out Kunde kunde);
		return kunde;
	}

	private void RemoveFromCache(string id)
	{
		_memoryCache.Remove(id);
	}

	public record KundeDTO(string Name, string PhoneNumber, string Email, string City, int ZipCode, string Country, string Address);
}
