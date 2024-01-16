using api.dtos.stock;
using api.helpers;
using api.models;

namespace api.services.Stock;

public interface IStockService
{
    Task<ServiceResponse<List<StockDto>>> GetAllStocks(QueryObject queryObject);
    Task<ServiceResponse<StockDto>> GetStock(int id);
    
    Task<ServiceResponse<models.Stock>> UpdateStock(int id, UpdateStockRequestDto stockRequestDto );

    
    Task<ServiceResponse<StockDto>> AddStock(CreateStockRequestDto stockRequest);

    Task<bool> DoesExists(int id);

}