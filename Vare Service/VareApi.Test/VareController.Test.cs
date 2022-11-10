using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using VareApi.Controllers;
using VareApi.Models;
using VareApi.Services;

namespace VareApi.Test;

[TestFixture]
public class Vare_Tests
{
    private ILogger<VareController> _logger;
    //private IConfiguration? _configuration;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<VareController>>().Object;

        //var myConfiguration = new Dictionary<string, string>
        //{
        //    {"VareBrokerHost", "http://testhost.local"}
        //};

        //_configuration = new ConfigurationBuilder()
        //    .AddInMemoryCollection(myConfiguration)
        //    .Build();
    }

    [Test]
    public async Task TestVareEndpoint_post_sucess()
    {
        // Arrange
        var auctionStartTime = new DateTime(2023, 11, 22, 14, 22, 32);
        var vare = CreateVare(auctionStartTime);
        var mockRepo = new Mock<IDataService>();

        mockRepo.Setup(svc => svc.Create(vare)).Returns(Task.CompletedTask);
        
        var controller = new VareController(_logger, mockRepo.Object);

        // Act
        var result = await controller.Post(vare);

        // Assert
        Assert.That(result, Is.InstanceOf(typeof(Vare)));
        Assert.Multiple(() =>
        {
            Assert.That(result?.ProductId, Is.EqualTo(1.ToString()));
            Assert.That(result?.AuctionStart, Is.EqualTo(auctionStartTime.ToString()));
        });
    }

    // Vi ønsker at teste det tilfælde, at der opstår en fejl i DataService.Create()-metoden,
    // når vi laver et HTTP post til vores VareController.
    // Vi vil gerne teste, at der retuneres et null-objekt fra Post()-metoden.
    [Test]
    public async Task TestVareEndpoint_post_failure()
    {
        // Arrange
        var vare = CreateVare(new DateTime(2023, 11, 22, 14, 22, 32));
        var mockRepo = new Mock<IDataService>();
        
        mockRepo.Setup(svc => svc.Create(vare)).Returns(Task.FromException(new Exception()));

        var controller = new VareController(_logger, mockRepo.Object);

        // Act        
        var result = await controller.Post(vare);

        // Assert
        Assert.That(result, Is.Null);
    }

    private Vare CreateVare(DateTime auctionStartTime)
    {
        var vare = new Vare()
        {
            ProductId = "1",
            Title = "Test Bord",
            Description = "Test bord lavet af birk",
            ShowRoomId = 1,
            Valuation = 10.00,
            AuctionStart = auctionStartTime.ToString(),
            Images = new[] {"image1", "image2"}
        };
        return vare;
    }

    private void PrintVare(Vare vare)
    {
        Console.WriteLine("Id:          " + vare.ProductId);
        Console.WriteLine("Title:       " + vare.Title);
        Console.WriteLine("Description: " + vare.Description);
        Console.WriteLine("Show Room:   " + vare.ShowRoomId);
        Console.WriteLine("Valuation:   " + vare.Valuation);
        Console.WriteLine("Start:       " + vare.AuctionStart);
    }
}