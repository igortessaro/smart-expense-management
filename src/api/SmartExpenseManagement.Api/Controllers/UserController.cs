using SmartExpenseManagement.Api.Commands;
using SmartExpenseManagement.Api.Repository;
using SmartExpenseManagement.Api.Repository.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace SmartExpenseManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IExpenseRepository _expenseRepository;

    public UserController(ILogger<UserController> logger, IUserRepository userRepository, IExpenseRepository expenseRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
        _expenseRepository = expenseRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserCommand user)
    {
        _logger.LogInformation("{Method}: starting to create a new user({@User})", nameof(CreateAsync), user);

        var entity = new User(user.Name, user.Password);
        await _userRepository.AddAsync(entity);

        _logger.LogInformation("{Method}: the user({@User}) was successfully created", nameof(CreateAsync), entity);

        return Ok(entity);
    }

    [HttpGet("{uuid}/expenses")]
    public async Task<IActionResult> GetAsync(Guid uuid)
    {
        _logger.LogInformation("{Method}: starting to find expenses from a user({Uuid})", nameof(GetAsync), uuid);

        var user = await _userRepository.GetSingleAsync(x => x.Uuid == uuid);

        if (user is null)
        {
            _logger.LogWarning("{Method}: no user finded ({Uuid})", nameof(GetAsync), uuid);
            return BadRequest("User not found");
        }

        var expenses = await _expenseRepository.GetAllAsync(x => x.UserUuid == uuid);

        if (expenses is null || !expenses.Any())
        {
            _logger.LogWarning("{Method}: no expenses finded from user({Uuid})", nameof(GetAsync), uuid);
        }

        return Ok(expenses);
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] string name)
    {
        _logger.LogInformation("{Method}: starting to find a user by name({Name})", nameof(GetAsync), name);

        if (string.IsNullOrEmpty(name))
        {
            _logger.LogWarning("{Method}: the parameter name is missing", nameof(GetAsync));
            return BadRequest();
        }

        var user = await _userRepository.GetSingleAsync(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

        if (user is null)
        {
            _logger.LogWarning("{Method}: no user found with name {Name}", nameof(GetAsync), name);
        }

        return Ok(user);
    }
}
