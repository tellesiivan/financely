using api.dtos.stock;
using api.models;

namespace api.mappers;

public static class StockMappers
{
   // extension method
   public static StockDto ToStockDto(this Stock stockModel)
   {
      return new StockDto()
      {
         Id = stockModel.Id,
         Symbol = stockModel.Symbol,
         CompanyName = stockModel.CompanyName,
         MarketCap = stockModel.MarketCap,
         Industry = stockModel.Industry,
         Purchase = stockModel.Purchase,
         LastDiv = stockModel.LastDiv
      };
   }

   public static Stock ToStockFromCreateDto(this CreateStockRequestDto stockRequestDto)
   {
      return new Stock()
      {
         Symbol = stockRequestDto.Symbol,
         CompanyName = stockRequestDto.CompanyName,
         MarketCap = stockRequestDto.MarketCap,
         Industry = stockRequestDto.Industry,
         Purchase = stockRequestDto.Purchase,
         LastDiv = stockRequestDto.LastDiv,
      };
   }
}