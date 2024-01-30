using System.ComponentModel.DataAnnotations;

namespace api.dtos.stock;

public class StockSymbolDto
{
    [Microsoft.Build.Framework.Required]
    [MaxLength(10, ErrorMessage = "Symbol cannot be over 10 characters")]
    public string Symbol { get; set; } = string.Empty;
}