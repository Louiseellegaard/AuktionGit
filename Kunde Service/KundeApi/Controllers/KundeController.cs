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
    public IEnumerable<string> Get()
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
    // GET api/Kunde
    [HttpGet]
    public async Task<IEnumerable<Kunde>> GetKunder()
    {
        var kundeListe = await _dataService
            .GetAll();

        return kundeListe;
    }

	// POST api/Kunde
	[HttpPost]
	public async Task<Kunde> Post(KundeDTO kundeDTO)
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

		var result = _dataService
			.Create(kunde);

		if (result.IsFaulted)
		{
			return null;
		}

		return kunde;
	}

	public record KundeDTO(string Name, string PhoneNumber, string Email, string City, int ZipCode, string Country, string Address);
}
