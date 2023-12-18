using DAL;
using WebAPI;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using DAL.Repositories.Interfaces;
using DAL.Repositories;
using Microsoft.OpenApi.Models;
using NLog.Web;
using NLog;
using Infrastructure.AppSettingsModel;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddScoped<IAdminService, AdminService>();
    builder.Services.AddScoped<IBankingService, BankingService>();
    builder.Services.AddScoped<IValidationService, ValidationService>();
    builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

    // Add services to the container.
    builder.Services.Configure<ValidationSettings>(builder.Configuration.GetSection("ValidationSettings"));
    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Sample Backing System APIs",
            Version = "v1",
            Description = "APIs for banking with use.",
        });
    });

    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
    builder.Host.UseNLog();

    var app = builder.Build();
    app.ConfigureCustomExceptionMiddleware();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
