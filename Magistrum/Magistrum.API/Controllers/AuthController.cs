using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Magistrum.API.ViewModels;
using Microsoft.AspNetCore.Authentication;

namespace Magistrum.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="registerViewModel"></param>
    /// <returns> Return a message if register was succsessful</returns>
    /// <response code="200">Returns a message if register was succsessful</response>
    /// <response code="400">If the register was not succsessful</response>
    /// <response code="500">If there is an error registering the user</response>
    [AllowAnonymous]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(ErrorModel), 400)]
    [ProducesResponseType(typeof(ErrorModel), 500)]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ErrorModel
            {
                Message = "Dados inválidos.",
                HttpStatus = 400,
                ErrorCode = "INVALID_DATA",
                Data = ModelState.Values.SelectMany(v => v.Errors)
            });

        var user = new IdentityUser
        {
            UserName = registerViewModel.Email,
            Email = registerViewModel.Email
        };

        var result = await _userManager.CreateAsync(user, registerViewModel.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new ErrorModel
            {
                Message = "Erro ao registrar o usuário.",
                HttpStatus = 400,
                ErrorCode = "REGISTER_ERROR",
                Data = result.Errors
            });
        }

        return Ok(new { Message = "Usuário registrado com sucesso." });
    }


    /// <summary>
    /// Login
    /// </summary>
    /// <param name="loginViewModel"></param>
    /// <returns>Return a token if login was succsessful</returns>
    /// <response code="200">Returns a token and expiration date if the login was succsessful</response>
    /// <response code="400">If the login was not succsessful</response>
    /// <response code="500">If there is an error logging the user</response>
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenModel), 200)]
    [ProducesResponseType(typeof(ErrorModel), 400)]
    [ProducesResponseType(typeof(ErrorModel), 500)]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorModel
            {
                Message = "Dados inválidos.",
                HttpStatus = 400,
                ErrorCode = "INVALID_DATA",
                Data = ModelState.Values.SelectMany(v => v.Errors)
            });
        }

        var user = await _userManager.FindByNameAsync(loginViewModel.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, loginViewModel.Password))
        {
            return BadRequest(new ErrorModel
            {
                Message = "Usuário ou senha inválidos.",
                HttpStatus = 400,
                ErrorCode = "INVALID_LOGIN"
            });
        }

        var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, false);

        if (!result.Succeeded)
        {
            return BadRequest(new ErrorModel
            {
                Message = "Erro ao realizar o login.",
                HttpStatus = 400,
                ErrorCode = "LOGIN_ERROR"
            });
        }

        var token = TokenService.GenerateToken(user);

        return Ok(token);
    }

    /// <summary>
    /// Logout
    /// </summary>
    /// <returns>Return a message if logout was succsessful</returns>
    /// <response code="200">Returns a message if logout was succsessful</response>
    [Authorize]
    [ProducesResponseType(typeof(string), 200)]
    [HttpPost("logoff")]
    public async Task<IActionResult> Logoff()
    {
        await _signInManager.SignOutAsync();
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return Ok(new { Message = "Logout realizado com sucesso." });
    }
}

