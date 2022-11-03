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
        public void Post(Vare vare)
        {
           _dataService
                .Create(vare);
        }

        // PUT api/vare/5
        [HttpPut]
        public void Put(Vare vare)
        {
            _dataService
                .Update(vare);
        }

        // DELETE api/vare/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}