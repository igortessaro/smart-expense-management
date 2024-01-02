using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SmartExpenseManagement.Api.Options;
using SmartExpenseManagement.Api.Repository;
using SmartExpenseManagement.Api.Services;

const string originsPolicy = "AllowAllOrigins";
var headersExposed = new[] { "Date", "Content-Type", "Content-Disposition", "Content-Length" };

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
_ = builder.Services.AddScoped<IUserRepository, UserRepository>();
_ = builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
_ = builder.Services.AddScoped<ITokenService, TokenService>();

_ = builder.Services.AddControllers();
var key = Encoding.ASCII.GetBytes("fedaf7d8863b48e197b9287d492b708e");
_ = builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
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
_ = app.UseAuthentication();
_ = app.UseAuthorization();
_ = app.UseCors(originsPolicy);
_ = app.MapControllers();

app.Run();
