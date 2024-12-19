using _4thWallCafe.API;
using _4thWallCafe.App;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.ApsNetCore", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File("logs.txt", 
        rollingInterval: RollingInterval.Day, 
        rollOnFileSizeLimit: true)
    .CreateLogger();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

builder.Services.AddControllers();

// Provide ICustomerService for injection
var config = new AppConfiguration(builder.Configuration);
var sf = new ServiceFactory(config);
builder.Services.AddScoped(_ => sf.GenerateCustomerService());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();