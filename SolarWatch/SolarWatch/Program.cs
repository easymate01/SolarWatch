using SolarWatch.Services;
using SolarWatch.Services.Json;
using SolarWatch.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

app.MapControllers();

app.Run();
