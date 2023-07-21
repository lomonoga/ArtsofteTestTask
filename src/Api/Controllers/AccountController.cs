using System.Security.Claims;
using Api.DTO;
using Data.Domain.Models;
using Logic.Common.DTO.Requests;
using Logic.Common.DTO.Responses;
using Logic.Exceptions.User;
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
            return BadRequest(new { code = userExistsException.Code, 
                message = userExistsException.Message });
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
    public async Task<IActionResult> LoginUser([FromBody]UserLoginRequest userLoginRequest,CancellationToken token)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        User userResponse;
        try
        {
            userResponse = await _mediator.Send(new LoginUser(userLoginRequest), token);
        }
        catch (UserDoesNotExistException userDoesNotExistException)
        {
            return BadRequest(new { code = userDoesNotExistException.Code,
                message = userDoesNotExistException.Message });
        }
        
        var tokenDude = _tokenManager.GenerateToken(userResponse);
        
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
        try
        {
            await _mediator.Send(new LogoutUser(), token);
        }
        catch (UserLogoutException userLogoutException)
        {
            return BadRequest(new { code = userLogoutException.Code, 
                message = userLogoutException.Message });
        }
        
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
    public async Task<IActionResult> GetInfoCurrentUser(CancellationToken token)
    {
        UserResponse userResponse;
        try
        {
            userResponse = await _mediator.Send(new GetUser(), token);
        }
        catch (GetCurrentUserException getCurrentUserException)
        {
            return BadRequest(new { code = getCurrentUserException.Code, 
                message = getCurrentUserException.Message });
        }
        
        return Ok(userResponse);
    }
}