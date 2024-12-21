using System.Text;
using _4thWallCafe.API;
using _4thWallCafe.API.Authentication;
using _4thWallCafe.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

var config = new AppConfiguration(builder.Configuration);
// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var loggerConfig = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.ApsNetCore", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File("logs.txt", 
        rollingInterval: RollingInterval.Day, 
        rollOnFileSizeLimit: true);

    //Add Database Logging if Enables in appsettings.json
if (builder.Configuration.GetValue<bool>("Logging:DbLogging:Enabled"))
{
    loggerConfig.WriteTo.MSSqlServer(
        connectionString: builder.Configuration["ConnectionString"],
        tableName: "API_LogEvents",
        appConfiguration: builder.Configuration,
        restrictedToMinimumLevel: config.GetDbLogEventLevel(),
        autoCreateSqlTable: true);
}

Log.Logger = loggerConfig.CreateLogger();

builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

builder.Services.AddControllers();

// Provide ICustomerService for injection
var sf = new ServiceFactory(config);
builder.Services.AddScoped(_ => sf.GenerateCustomerService());
builder.Services.AddScoped(_ => sf.GenerateCafeOrderService());
builder.Services.AddScoped<IJwtService, JwtService>();

// Configure JWS Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Secret"])),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });

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