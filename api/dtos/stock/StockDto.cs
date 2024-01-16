using System.ComponentModel.DataAnnotations;
using api.dtos.comment;

namespace api.dtos.stock;

public class StockDto
{
    [Required]
    public int Id { get; set; }
    [Microsoft.Build.Framework.Required]
    [MaxLength(10, ErrorMessage = "Symbol cannot be over 10 characters")]
    public string Symbol { get; set; } = string.Empty;
    [Microsoft.Build.Framework.Required]
    [MaxLength(30, ErrorMessage = "Company name cannot be over 30 characters")]
    public string CompanyName { get; set; } = string.Empty;
    [Microsoft.Build.Framework.Required]
    [Range(1, 1000000)]
    public decimal Purchase { get; set; }
    [Microsoft.Build.Framework.Required]
    [Range(0.001, 100)]
    public decimal LastDiv { get; set; }
    [Microsoft.Build.Framework.Required]
    [MaxLength(10, ErrorMessage = "Industry cannot be over 10 characters")]
    public string Industry { get; set; } = string.Empty;
    [Microsoft.Build.Framework.Required]
    [Range(1, 5000000)]
    public long MarketCap { get; set; }
    public List<CommentDto>? Comments { get; set; }
}