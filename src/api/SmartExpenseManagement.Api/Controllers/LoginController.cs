using SmartExpenseManagement.Api.Commands;
using SmartExpenseManagement.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using SmartExpenseManagement.Api.Services;

namespace SmartExpenseManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public LoginController(ILogger<LoginController> logger, IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _logger = logger;
        _tokenService = tokenService;
    }

    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] LoginCommand login)
    {
        _logger.LogInformation("{Method}: starting to login ({@Login})", nameof(LoginAsync), login);

        var user = await _userRepository.GetSingleAsync(x => x.UserName.Equals(login.UserName, StringComparison.CurrentCultureIgnoreCase) && x.Password.Equals(login.Password));

        if (user is null)
        {
            _logger.LogWarning("{Method}: user not found by {@Login}", nameof(LoginAsync), login);
            return Unauthorized();
        }

        var token = _tokenService.GenerateToken(user);

        return Ok(token);
    }
}
