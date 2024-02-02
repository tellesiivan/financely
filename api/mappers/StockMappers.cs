using api.dtos.stock;
using api.models;

namespace api.mappers;

public static class StockMappers
{
   // extension method
   public static StockDto ToStockDto(this Stock stockModel)
   {
      return new StockDto
      {
         Id = stockModel.Id,
         Symbol = stockModel.Symbol,
         CompanyName = stockModel.CompanyName,
         MarketCap = stockModel.MarketCap,
         Industry = stockModel.Industry,
         Purchase = stockModel.Purchase,
         LastDiv = stockModel.LastDiv,
         Comments = stockModel.Comments.Select(comment => comment.Dto()).ToList()
      };
   }

   public static Stock ToStockFromCreateDto(this CreateStockRequestDto stockRequestDto)
   {
      return new Stock
      {
         Symbol = stockRequestDto.Symbol,
         CompanyName = stockRequestDto.CompanyName,
         MarketCap = stockRequestDto.MarketCap,
         Industry = stockRequestDto.Industry,
         Purchase = stockRequestDto.Purchase,
         LastDiv = stockRequestDto.LastDiv,
      };
   }  
   
   public static CreateStockRequestDto ToCreateDto(this Stock stock)
   {
      return new CreateStockRequestDto
      {
         Symbol = stock.Symbol,
         CompanyName = stock.CompanyName,
         Purchase = stock.Purchase,
         Industry = stock.Industry,
         LastDiv = stock.LastDiv,
         MarketCap = stock.MarketCap
      };
   }
   
   public static Stock FmpToStock(this FMPStock fmpStock)
   {
      return new Stock
      {
         Symbol = fmpStock.Symbol,
         Purchase = (decimal)fmpStock.Price,
         LastDiv = (decimal)fmpStock.LastDiv,
         CompanyName = fmpStock.CompanyName,
         Industry = fmpStock.Industry,
         MarketCap = fmpStock.MktCap
      };
   }
}