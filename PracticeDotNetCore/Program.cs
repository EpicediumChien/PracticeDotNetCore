using Microsoft.EntityFrameworkCore;
using PracticeDotNetCore.Data;
using PracticeDotNetCore.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddSwaggerGen();
// Get the Redis connection string
var redisConnectionString = builder.Configuration.GetConnectionString("Redis");

// Add the Redis distributed cache service
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    options.InstanceName = "TodoApp_"; // A prefix for all our cache keys
});

// *** IMPORTANT PART HERE ***
// Get the connection string from the configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add DbContext, connecting to PostgreSQL
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseNpgsql(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.MapGet("/", () => "Hello World!");

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () => System.Diagnostics.Process.GetCurrentProcess().ProcessName);

app.Run();
