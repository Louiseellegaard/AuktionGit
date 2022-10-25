namespace VareApi.Models;

public class Vare
    {
    public string ProductId { get; set; }   
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int ShowRoomid { get; set; }
    public double Valuation { get; set; }
    public string AuctionStart  { get; set; }
    public string[]? Images { get; set; }
    }
