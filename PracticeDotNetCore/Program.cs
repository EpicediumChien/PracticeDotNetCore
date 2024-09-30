var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () => System.Diagnostics.Process.GetCurrentProcess().ProcessName);

app.Run();
