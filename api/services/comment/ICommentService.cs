using api.dtos.comment;
using api.models;

namespace api.services.comment;

public interface ICommentService
{
    Task<ServiceResponse<List<CommentDto>>> GetAll();
    Task<ServiceResponse<CommentDto>> GetById(int id);
    Task<ServiceResponse<string>> Delete(int id);
    Task<ServiceResponse<CommentDto>> CreateComment(int stockId, CreateCommentDto comment);
    Task<ServiceResponse<CommentDto>> UpdateComment(int commentId, UpdateCommentDto comment);
    
}