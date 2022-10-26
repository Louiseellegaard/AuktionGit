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
        public Vare Get(string id)
        {
            var productId = _dataService.GetById(id);
            return productId;
        }

        // POST api/vare
        [HttpPost]
        public void Post(Vare vare)
        {
           _dataService.Create(vare);
        }

        // PUT api/vare/5
        [HttpPut]
        public void Put(Vare vare)
        {
            _dataService.Update(vare);
        }

        // DELETE api/vare/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}