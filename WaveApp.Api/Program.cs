using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WaveApp.Application.Interfaces;
using WaveApp.Core.Entities;
using WaveApp.Core.Interfaces;
using WaveApp.Infrastructure.Data;
using WaveApp.Infrastructure.Services;



var builder = WebApplication.CreateBuilder(args);


// =========================================================
// DATABASE
// =========================================================

builder.Services.AddDbContext<WaveDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("UserDatabase")
    ));


// =========================================================
// APPLICATION SERVICES
// =========================================================

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IProductService, ProductService>();


// =========================================================
// PASSWORD HASHER
// =========================================================

builder.Services.AddScoped<
    IPasswordHasher<Login>,
    PasswordHasher<Login>>();


// =========================================================
// CONTROLLERS
// =========================================================

builder.Services.AddControllers();
// =========================================================
// JWT AUTHENTICATION
// =========================================================

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "JWT signing key is not configured.");

var jwtIssuer =
    builder.Configuration["Jwt:Issuer"];

var jwtAudience =
    builder.Configuration["Jwt:Audience"];


builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)),

                ClockSkew = TimeSpan.Zero
            };
    });


builder.Services.AddAuthorization();

// =========================================================
// CORS
// =========================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueApp", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://127.0.0.1:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// =========================================================
// SWAGGER
// =========================================================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// =========================================================
// BUILD
// =========================================================

var app = builder.Build();


// =========================================================
// SWAGGER
// =========================================================

app.UseSwagger();
app.UseSwaggerUI();


// =========================================================
// CORS
// =========================================================

app.UseCors("VueApp");


// =========================================================
// CONTROLLERS
// =========================================================

app.MapControllers();

app.Run();