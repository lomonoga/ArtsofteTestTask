using Api.DTO;
using Data.Domain.Models;
using Logic.Exceptions;
using Logic.Handlers.Users;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/account")]
public class AccountApiController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ISecurityService _securityService;
    private readonly ITokenManager _tokenManager;

    public AccountApiController(IMediator mediator, ISecurityService securityService, ITokenManager tokenManager)
    {
        _mediator = mediator;
        _securityService = securityService;
        _tokenManager = tokenManager;
    }

    #region Swagger

    /// <summary>
    ///     Позволяет добавить пользователя 
    /// </summary>
    /// <param name="userRegisterRequest">Данные о пользователе</param>
    /// <response code="200">Добавление прошло успешно</response>
    /// <response code="400">Такой пользователь уже зарегистрирован</response>
    
    #endregion
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody]UserRegisterRequest userRegisterRequest, CancellationToken token)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _mediator.Send(new SaveUser(userRegisterRequest), token);
        }
        catch (UserExistsException userExistsException)
        {
            return BadRequest(new { code = userExistsException.Code, message = userExistsException.Message });
        }

        return Ok();
    }
    
    #region Swagger

    /// <summary>
    ///     Позволяет войти в систему пользователю
    /// </summary>
    /// <response code="200"></response>
    /// <response code="400"></response>
    
    #endregion
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(CancellationToken token)
    {
        var tokenDude = _tokenManager.GenerateToken(new User {Phone = "79862478961", FIO = "YfrvftyftyTtr"});
        return Ok(tokenDude);
    }
    
    #region Swagger

    /// <summary>
    ///     Позволяет пользователю выйти из системы 
    /// </summary>
    /// <response code="200">Выход произведён успешно</response>
    /// <response code="400">Произошла ошибка выхода</response>
    
    #endregion
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> LogoutUser(CancellationToken token)
    {
        return Ok();
    }
    
    #region Swagger

    /// <summary>
    ///     Позволяет получить данные текущего пользователя
    /// </summary>
    /// <response code="200">Данные переданы успешно</response>
    /// <response code="400">Ошибка получения данных</response>
    /// <response code="401">Пользователь не авторизован</response>
    
    #endregion
    
    [Authorize]
    [HttpPost("get-my-info")]
    public async Task<IActionResult> GetCurrentUser(CancellationToken token)
    {
        var user = _securityService.GetCurrentUser();
        return Ok(user?.Identity?.Name);
    }
}