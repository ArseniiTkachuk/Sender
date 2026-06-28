using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sender.Application;
using Sender.Application.Interfaces;
using Sender.Persistence;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

var tokenKey = builder.Configuration["TokenKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        // ДОДАЙТЕ ЦЕЙ БЛОК:
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                // Зупиняємо стандартну порожню відповідь
                context.HandleResponse();

                // Встановлюємо свій статус і повідомлення
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsJsonAsync(new { message = "Ви не авторизовані." });
            }
        };
    });

builder.Services.AddAuthorization();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// Реєстрація БД
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Реєстрація репозиторіїв (тепер вони доступні в усьому проекті)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<AuthUser>();
builder.Services.AddScoped<ContactCRUD>();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<CustomAuthMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
