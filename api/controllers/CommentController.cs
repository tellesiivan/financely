using api.dtos.comment;
using api.models;
using api.services.comment;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers;

[Route("[controller]")]
[ApiController]
public class CommentController: ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<ServiceResponse<List<CommentDto>>>> GetAll()
    {
        var response = await _commentService.GetAll();
        return response.IsSuccess ? Ok(response) : NotFound(response);
    }
    
    [HttpGet("id/{id}")]
    public async Task<ActionResult<ServiceResponse<List<CommentDto>>>> GetById(int id)
    {
        var response = await _commentService.GetById(id);
        return response.IsSuccess ? Ok(response) : NotFound(response);
    }
    
    [HttpPost("create/{stockId}")]
    public async Task<ActionResult<ServiceResponse<CommentDto>>> Create(int stockId, [FromBody] CreateCommentDto comment)
    {
        var response = await _commentService.CreateComment(stockId, comment);
        return response.IsSuccess ? Ok(response) : NotFound(response);
    }
}