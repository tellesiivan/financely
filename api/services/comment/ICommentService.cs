using System.Security.Claims;
using api.dtos.comment;
using api.models;

namespace api.services.comment;

public interface ICommentService
{
    Task<ServiceResponse<List<CommentDto>>> GetAll();
    Task<ServiceResponse<CommentDto>> GetById(int id);
    Task<ServiceResponse<string>> Delete(int id);
    Task<ServiceResponse<CommentDto>> CreateComment(string symbol, CreateCommentDto comment, ClaimsPrincipal user);
    Task<ServiceResponse<CommentDto>> UpdateComment(int commentId, UpdateCommentDto comment);
    
}