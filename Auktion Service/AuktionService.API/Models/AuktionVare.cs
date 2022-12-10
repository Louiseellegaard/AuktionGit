namespace AuktionService.Models;

public class AuktionVare
{
	public string? AuctionId { get; set; }
    public string? AuctionDescription { get; set; }
    public string? ProductId { get; set; }
    public string? ProductCategory { get; set; }
    public string? BuyerId { get; set; }
    public DateTime EndTime { get; set; }
    public double MinimumPrice { get; set; }
}
