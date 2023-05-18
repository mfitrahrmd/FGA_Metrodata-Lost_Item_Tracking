namespace Application.Exceptions;

public class ServiceException : ApplicationException
{
    public ErrorType ErrorType { get; }

    public ServiceException(ErrorType errorType, string message) : base(message)
    {
        ErrorType = errorType;
    }
}

public class ServiceException<T> : ApplicationException
{
    public ErrorType ErrorType { get; }
    public T MetaData { get; set; }

    public ServiceException(ErrorType errorType, string message, T metaData) : base(message)
    {
        ErrorType = errorType;
        MetaData = metaData;
    }
}