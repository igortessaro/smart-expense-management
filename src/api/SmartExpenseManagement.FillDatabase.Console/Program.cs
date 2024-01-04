using SmartExpenseManagement.Domain.CQRS.Commands;
using System.Collections.Immutable;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

Console.WriteLine("Starting");

var baseUrl = "http://localhost:5224";
var userLogin = new { login = "igorstessaro57@gmail.com", password = "123456" };

using var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
httpClient.BaseAddress = new Uri(baseUrl);

var contentGetToken = new StringContent(JsonSerializer.Serialize(userLogin), Encoding.UTF8, "application/json");
using var tokenResponse = await httpClient.PostAsync("/api/login", contentGetToken);
var tokenString = await tokenResponse.Content.ReadAsStringAsync();
var tokenJobject = JsonSerializer.Deserialize<JsonObject>(tokenString);
var token = tokenJobject["token"].GetValue<string>();


httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

var directories = Directory.GetDirectories($"C:\\smart-expense-management-expenses\\", "*").ToList();
var expenses = directories.SelectMany(d => ProcessFiles(d)).ToList();

var expenses202301Tasks = expenses
    .Select(x =>
    {
        var content = new StringContent(JsonSerializer.Serialize(x), Encoding.UTF8, "application/json");
        return httpClient.PostAsync("/api/expense", content);
    })
    .ToArray();

Task.WaitAll(expenses202301Tasks);
Console.WriteLine("Finish");
Console.ReadKey();

IReadOnlyList<CreateExpenseCommand> ProcessFiles(string directory)
{
    var result = new List<CreateExpenseCommand>();
    var period = directory.Split("\\").Last();
    var files = Directory.GetFiles(directory);
    files.ToList().ForEach(f =>
    {
        var expenses = GetAllExpenseFromFile(f, period);
        result.AddRange(expenses);
    });

    return result.ToImmutableList();
}

IReadOnlyList<CreateExpenseCommand> GetAllExpenseFromFile(string filePath, string period)
{
    IList<CreateExpenseCommand> result = new List<CreateExpenseCommand>();
    using var streamReader = new StreamReader(filePath, Encoding.UTF8, true);
    while (!streamReader.EndOfStream)
    {
        var header = "Expense,Amount,Category,Comment,Due Date,Pay Day,Paid By";
        var line = streamReader.ReadLine();

        if (string.IsNullOrEmpty(line) || header.Equals(line))
        {
            continue;
        }

        var expense = CreateExpenseCommand(line, period);

        if (expense is null) continue;

        result.Add(expense);
    }

    return result.ToImmutableList();
}

CreateExpenseCommand? CreateExpenseCommand(string expenseText, string period)
{
    var expenseSplited = expenseText.Split(",");

    if (expenseSplited == null || !expenseSplited.Any() || expenseSplited.All(x => string.IsNullOrEmpty(x)))
    {
        return null;
    }

    var dates = DiscoverDates(expenseSplited);

    var createExpenseCommand = new CreateExpenseCommand()
    {
        Description = expenseSplited[0],
        Value = ParseToDecimal(expenseSplited[1]),
        Category = expenseSplited[2].ToUpper(),
        DueDate = dates.dueDate,
        PaydAt = dates.paydAt,
        UserId = DiscoverUser(expenseSplited.Last()),
        Period = period,
        ExpenseGroupId = "65972cb5630f6f3355234c3d"
    };

    return createExpenseCommand;
}

(DateTime? dueDate, DateTime? paydAt) DiscoverDates(string[] expenseSplited)
{
    DateTime? dueDate = null;
    DateTime? paydAt = null;

    if (expenseSplited.Length == 8)
    {
        dueDate = ParseToDateTime(expenseSplited[3] + expenseSplited[4]);
        paydAt = ParseToDateTime(expenseSplited[5] + expenseSplited[6]);
        return (dueDate, paydAt);
    }

    if (expenseSplited.Length == 6)
    {
        return (dueDate, paydAt);
    }

    if (string.IsNullOrEmpty(expenseSplited[3]))
    {
        paydAt = ParseToDateTime(expenseSplited[4] + expenseSplited[5]);
        return (dueDate, paydAt);
    }

    dueDate = ParseToDateTime(expenseSplited[3] + expenseSplited[4]);
    return (dueDate, paydAt);
}

decimal? ParseToDecimal(string value)
{
    if (string.IsNullOrEmpty(value))
    {
        return null;
    }

    if (!decimal.TryParse(value, out var result))
    {
        return null;
    }

    return result;
}

DateTime? ParseToDateTime(string value)
{
    if (string.IsNullOrEmpty(value))
    {
        return null;
    }

    if (!DateTime.TryParse(value, out var result))
    {
        return null;
    }

    return result;
}

string DiscoverUser(string userName)
{
    if (userName.Equals("Igor Tessaro"))
    {
        return "6597021f3d19925fbd6d6cd9";
    }

    return "659702853d19925fbd6d6cda";
}
