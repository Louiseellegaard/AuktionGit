using Microsoft.AspNetCore.Mvc;
using VareApi.Models;
using VareApi.Services;

namespace VareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VareController : ControllerBase
    {
        private readonly ILogger<VareController> _logger;
        private readonly IDataService _dataService;

        public VareController(ILogger<VareController> logger, IDataService dataService) 
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
                properties.Add($"{attribute.AttributeType.Name} - { attribute}"); 
            }
            return properties;
        }

        // GET api/vare
        [HttpGet]
        public async Task<IEnumerable<Vare>> GetVarer()
        {
            var vareListe = await _dataService
                .GetAll();

            return vareListe;
        }

        // GET api/vare/5
        [HttpGet("{id}")]
        public async Task<Vare> Get(string id)
        {
            var vare = await _dataService
                .GetById(id);

            return vare;
        }

        // POST api/vare
        [HttpPost]
        public async Task Post(Vare vare)
        {
            await _dataService
                .Create(vare);
        }

        // PUT api/vare/5
        [HttpPut]
        public async Task Put(Vare vare)
        {
            await _dataService
                .Update(vare);
        }

        // DELETE api/vare/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}