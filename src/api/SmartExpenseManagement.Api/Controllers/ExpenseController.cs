using SmartExpenseManagement.Api.Commands;
using SmartExpenseManagement.Api.Repository;
using SmartExpenseManagement.Api.Repository.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SmartExpenseManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExpenseController : ControllerBase
{
    private readonly ILogger<ExpenseController> _logger;
    private readonly IExpenseRepository _expenseRepository;

    public ExpenseController(ILogger<ExpenseController> logger, IExpenseRepository expenseRepository)
    {
        _logger = logger;
        _expenseRepository = expenseRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateExpenseCommand expense)
    {
        _logger.LogInformation("{Method}: starting to create a new expense({@Expense})", nameof(CreateAsync), expense);

        var entity = new Expense(expense.UserId, expense.Description, expense.Value);
        await _expenseRepository.AddAsync(entity);

        _logger.LogInformation("{Method}: the expense({@Expense}) was successfully created", nameof(CreateAsync), entity);

        return Ok(entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] UpdateExpenseCommand command)
    {
        _logger.LogInformation("{Method}: starting to update a expense({@Expense}) with id({Id})", nameof(UpdateAsync), command, id);

        var entity = await _expenseRepository.GetSingleAsync(x => x.Id == id);

        if (entity is null)
        {
            _logger.LogWarning("{Method}: expense with id({Id}) was not found", nameof(UpdateAsync), id);
            return BadRequest("Expense was not found");
        }

        entity.Description = command.Description;
        entity.Value = command.Value;

        await _expenseRepository.UpdateAsync(entity);

        _logger.LogInformation("{Method}: the expense({@Expense}) was successfully updated", nameof(UpdateAsync), entity);

        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        _logger.LogInformation("{Method}: starting to delete a expense with id({Id})", nameof(DeleteAsync), id);

        await _expenseRepository.DeleteAsync(x => x.Id == id);

        return Ok();
    }
}
