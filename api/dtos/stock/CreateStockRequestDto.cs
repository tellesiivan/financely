using System.ComponentModel.DataAnnotations;

namespace api.dtos.stock;

public class CreateStockRequestDto
{
    [Required]
    [MaxLength(10, ErrorMessage = "Symbol cannot be over 10 characters")]
    public string Symbol { get; set; } = string.Empty;
    [Required]
    [MaxLength(30, ErrorMessage = "Company name cannot be over 30 characters")]
    public string CompanyName { get; set; } = string.Empty;
    [Required]
    [Range(1, 1000000)]
    public decimal Purchase { get; set; }
    [Required]
    [Range(0.001, 100)]
    public decimal LastDiv { get; set; }
    [Required]
    [MaxLength(10, ErrorMessage = "Industry cannot be over 10 characters")]
    public string Industry { get; set; } = string.Empty;
    [Required]
    [Range(1, 5000000)]
    public long MarketCap { get; set; }
}