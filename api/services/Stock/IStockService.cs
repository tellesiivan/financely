using api.dtos.stock;
using api.models;

namespace api.services.Stock;

public interface IStockService
{
    Task<ServiceResponse<List<StockDto>>> GetAllStocks();
    Task<ServiceResponse<StockDto>> GetStock(int id);
    
    Task<ServiceResponse<models.Stock>> UpdateStock(int id, UpdateStockRequestDto stockRequestDto );

    
    Task<ServiceResponse<StockDto>> AddStock(CreateStockRequestDto stockRequest);
    
    
}