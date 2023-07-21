namespace Logic.Exceptions.User;

public class UserDoesNotExistException : BaseException
{
    public UserDoesNotExistException(string message = "Телефон или пароль неверен", string code = "401") 
        : base(message, code)
    {
    }
}