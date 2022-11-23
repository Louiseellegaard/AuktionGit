using Microsoft.AspNetCore.Mvc;
using KundeApi.Models;

namespace KundeApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KundeController : ControllerBase
{
    private readonly ILogger<KundeController> _logger;

    public KundeController(ILogger<KundeController> logger)
    {
        _logger = logger;
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

    // GET api/kunde
    [HttpGet]
    public async Task<IEnumerable<Kunde>> GetKunder()
    {
        var kundeListe = new List<Kunde>()
        {
            new Kunde()
            {
                KundeId = "1",
                Name = "Mikkel",
                PhoneNumber = "55555555",
                Email = "mikkel@mikkel.dk",
                City = "Aarhus",
                ZipCode = 8000,
                Country = "Denmark",
                Address = "Mikkelvej 10"
            },
            new Kunde()
            {
                KundeId = "2",
                Name = "Mikkel",
                PhoneNumber = "55555555",
                Email = "mikkel@mikkel.dk",
                City = "Aarhus",
                ZipCode = 8000,
                Country = "Denmark",
                Address = "Mikkelvej 10"
            },
            new Kunde()
            {
                KundeId = "3",
                Name = "Mikkel",
                PhoneNumber = "55555555",
                Email = "mikkel@mikkel.dk",
                City = "Aarhus",
                ZipCode = 8000,
                Country = "Denmark",
                Address = "Mikkelvej 10"
            },
            new Kunde()
            {
                KundeId = "4",
                Name = "Mikkel",
                PhoneNumber = "55555555",
                Email = "mikkel@mikkel.dk",
                City = "Aarhus",
                ZipCode = 8000,
                Country = "Denmark",
                Address = "Mikkelvej 10"
            },
            new Kunde()
            {
                KundeId = "5",
                Name = "Mikkel",
                PhoneNumber = "55555555",
                Email = "mikkel@mikkel.dk",
                City = "Aarhus",
                ZipCode = 8000,
                Country = "Denmark",
                Address = "Mikkelvej 10"
            },
            new Kunde()
            {
                KundeId = "6",
                Name = "Mikkel",
                PhoneNumber = "55555555",
                Email = "mikkel@mikkel.dk",
                City = "Aarhus",
                ZipCode = 8000,
                Country = "Denmark",
                Address = "Mikkelvej 10"
            },
            new Kunde()
            {
                KundeId = "7",
                Name = "Mikkel",
                PhoneNumber = "55555555",
                Email = "mikkel@mikkel.dk",
                City = "Aarhus",
                ZipCode = 8000,
                Country = "Denmark",
                Address = "Mikkelvej 10"
            },
            new Kunde()
            {
                KundeId = "8",
                Name = "Mikkel",
                PhoneNumber = "55555555",
                Email = "mikkel@mikkel.dk",
                City = "Aarhus",
                ZipCode = 8000,
                Country = "Denmark",
                Address = "Mikkelvej 10"
            },
            new Kunde()
            {
                KundeId = "9",
                Name = "Mikkel",
                PhoneNumber = "55555555",
                Email = "mikkel@mikkel.dk",
                City = "Aarhus",
                ZipCode = 8000,
                Country = "Denmark",
                Address = "Mikkelvej 10"
            },
            new Kunde()
            {
                KundeId = "10",
                Name = "Mikkel",
                PhoneNumber = "55555555",
                Email = "mikkel@mikkel.dk",
                City = "Aarhus",
                ZipCode = 8000,
                Country = "Denmark",
                Address = "Mikkelvej 10"
            }
        };

        return kundeListe;
    }
}
