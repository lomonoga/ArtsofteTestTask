namespace Logic.Exceptions;

public class UserExistsException : Exception
{
    public readonly string Code;
    public readonly string Message;

    public UserExistsException(string message = "Такой пользователь уже существует", string code = "409") 
        : base(message, innerException: null)
    {
        Code = code;
        Message = message;
    }
}