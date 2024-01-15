using api.models;
using api.data;
using api.dtos.stock;
using api.mappers;
using Microsoft.EntityFrameworkCore;

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
                .Include(s => s.Comments)
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
                await _applicationDbContext.Stocks
                    .Include(s => s.Comments)
                    .FirstOrDefaultAsync(s => s.Id == id);

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

    public async Task<ServiceResponse<models.Stock>> UpdateStock(int id, UpdateStockRequestDto stockRequestDto)
    {
        var response = new ServiceResponse<models.Stock>();

        try
        {
            var matchedStock = await _applicationDbContext.Stocks.FindAsync(id);

            if (matchedStock is null)
            {
                throw new Exception("There was no stock matched with the provided id");
            }

            matchedStock.CompanyName = stockRequestDto.CompanyName;
            matchedStock.MarketCap = stockRequestDto.MarketCap;
            matchedStock.Industry = stockRequestDto.Industry;
            matchedStock.LastDiv = stockRequestDto.LastDiv;
            matchedStock.Purchase = stockRequestDto.Purchase;
            matchedStock.Symbol = stockRequestDto.Symbol;

            await _applicationDbContext.SaveChangesAsync();
            
            response.Data = matchedStock;
            response.Message = "success";
            response.IsSuccess = true;
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
             await _applicationDbContext.Stocks.AddAsync(stockModel);
             await _applicationDbContext.SaveChangesAsync();
             var matchResponse = await this.GetStock(stockModel.Id);

             if (matchResponse?.Data is null)
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

    public async Task<bool> DoesExists(int id)
    {
        return await _applicationDbContext.Stocks.AnyAsync(stock => stock.Id == id);
    }
}