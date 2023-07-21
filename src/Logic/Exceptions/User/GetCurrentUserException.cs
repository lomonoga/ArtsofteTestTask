namespace Logic.Exceptions.User;

public class GetCurrentUserException : BaseException
{
    public GetCurrentUserException(string message = "Ошибка получение пользователя", string code = "401")
        : base(message, code)
    {
    }
}