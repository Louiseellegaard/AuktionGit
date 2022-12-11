namespace IndexService.Models;

public class Bud
{
	public string BidId { get; set; }
    public string AuctionId { get; set; }
    public string BuyerId { get; set; }
    public DateTime Date { get; set; }
    public double Bid { get; set; }
}
