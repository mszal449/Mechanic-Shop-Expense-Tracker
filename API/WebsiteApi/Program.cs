using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebsiteApi;
using WebsiteApi.Context;
using WebsiteApi.Models;
using WebsiteApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Environment variables, appsettings and secret values
builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddUserSecrets<Program>();
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json");
}


builder.Services.AddControllers();
builder.Services.AddScoped<JobService>();
builder.Services.AddScoped<ExpenseService>();
builder.Services.AddHttpClient();


// Register DbContext
builder.Services.AddDbContext<DataContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer(
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MechanicShopExpenseTracker;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False",
        options => options.EnableRetryOnFailure());
});


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
// Use CORS
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
