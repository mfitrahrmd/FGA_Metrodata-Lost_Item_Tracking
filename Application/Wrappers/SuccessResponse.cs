#region

using Application.Wrappers.Common;

#endregion

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

public class SuccessResponse : BaseResponse
{
    public SuccessResponse(string message, int code = 200) : base(true, code, message)
    {
    }
}