namespace IndexService.Models;

public class AuktionFuld
{
    public string? AuctionId { get; set; }
	public string? AuctionDescription { get; set; }
	public string? ProductId { get; set; }
	public string? ProductTitle { get; set; }
	public string? ProductDescription { get; set; }
	public double ProductValuation { get; set; }
	public double MinimumPrice { get; set; }
	public string? BuyerId { get; set; }
    public int? ShowRoomId { get; set; }
	public DateTime? AuctionStart { get; set; }
	public DateTime? AuctionEnd { get; set; }
    public string[]? Images { get; set; }
	public string BidId { get; set; }
	public double Bid { get; set; }
	public DateTime BidDate { get; set; }
}
