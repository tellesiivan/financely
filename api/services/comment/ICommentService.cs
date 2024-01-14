using api.dtos.comment;
using api.models;

namespace api.services.comment;

public interface ICommentService
{
    Task<ServiceResponse<List<CommentDto>>> GetAll();
    Task<ServiceResponse<CommentDto>> GetById(int id);
    
}