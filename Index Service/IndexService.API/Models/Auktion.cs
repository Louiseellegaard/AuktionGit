namespace IndexService.Models;

public class Auktion
{
    public string? AuctionId { get; set; }
    public string? ProductId { get; set; }
    public string? BuyerId { get; set; }
    public string? Description { get; set; }
    public DateTime EndTime { get; set; }
    public double MinimumPrice { get; set; }
}