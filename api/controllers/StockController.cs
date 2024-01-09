using api.dtos.stock;
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
    public async Task<ActionResult<ServiceResponse<List<StockDto>>>> GetAllStocksList()
    {
        var response = await _stockService.GetAllStocks();
        return response.IsSuccess ? Ok(response) : NotFound(response);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<StockDto>>> GetStockById([FromRoute] int id)
    {
        var stockResponse = await _stockService.GetStock(id);
        return stockResponse.IsSuccess ? Ok(stockResponse) : NotFound(stockResponse);
    }
    
    
    [HttpPost("add")]
    public async Task<ActionResult<ServiceResponse<StockDto>>> AddStock([FromBody] CreateStockRequestDto stockRequest)
    {
        var stockResponse = await _stockService.AddStock(stockRequest);
        return stockResponse.IsSuccess ? Ok(stockResponse) : NotFound(stockResponse);
    }
}