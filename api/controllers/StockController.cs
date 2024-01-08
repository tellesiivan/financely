using api.services.Stock;
using api.models;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers;

[Route("[controller]")]
[ApiController]
public class StockController: ControllerBase
{
    private readonly IStockService _stockService;

    public StockController(IStockService stockService)
    {
        _stockService = stockService;
    }
    
    [HttpGet("all")]
    public async Task<ActionResult<List<Stock>>> GetAllStocksList()
    {
        var stocks = await _stockService.GetAllStocks();
        return Ok(stocks);
    }
    
    
}