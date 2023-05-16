namespace Application.Wrappers.Common;

public abstract class BaseResponse
{
    public bool IsSucceeded { get; set; }
    public int Code { get; set; }
    public string Message { get; set; }

    public BaseResponse()
    {
    }

    public BaseResponse(bool isSucceeded, int code, string message)
    {
        (IsSucceeded, Code, Message) = (isSucceeded, code, message);
    }
}