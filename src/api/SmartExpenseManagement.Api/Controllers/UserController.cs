using SmartExpenseManagement.Api.Commands;
using SmartExpenseManagement.Api.Repository;
using SmartExpenseManagement.Api.Repository.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SmartExpenseManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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

        var entity = new User(user.UserName, user.Password, user.Role);
        await _userRepository.AddAsync(entity);

        _logger.LogInformation("{Method}: the user({@User}) was successfully created", nameof(CreateAsync), entity);

        return Ok(entity);
    }

    [HttpGet("{id}/expenses")]
    public async Task<IActionResult> GetExpensesAsync(string id)
    {
        _logger.LogInformation("{Method}: starting to find expenses from a user({Id})", nameof(GetAsync), id);

        var user = await _userRepository.GetSingleAsync(x => x.Id == id);

        if (user is null)
        {
            _logger.LogWarning("{Method}: user not found ({Id})", nameof(GetAsync), id);
            return BadRequest("User not found");
        }

        var expenses = await _expenseRepository.GetAllAsync(x => x.UserId == id);

        if (expenses is null || !expenses.Any())
        {
            _logger.LogWarning("{Method}: expenses not found for user({Id})", nameof(GetAsync), id);
        }

        return Ok(expenses);
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] string userName)
    {
        _logger.LogInformation("{Method}: starting to find a user by user name({UserName})", nameof(GetAsync), userName);

        if (string.IsNullOrEmpty(userName))
        {
            _logger.LogWarning("{Method}: the parameter user name is missing", nameof(GetAsync));
            return BadRequest();
        }

        var user = await _userRepository.GetSingleAsync(x => x.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase));

        if (user is null)
        {
            _logger.LogWarning("{Method}: no user found with user name {UserName}", nameof(GetAsync), userName);
        }

        return Ok(user);
    }
}
