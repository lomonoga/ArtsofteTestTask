using Api.DTO;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/account/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Swagger

    /// <summary>
    ///     Позволяет добавить пользователя 
    /// </summary>
    /// <param name="userRequest">Данные о пользователи</param>
    /// <response code="200">Добавление прошло успешно</response>
    /// <response code="400">Ошибка добавления</response>
    
    #endregion

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody]UserRequest userRequest)
    {
        _mediator.Send();
    }
}