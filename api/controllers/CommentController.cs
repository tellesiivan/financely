using api.dtos.comment;
using api.helpers;
using api.models;
using api.services.comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class CommentController(ICommentService commentService) : ControllerBase
{
    [HttpGet("all")]
    
    public async Task<ActionResult<ServiceResponse<List<CommentDto>>>> GetAll([FromQuery] CommentQuery commentQuery)
    {
        // validation: Data annotation(normally should be used when getting info via form,body etc
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var response = await commentService.GetAll(commentQuery);
        return response.IsSuccess ? Ok(response) : NotFound(response);
    }
    
    [HttpGet("id/{id:int}")]
    public async Task<ActionResult<ServiceResponse<List<CommentDto>>>> GetById(int id)
    {
        var response = await commentService.GetById(id);
        return response.IsSuccess ? Ok(response) : NotFound(response);
    }
    
    [HttpDelete("delete/{id:int}")]
    public async Task<ActionResult<ServiceResponse<string>>> DeleteById([FromRoute] int id)
    {
        var response = await commentService.Delete(id);
        return response.IsSuccess ? Ok(response) : NotFound(response);
    }
    
    [HttpPost("create/{symbol:alpha}")]
    public async Task<ActionResult<ServiceResponse<CommentDto>>> Create(string symbol, [FromBody] CreateCommentDto comment)
    {
        // validation: Data annotation(normally should be used when getting info via form,body etc
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
   
        var response = await commentService.CreateComment(symbol, comment, User);
        return response.IsSuccess ? Ok(response) : NotFound(response);
    }
    
    [HttpPut("update/{commentId:int}")]
    public async Task<ActionResult<ServiceResponse<CommentDto>>> Update([FromRoute]int commentId, [FromBody] UpdateCommentDto comment)
    {
        // validation: Data annotation(normally should be used when getting info via form,body etc
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await commentService.UpdateComment(commentId, comment);
        return response.IsSuccess ? Ok(response) : NotFound(response);
    }
}