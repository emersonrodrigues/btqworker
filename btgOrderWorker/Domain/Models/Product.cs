using System.Text.Json.Serialization;

namespace btgOrderWorker.Domain.models;

public class Product
{    
    public int Id { get; set; }
    [JsonPropertyName("produto")]
    public string Description { get; set; }
    [JsonPropertyName("quantidade")]
    public int Amount { get; set; }
    [JsonPropertyName("preco")]
    public decimal Price { get; set; }
}