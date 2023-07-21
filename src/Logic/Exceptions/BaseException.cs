namespace Logic.Exceptions;

public class BaseException : Exception
{
    public readonly string Code;
    public readonly string Message;

    public BaseException(string message = "Ошибка", string code = "400") 
        : base(message, innerException: null)
    {
        Code = code;
        Message = message;
    }
}