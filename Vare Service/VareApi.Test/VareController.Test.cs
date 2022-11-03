using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using VareApi.Controllers;
using VareApi.Models;
using VareApi.Services;

namespace VareApi.Test;

[TestFixture]
public class Tests
{
    private ILogger<VareController> _logger;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<VareController>>().Object;
    }

    [Test]
    public async Task TestVareEndpoint_failure_posting()
    {
        // Arrange
        var vare = CreateVare(new DateTime(2023, 11, 22, 14, 22, 32));
        var mockRepo = new Mock<IDataService>();
        
        mockRepo.Setup(svc => svc.Create(vare)).Returns(Task.FromException(new Exception()));

        var controller = new VareController(_logger, mockRepo.Object);

        // Act        
        var result = await controller.Post(vare);

        Console.WriteLine("Id:          " + vare.ProductId);
        Console.WriteLine("Title:       " + vare.Title);
        Console.WriteLine("Description: " + vare.Description);
        Console.WriteLine("Show Room:   " + vare.ShowRoomId);
        Console.WriteLine("Valuation:   " + vare.Valuation);
        Console.WriteLine("Start:       " + vare.AuctionStart);

        // Assert
        Assert.That(result, Is.Null);
    }

    private Vare CreateVare(DateTime requestTime)
    {
        var vare = new Vare()
        {
            ProductId = "1",
            Title = "Test Bord",
            Description = "Test bord lavet af birk",
            ShowRoomId = 1,
            Valuation = 10.00,
            AuctionStart = requestTime.ToLongTimeString(),
            Images = new[] {"image1", "image2"}
        };
        return vare;
    }
}