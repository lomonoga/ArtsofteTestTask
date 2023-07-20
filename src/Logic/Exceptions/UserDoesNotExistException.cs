namespace Logic.Exceptions;

public class UserDoesNotExistException : Exception
{
    public readonly string Code;
    public readonly string Message;

    public UserDoesNotExistException(string message = "Телефон или пароль неверен", string code = "401") 
        : base(message, innerException: null)
    {
        Code = code;
        Message = message;
    }
}