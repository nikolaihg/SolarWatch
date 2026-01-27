using System.Text;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SolarWatch.Api.Data;
using SolarWatch.Api.Repositories;
using SolarWatch.Api.Services;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

string jwtIssuer = Environment.GetEnvironmentVariable("JWT_VALID_ISSUER")!;
string jwtAudience = Environment.GetEnvironmentVariable("JWT_VALID_AUDIENCE")!;
string jwtExpires = Environment.GetEnvironmentVariable("JWT_EXPIRES_IN_MINUTES")!;
string jwtKey = Environment.GetEnvironmentVariable("JWT_SIGNING_KEY")!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
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

builder.Services.AddAuthorization();

builder.Services.AddDbContext<SolarWatchDbContext>(options =>
{
    var host = builder.Configuration["DB_HOST"] ?? throw new ArgumentException("DB_HOST is required");
    var port = builder.Configuration["DB_PORT"] ?? throw new ArgumentException("DB_PORT is required");
    var database = builder.Configuration["DB_NAME"] ?? throw new ArgumentException("DB_NAME is required");
    var username = builder.Configuration["DB_USER"] ?? throw new ArgumentException("DB_USER is required");
    var password = builder.Configuration["DB_PASSWORD"] ?? throw new ArgumentException("DB_PASSWORD is required");

    var connectionString = $"Host={host};Port={port};Username={username};Password={password};Database={database}";
    options.UseNpgsql(connectionString);
});

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ISunriseSunsetService, SunriseSunsetService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ISolarDataRepository, SolarDataRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();