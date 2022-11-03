using Microsoft.VisualStudio.TestTools.UnitTesting;
using VareApi;
using VareApi.Models;
using VareApi.Services;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace VareApi.Test
{
    [TestClass]
    public class Test
    {
        public DataService _dataService;

        public Test(DataService dataService)
        {
            _dataService = dataService;
        }

        [TestMethod]
        public async Task TestGetById()
        {
            Vare vare = await _dataService.GetById("1");

            Console.WriteLine(vare.Title);
            
            Assert.AreEqual(vare.Title, "Bord");
        }
    }
}
