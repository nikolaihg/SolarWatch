using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SolarWatch.Api.Data;
using SolarWatch.Api.Repositories;
using SolarWatch.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var jwtIssuer = builder.Configuration["Jwt:ValidIssuer"];
var jwtAudience = builder.Configuration["Jwt:ValidAudience"];
var jwtKey = builder.Configuration["Jwt:SigningKey"];

if (string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience) || string.IsNullOrEmpty(jwtKey))
{
    throw new ApplicationException("JWT configuration is missing");
}


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

builder.Services.AddDbContext<AppDbContext>(options => { options.UseNpgsql(connectionString); });

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.AddControllers();

builder.Services.AddOpenApi("v2");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

// Register services
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ISolarDataService, SolarDataService>();
builder.Services.AddScoped<ISunriseSunsetService, SunriseSunsetService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// Register repositories
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ISolarDataRepository, SolarDataRepository>();

var app = builder.Build();

// Automatically apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var services = scope.ServiceProvider;
    try
    {
        Console.WriteLine("Applying Database Migrations...");
        dbContext.Database.Migrate();
        Console.WriteLine("Database Migrations Applied Successfully.");
        
        // Initialize roles
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { "User", "Admin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while migrating or seeding the database: {ex.Message}");
        throw;
    }
}


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();