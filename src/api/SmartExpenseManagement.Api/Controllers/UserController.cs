using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartExpenseManagement.Domain.CQRS.Commands;
using SmartExpenseManagement.Domain.Entities;
using SmartExpenseManagement.Domain.Repositories;

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

        var entity = new User(user.FirstName, user.LastName, user.Login, user.Password, user.Role, user.Email);
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
    public async Task<IActionResult> GetAsync([FromQuery] string name)
    {
        _logger.LogInformation("{Method}: starting to find a user by user name({UserName})", nameof(GetAsync), name);

        if (string.IsNullOrEmpty(name))
        {
            _logger.LogWarning("{Method}: the parameter user name is missing", nameof(GetAsync));
            return BadRequest();
        }

        var user = await _userRepository.GetSingleAsync(x => x.FirstName.Equals(name, StringComparison.CurrentCultureIgnoreCase));

        if (user is null)
        {
            _logger.LogWarning("{Method}: no user found with user name {UserName}", nameof(GetAsync), name);
        }

        return Ok(user);
    }
}
