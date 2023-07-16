namespace Logic.Exceptions;

public class UserExistsException : Exception
{
    public readonly string Message;
    public readonly string Code;
    
    public UserExistsException(string message = "Такой пользователь уже существует", string code = "409") 
        : base(message, innerException: null)
    {
        Message = message;
        Code = code;
    }
}