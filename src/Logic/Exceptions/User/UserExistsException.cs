namespace Logic.Exceptions.User;

public class UserExistsException : BaseException
{
    public UserExistsException(string message = "Такой пользователь уже существует", string code = "409") 
        : base(message, code)
    {
    }
}