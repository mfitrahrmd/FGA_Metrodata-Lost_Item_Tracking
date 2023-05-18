namespace Application.Exceptions;

public enum ErrorType
{
    InvalidInput = 400,
    ResourceNotFound = 404,
    Unauthenticated = 401,
    Unauthorized = 403,
    Internal = 500
}