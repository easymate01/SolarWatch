using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SolarWatch.Data;
using SolarWatch.Services;
using SolarWatch.Services.Json;
using SolarWatch.Services.Repositories;
using System.Text;


var configuration = Configuration();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
AddAuthentication();

builder.Services.AddDbContext<UsersContext>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IWeatherDataProvider, WeatherProvider>();
builder.Services.AddTransient<IJsonProcessor, JsonProcessor>();

builder.Services.AddTransient<ICityRepository, CityRepository>();
builder.Services.AddTransient<ISunriseSunsetRepository, SunriseSunsetRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();

IConfiguration Configuration()
{
    return new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();
}

void AddAuthentication()
{
    var jwtSettings = configuration.GetSection("JwtSettings");

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["ValidIssuer"],
                ValidAudience = jwtSettings["ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings["IssuerSigningKey"])
                ),
            };
        });
}

