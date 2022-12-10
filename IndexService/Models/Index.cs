
namespace IndexService.Models;

public class Index
{
    public string? AuctionId { get; set; }
    public string? ProductId { get; set; }
    public string? BuyerId { get; set; }
    public string? ProductDescription { get; set; }
    public DateTime EndTime { get; set; }
    public double MinimumPrice { get; set; }
    public string? Title { get; set; }
    public string? AuctionDescription { get; set; }
    public int ShowRoomId { get; set; }
    public double Valuation { get; set; }
    public string? AuctionStart { get; set; }
    public string[]? Images { get; set; }
}
