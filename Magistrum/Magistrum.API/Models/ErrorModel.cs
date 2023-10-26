public class ErrorModel
{
    public string Message { get; set; } = null!;
    public string ErrorCode { get; set; } = null!;
    public int HttpStatus { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    public object? Data { get; set; }

}