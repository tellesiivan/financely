using api.dtos.stock;
using api.helpers;
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
    public async Task<ActionResult<ServiceResponse<List<StockDto>>>> GetAllStocksList([FromQuery] QueryObject queryObject)
    {
        var response = await _stockService.GetAllStocks(queryObject);
        return response.IsSuccess ? Ok(response) : NotFound(response);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceResponse<StockDto>>> GetStockById([FromRoute] int id)
    {
        var stockResponse = await _stockService.GetStock(id);
        return stockResponse.IsSuccess ? Ok(stockResponse) : NotFound(stockResponse);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ServiceResponse<Stock>>> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto stockRequestDto)
    {
        // validation: Data annotation(normally should be used when getting info via form,body etc
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var stockResponse = await _stockService.UpdateStock(id, stockRequestDto);
        return stockResponse.IsSuccess ? Ok(stockResponse) : NotFound(stockResponse);
    }
    
    [HttpPost("add")]
    public async Task<ActionResult<ServiceResponse<StockDto>>> AddStock([FromBody] CreateStockRequestDto stockRequest)
    {
        // validation: Data annotation(normally should be used when getting info via form,body etc
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var stockResponse = await _stockService.AddStock(stockRequest);
        return stockResponse.IsSuccess ? Ok(stockResponse) : NotFound(stockResponse);
    }
}