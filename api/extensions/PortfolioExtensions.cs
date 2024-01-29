using api.models;

namespace api.extensions;

public static class PortfolioExtensions
{
    public static Stock ToStock(this Portfolio portfolio) => new Stock()
    {
         Id = portfolio.Stock.Id,
         Industry = portfolio.Stock.Industry,
         Purchase = portfolio.Stock.Purchase,
         Symbol = portfolio.Stock.Symbol,
         CompanyName = portfolio.Stock.CompanyName,
         LastDiv = portfolio.Stock.LastDiv,
         MarketCap = portfolio.Stock.MarketCap, 
    };
}