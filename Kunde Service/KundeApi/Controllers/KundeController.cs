using Microsoft.AspNetCore.Mvc;
using KundeApi.Models;
using KundeApi.Services;

namespace KundeApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KundeController : ControllerBase
{
    private readonly ILogger<KundeController> _logger;
    private readonly IDataService _dataService;

    public KundeController(ILogger<KundeController> logger, IDataService dataService)
    {
        _logger = logger;
        _dataService = dataService;
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
        var kunde = await _dataService
            .Get(id);

        if(kunde is null)
        {
            return NotFound();
        }

        return kunde;
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

		return CreatedAtRoute("Get", new { id = kunde.KundeId }, kunde);
	}

	public record KundeDTO(string Name, string PhoneNumber, string Email, string City, int ZipCode, string Country, string Address);
}
