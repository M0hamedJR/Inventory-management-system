using EmailService;
using InventrySystem.Extensions;
using InventrySystem.Hubs;
using InventrySystem.JwtFeatures;
using InventrySystem.Services;
using Microsoft.AspNetCore.Identity;
using NLog;
using InventrySystem.SubscribeTableDependencies;
using InventrySystem.MiddlewareExtensions;
using Microsoft.EntityFrameworkCore;
using Repository;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddScoped<JwtHandler>();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureSwagger();
builder.Services.AddAuthentication();
builder.Services.AddSignalR();
// DI
builder.Services.AddSingleton<DashboardHub>();
builder.Services.AddSingleton<SubscribeCategoryTableDependency>();


builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
    opt.TokenLifespan = TimeSpan.FromHours(2));


var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<DashboardService>();

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSingleton<SubscribeCategoryTableDependency>();
builder.Services.AddSingleton<SubscribeProductTableDependency>();
builder.Services.AddSingleton<SubscribeShelfTableDependency>();

// connect to the hardware
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()  // Required for SignalR
            .SetIsOriginAllowed(_ => true); // Allow SignalR dynamic connections
    });
});

var app = builder.Build();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No Connection String Was Found");

// Order is important for middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory System API v1");
    });
}

// CORS must be before Auth and SignalR
app.UseCors("AllowAll");

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting(); // Add this before authentication and endpoints
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

// SignalR hub mapping
app.MapHub<DashboardHub>("/dashboardHub");

// Configure other endpoints
app.MapControllers();

// Fallback for Angular routes (important!)
app.MapFallbackToFile("index.html");

// Table dependencies
var tableDependencyServices = app.Services.GetRequiredService<IEnumerable<ISubscribeTableDependency>>();
foreach (var service in tableDependencyServices)
{
    Console.WriteLine($"Subscribing to table dependency: {service.GetType().Name}");
    service.SubscribeTableDependency(connectionString);
}

app.UseSqlTableDependency<SubscribeCategoryTableDependency>(connectionString);
app.UseSqlTableDependency<SubscribeProductTableDependency>(connectionString);
app.UseSqlTableDependency<SubscribeShelfTableDependency>(connectionString);

app.Run();
