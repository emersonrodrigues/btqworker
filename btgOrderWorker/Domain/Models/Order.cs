using System.Text.Json.Serialization;

namespace btgOrderWorker.Domain.models;

public class Order
{    
    [JsonPropertyName("codigoPedido")]
    public int Id { get; set; }
    [JsonPropertyName("codigoCliente")]
    public int CustomerId { get; set; }
    [JsonPropertyName("itens")]
    public List<Product> products { get; set; }
}