using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartExpenseManagement.Domain.CQRS.Commands;
using SmartExpenseManagement.Domain.Entities;
using SmartExpenseManagement.Domain.Repositories;

namespace SmartExpenseManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class ExpenseGroupController : ControllerBase
{
    private readonly ILogger<ExpenseGroupController> _logger;
    private readonly IExpenseGroupRepository _expenseGroupRepository;

    public ExpenseGroupController(ILogger<ExpenseGroupController> logger, IExpenseGroupRepository expenseGroupRepository)
    {
        _logger = logger;
        _expenseGroupRepository = expenseGroupRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateExpenseGroupCommand expenseGroup)
    {
        _logger.LogInformation("{Method}: starting to create a new expense group({@ExpenseGroup})", nameof(CreateAsync), expenseGroup);
        var owner = User.Claims.First(x => x.Type.Equals("Id")).Value;
        var entity = new ExpenseGroup(expenseGroup.Description, expenseGroup.Users, owner);
        await _expenseGroupRepository.AddAsync(entity);

        _logger.LogInformation("{Method}: the expense group({@ExpenseGroup}) was successfully created", nameof(CreateAsync), entity);

        return Ok(entity);
    }
}

