using System.Text;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SolarWatch.Api.Data;
using SolarWatch.Api.Repositories;
using SolarWatch.Api.Services;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

string jwtIssuer = Environment.GetEnvironmentVariable("JWT_VALID_ISSUER")!;
string jwtAudience = Environment.GetEnvironmentVariable("JWT_VALID_AUDIENCE")!;
string jwtKey = Environment.GetEnvironmentVariable("JWT_SIGNING_KEY")!;

string BuildConnectionString()
{
    var host = builder.Configuration["DB_HOST"] ?? throw new ArgumentException("DB_HOST is required");
    var port = builder.Configuration["DB_PORT"] ?? throw new ArgumentException("DB_PORT is required");
    var database = builder.Configuration["DB_NAME"] ?? throw new ArgumentException("DB_NAME is required");
    var username = builder.Configuration["DB_USER"] ?? throw new ArgumentException("DB_USER is required");
    var password = builder.Configuration["DB_PASSWORD"] ?? throw new ArgumentException("DB_PASSWORD is required");

    return $"Host={host};Port={port};Username={username};Password={password};Database={database}";
}

var connectionString = BuildConnectionString();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        });


builder.Services.AddAuthorizationBuilder()
    .AddDefaultPolicy("User", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build())
    .AddPolicy("Admin", new AuthorizationPolicyBuilder()
        .RequireRole("Admin")
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build());

builder.Services.AddDbContext<SolarWatchDbContext>(options => { options.UseNpgsql(connectionString); });
builder.Services.AddDbContext<UserDbContext>(options => { options.UseNpgsql(connectionString); });

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<UserDbContext>().AddDefaultTokenProviders();

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

builder.Services.AddScoped<ISunriseSunsetService, SunriseSunsetService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ISolarDataRepository, SolarDataRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();