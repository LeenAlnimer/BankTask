using System.Text;
using BankTask.Application.Interfaces.Repositories;
using BankTask.Application.Interfaces.Security;
using BankTask.Application.Interfaces.Services;
using BankTask.Application.Services;
using BankTask.Authentication;
using BankTask.DBManager;
using BankTask.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

// Security
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// JWT
var jwtSecretKey =
    builder.Configuration["Jwt:SecretKey"]
    ?? throw new InvalidOperationException(
        "JWT secret key not found.");

var jwtIssuer =
    builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException(
        "JWT issuer not found.");

var jwtAudience =
    builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException(
        "JWT audience not found.");

builder.Services.AddScoped<IJwtService>(_ =>
    new JwtService(
        jwtSecretKey,
        jwtIssuer,
        jwtAudience));

// JWT Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecretKey)),

            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            ValidateAudience = true,
            ValidAudience = jwtAudience,

            ValidateLifetime = true,

            ClockSkew = TimeSpan.Zero
        };
    });

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Enter your JWT token."
        });

    options.AddSecurityRequirement(
        new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference =
                        new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },
                Array.Empty<string>()
            }
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();