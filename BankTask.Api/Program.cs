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

// Create Connection Factory
var connectionFactory = new ConnectionFactory(
    sqlServerConnectionString,
    postgreSqlConnectionString);

builder.Services.AddSingleton<IConnectionFactory>(connectionFactory);



// Repositories
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AccountRepository>();
builder.Services.AddScoped<TransactionRepository>();
builder.Services.AddScoped<AuditLogRepository>();

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// HTTP Request Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();