namespace Application.Exceptions;

public enum ErrorType
{
    ResourceNotFound = 404,
    Unauthenticated = 401,
    Unauthorized = 403,
    Internal = 500
}