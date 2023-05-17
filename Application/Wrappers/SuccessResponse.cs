using Application.Wrappers.Common;

namespace Application.Wrappers;

public class SuccessResponse<TData> : BaseResponse
{
    public TData Data { get; set; }

    public SuccessResponse(string message, int code = 200) : base(true, code, message)
    {
    }

    public SuccessResponse(string message, TData data, int code = 200) : base(true, code, message)
    {
        Data = data;
    }
}