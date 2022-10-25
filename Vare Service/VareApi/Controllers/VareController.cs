using Microsoft.AspNetCore.Mvc;
using VareApi.Models;
using VareApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public IEnumerable<Vare> GetVarer()
        {
            var varer = _dataService.GetAll();
            return varer;
        }

        // GET api/vare/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            throw new NotImplementedException();
        }

        // POST api/vare
        [HttpPost]
        public void Post(Vare vare)
        {
            throw new NotImplementedException();
        }

        // PUT api/vare/5
        [HttpPut("{id}")]
        public void Put(string id)
        {
            throw new NotImplementedException();
        }

        // DELETE api/vare/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}