using System.Text;
using Backend.Data;
using Backend.Entities;
using Backend.Repositories;
using Backend.Repositories.Interfaces;
using Backend.Services;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); // Registers all controllers in the project.

// JWT bearer config vars

var secret = builder.Configuration["Jwt:Secret"];
if(string.IsNullOrEmpty(secret))
{
    throw new ArgumentNullException("Secret key cannot be null.");
}

var issuer = builder.Configuration["Jwt:Issuer"];
if (string.IsNullOrEmpty(issuer))
{
    throw new ArgumentNullException("Issuer cannot be null.");
}

var audience = builder.Configuration["Jwt:Audience"];
if(string.IsNullOrEmpty(audience))
{
    throw new ArgumentNullException("Audience cannot be null.");
}

// Database connection string setup

var dbPassword = builder.Configuration["DbPassword"];
if (string.IsNullOrEmpty(dbPassword))
{
    throw new ArgumentNullException("Database password cannot be null. Make sure it is stored in user secrets.");
}

var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
if(connectionStringTemplate == null)
{
    throw new ArgumentNullException("Database connection string (no password) is invalid");
}

var finalConnectionString = connectionStringTemplate.Replace("{DbPassword}", dbPassword);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(finalConnectionString);
});

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "EpicProject API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Custom application services.
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddScoped<ITokenProviderService, TokenProviderService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICodeExecutionService, CodeExecutionService>();
builder.Services.AddScoped<ICodeExecutionService, CodeExecutionService>();
builder.Services.AddScoped<ICodingProblemRepository, CodingProblemRepository>();
builder.Services.AddScoped<IProblemEvaluationService, ProblemEvaluationService>();
builder.Services.AddScoped<ICodingProblemService, CodingProblemService>();
builder.Services.AddScoped<IRewardService, RewardService>();


// Basic identity framework options.
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;

    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._+";

    // Temporary, change for production use.
    options.Lockout.AllowedForNewUsers = false; // Disable for new users.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.Zero; // No lockout time.
    options.Lockout.MaxFailedAccessAttempts = int.MaxValue; // Practically disable lockout.
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Identity framework specific services.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<SignInManager<User>>();
builder.Services.AddScoped<ICodingProblemService, CodingProblemService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    // Todo on FE: if 401 unauthorized is returned to the FE, redirect the user to the login page.
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseRouting();

app.UseCors("AllowReactApp");

app.UseAuthentication();

app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();