using Application.Wrappers.Common;

namespace Application.Wrappers;

public class FailResponse<TError> : BaseResponse
{
    public TError Error { get; set; }

    public FailResponse(string message, int code = 500) : base(false, code, message)
    {
    }

    public FailResponse(string message, TError error, int code = 500) : base(false, code, message)
    {
        Error = error;
    }
}