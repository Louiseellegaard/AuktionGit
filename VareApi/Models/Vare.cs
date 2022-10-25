namespace VareApi.Models;

public class Vare
    {
    public string productId { get; set; }   
    public string title { get; set; }
    public string description { get; set; }
    public int showRoomid { get; set; }
    public double valuation { get; set; }
    public string auktionStart  { get; set; }
    public string[] images { get; set; }
    }
