using SmartExpenseManagement.Api.Options;
using SmartExpenseManagement.Api.Repository;

const string originsPolicy = "AllowAllOrigins";
var headersExposed = new[] { "Date", "Content-Type", "Content-Disposition", "Content-Length" };

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
_ = builder.Services.AddScoped<IUserRepository, UserRepository>();
_ = builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();

_ = builder.Services.AddControllers();
_ = builder.Services.AddEndpointsApiExplorer();
_ = builder.Services.AddSwaggerGen();
_ = builder.Services.AddCors(options =>
{
    options.AddPolicy(originsPolicy,
        b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders(headersExposed));
});
_ = builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("MongoConnection"));
_ = builder.Services.AddScoped<MongoContext>();

var app = builder.Build();

_ = app.UseSwagger();
_ = app.UseSwaggerUI();

_ = app.UseHttpsRedirection();
_ = app.UseAuthorization();
_ = app.UseCors(originsPolicy);
_ = app.MapControllers();

app.Run();
