using SmartExpenseManagement.Api.Commands;
using SmartExpenseManagement.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace SmartExpenseManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly IUserRepository _userRepository;

    public LoginController(ILogger<LoginController> logger, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] LoginCommand login)
    {
        _logger.LogInformation("{Method}: starting to login ({@Login})", nameof(LoginAsync), login);

        var user = await _userRepository.GetSingleAsync(x => x.Name.Equals(login.UserName, StringComparison.CurrentCultureIgnoreCase) && x.Password.Equals(login.Password));

        if (user is null)
        {
            _logger.LogWarning("{Method}: no user finded by {@Login}", nameof(LoginAsync), login);
            return Unauthorized();
        }

        return Ok(user);
    }
}
