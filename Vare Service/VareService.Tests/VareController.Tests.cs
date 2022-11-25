using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using VareService.Controllers;
using VareService.Models;
using VareService.Services;

namespace VareService.Tests;

[TestFixture]
public class VareController_Tests
{
    private ILogger<VareController> _logger;
    private IMemoryCache _memoryCache;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<VareController>>().Object;
        _memoryCache = new Mock<IMemoryCache>().Object;
    }

    [Test]
    public async Task Controller_Post_Sucess()
    {
        // Arrange
        var auctionStartTime = new DateTime(2023, 11, 22, 14, 22, 32);
        var vare    = CreateVare(auctionStartTime);
		var vareDTO = CreateVareDTO(vare);


		var mockRepo = new Mock<IDataService>();

        mockRepo.Setup(svc => svc.Create(vare))
            .Returns(Task.FromResult<Vare>);

        var controller = new VareController(_logger, mockRepo.Object, _memoryCache);

        // Act
        var result = await controller.Post(vareDTO);
        Console.WriteLine(auctionStartTime.ToString());
		// Assert
        Assert.That(result, Is.InstanceOf(typeof(ActionResult<Vare>)));
        Assert.Multiple(() =>
        {
			Assert.That(result.Value?.Title, Is.EqualTo(vare.Title));
			Assert.That(result.Value?.AuctionStart, Is.EqualTo(auctionStartTime.ToString()));
        });
    }

    // Vi ønsker at teste det tilfælde, at der opstår en fejl i DataService.Create()-metoden,
    // når vi laver et HTTP post til vores VareController.
    // Vi vil gerne teste, at der retuneres et null-objekt fra Post()-metoden.
    [Test]
    public async Task Controller_Post_Failure()
    {
		// Arrange ----------------------------------
		// Laver en vare
		var auctionStartTime = new DateTime(2023, 11, 22, 14, 22, 32);
		var vare    = CreateVare(auctionStartTime);
		var vareDTO = CreateVareDTO(vare);


		// Laver en mock af "DataService"
		var mockRepo = new Mock<IDataService>();

        mockRepo.Setup(svc => svc.Create(vare))
            .Returns(Task.FromException<Vare>(new Exception()));

        var controller = new VareController(_logger, mockRepo.Object, _memoryCache);

        // Act --------------------------------------
        // Her prøver vi at poste en vare gennem controlleren,
        // der så venter på at få et svar tilbage, som den gemmer som "result"
        var result = await controller.Post(vareDTO);

        // Assert -----------------------------------
        // Her tester vi, om "result" = null
        Assert.That(result.Result, Is.Null);
    }

    private Vare CreateVare(DateTime auctionStartTime)
    {
        var vare = new Vare()
        {
            Title = "Test Bord",
            Description = "Test bord lavet af birk",
            ShowRoomId = 1,
            Valuation = 10.00,
            AuctionStart = auctionStartTime.ToString(),
            Images = new[] { "image1", "image2" }
        };
        return vare;
    }

	private VareController.VareDTO CreateVareDTO(Vare vare)
	{
        VareController.VareDTO vareDTO = new VareController.VareDTO(
            vare.Title,
            vare.Description,
            vare.ShowRoomId,
            vare.Valuation,
            vare.AuctionStart,
            vare.Images
        );

		return vareDTO;
	}
}