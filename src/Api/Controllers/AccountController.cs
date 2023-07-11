using Api.DTO;
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

    public AccountApiController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Swagger

    /// <summary>
    ///     Позволяет добавить пользователя 
    /// </summary>
    /// <param name="userRegisterRequest">Данные о пользователи</param>
    /// <response code="200">Добавление прошло успешно</response>
    /// <response code="400">Ошибка добавления</response>
    
    #endregion
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody]UserRegisterRequest userRegisterRequest, CancellationToken token)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        //var result = await _mediator.Send(new SaveUser(userRequest), token);
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
        
        return Ok();
    }
}