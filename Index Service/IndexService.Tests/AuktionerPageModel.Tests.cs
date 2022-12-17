using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Moq;
using IndexService.Models;
using IndexService.Pages;

namespace IndexService.Tests;

[TestFixture]
public class AuktionerPageModelTests
{
    private Mock<IHttpClientFactory> _mockClientFactory;
    private AuktionerPageModel _pageModel;

    [SetUp]
    public void Setup()
    {
        _mockClientFactory = new Mock<IHttpClientFactory>();
        _pageModel = new AuktionerPageModel(_mockClientFactory.Object);
    }


    //[Test]
    //public async Task OnGet_ReturnsAuktioner()
    //{
    //    // Checker om OnGet-metoden indsætter den forventede data i 'Auktioner', når HTTP request er succesrig.
        
    //    // Arrange
    //    var mockHttpClient = new Mock<HttpClient>();
    //        _mockClientFactory.Setup(f => f.CreateClient(It.IsAny<string>()))
    //            .Returns(mockHttpClient.Object);

    //    var list = new List<AuktionVare>()
    //    {
    //        new AuktionVare { AuctionId = "1", ProductCategory = (ProductCategory)1, ProductValuation = 20.00, MinimumPrice = 10.00 },
    //        new AuktionVare { AuctionId = "2", ProductCategory = (ProductCategory)2, ProductValuation = 10.00, MinimumPrice = 5.00 }
    //    };

    //    mockHttpClient.Setup(c => c.GetFromJsonAsync<List<AuktionVare>>(It.IsAny<string>()))
    //            .ReturnsAsync(list);


    //    // Act
    //    await _pageModel.OnGet();

    //    // Assert
    //    Assert.NotNull(_pageModel.Auktioner);
    //    Assert.That(_pageModel.Auktioner.Count(),             Is.EqualTo(2));
    //    Assert.That(_pageModel.Auktioner[0].AuctionId,        Is.EqualTo("1"));
    //    Assert.That(_pageModel.Auktioner[0].ProductValuation, Is.EqualTo(20.00));
    //    Assert.That(_pageModel.Auktioner[1].AuctionId,        Is.EqualTo("2"));
    //    Assert.That(_pageModel.Auktioner[1].ProductValuation, Is.EqualTo(10.00));
    //}

    //[Test]
    //public async Task OnGet_HandlesHttpRequestException()
    //{
    //    // Checker om OnGet-metoden håndterer og logger HttpRequestException, når der opstår en fejl i GetFromJsonAsync-metoden.

    //    // Arrange
    //    var mockHttpClient = new Mock<HttpClient>();
    //        _mockClientFactory.Setup(f => f.CreateClient(It.IsAny<string>()))
    //            .Returns(mockHttpClient.Object);

    //    mockHttpClient.Setup(c => c.GetFromJsonAsync<List<AuktionVare>>(It.IsAny<string>()))
    //        .ThrowsAsync(new HttpRequestException());

    //    // Act
    //    await _pageModel.OnGet();

    //    // Assert
    //    Assert.IsNull(_pageModel.Auktioner);
    //}
}