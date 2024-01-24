namespace api.models;

public class AuthResponse<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new List<string>();
}