using BankTask.Application.Interfaces.Repositories;
using BankTask.Application.Interfaces.Services;
using BankTask.Application.Services;
using BankTask.DBManager;
using BankTask.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Connection Strings
var sqlServerConnectionString =
    builder.Configuration.GetConnectionString("SqlServer")
    ?? throw new InvalidOperationException(
        "SqlServer connection string not found.");

var postgreSqlConnectionString =
    builder.Configuration.GetConnectionString("PostgreSQL")
    ?? throw new InvalidOperationException(
        "PostgreSQL connection string not found.");

var connectionFactory = new ConnectionFactory(
    sqlServerConnectionString,
    postgreSqlConnectionString);

builder.Services.AddSingleton<IConnectionFactory>(connectionFactory);

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AccountRepository>();
builder.Services.AddScoped<TransactionRepository>();
builder.Services.AddScoped<AuditLogRepository>();

// Services
builder.Services.AddScoped<IUserService, UserService>();

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();