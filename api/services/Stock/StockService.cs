using api.models;
using api.data;
using api.dtos.stock;
using api.mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api.services.Stock;

public class StockService: IStockService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public StockService(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<ServiceResponse<List<StockDto>>> GetAllStocks()
    {
        var response = new ServiceResponse<List<StockDto>>();

        try
        {
            var stockList = await _applicationDbContext.Stocks
                .ToListAsync();
            
            var mappedStockList = stockList.Select(stock => stock.ToStockDto())
                .ToList();
            
            if (mappedStockList is null)
            {
                throw new Exception("No stocks found");
            }

            response.Data = mappedStockList;
            response.Message = "Here is your list of stocks";
            response.IsSuccess = true;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.IsSuccess = false;
        }


        return response;
    }

    public async Task<ServiceResponse<StockDto>> GetStock(int id)
    {
        var response = new ServiceResponse<StockDto>();
        
        try
        {
            // .Find(Finds an entity with the given primary key values)
            var matchedStock =
                await _applicationDbContext.Stocks.FindAsync(id);

            if (matchedStock is null)
            {
                throw new Exception("There is no stock with the id provided");
            }

            response.IsSuccess = true;
            response.Data = matchedStock.ToStockDto();
            response.Message = "success";
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.IsSuccess = false;
        }

        return response;
    }

    public async Task<ServiceResponse<StockDto>> AddStock(CreateStockRequestDto stockRequest)
    {
        var response = new ServiceResponse<StockDto>();
        var stockModel = stockRequest.ToStockFromCreateDto();
        try
        {
             _applicationDbContext.Stocks.Add(stockModel);
             await _applicationDbContext.SaveChangesAsync();
             var matchResponse = await this.GetStock(stockModel.Id);

             if (matchResponse is null || matchResponse.Data is null)
             {
                 throw new Exception("matchResponse error");
             }
             
             response.IsSuccess = true;
             response.Data = matchResponse.Data;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.IsSuccess = false;
        }
        return response;
    }
}