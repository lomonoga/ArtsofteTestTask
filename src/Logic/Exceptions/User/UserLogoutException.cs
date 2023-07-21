namespace Logic.Exceptions.User;

public class UserLogoutException : BaseException
{
    public UserLogoutException(string message = "Ошибка деавторизации", string code = "403") 
        : base(message, code)
    {
    }
}