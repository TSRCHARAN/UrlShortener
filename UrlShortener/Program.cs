using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using UrlShortener.Domain.Models;
using UrlShortener.Domain.Repositories;
using UrlShortener.Domain.Services;
using UrlShortener.Persistence.Repositories;
using UrlShortener.Services;
using NLog;
using NLog.Web;
using StackExchange.Redis;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("Init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    ConfigureServices(builder);


    // Add services to the container.
    builder.Services.AddControllersWithViews();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseForwardedHeaders();
    }
    else
    {
        app.UseExceptionHandler("/Error");

        app.UseStatusCodePagesWithReExecute("/Error/{0}");
        app.UseForwardedHeaders();

    }
    //app.UseHttpsRedirection();

    var provider = new FileExtensionContentTypeProvider();
    // Add new MIME type mappings
    provider.Mappings[".res"] = "application/octet-stream";
    provider.Mappings[".pexe"] = "application/x-pnacl";
    provider.Mappings[".nmf"] = "application/octet-stream";
    provider.Mappings[".mem"] = "application/octet-stream";
    provider.Mappings[".wasm"] = "application/wasm";

    app.UseStaticFiles();

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
        ContentTypeProvider = provider
    });

    app.UseRouting();

    app.UseCookiePolicy();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    // NLog: catch setup errors
    // logger.Error(ex, ex.Message);
    // throw;
    Console.WriteLine(ex.ToString());
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    // NLog.LogManager.Shutdown();
}
void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddHttpClient("ignoreSSL")
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
            };
        });
    ConfigurationManager configuration = builder.Configuration;

    var origins = configuration.GetSection("Origin_url").Get<string[]>();
    if (origins == null)
    {
        Console.WriteLine("origins is null");
    }
    else
    {
        Console.WriteLine(origins[1]);
    }

    builder.Services.AddCors(option =>
    {
        option.AddPolicy("CorsPolicy", builders =>
                 builders.WithOrigins(configuration.GetSection("Origin_url").Get<string[]>())
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
    });

    builder.Services.AddScoped<Microsoft.Extensions.Logging.ILogger, Microsoft.Extensions.Logging.Logger<UnitOfWork>>();

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IUrlService, UrlService>();
    builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    builder.Services.AddDbContext<UrlDbContext>(options =>
       options.UseMySql(builder.Configuration.GetConnectionString("MVCConnection"),
        new MySqlServerVersion("8.0.12-mysql"),
      mySqlOptions =>
      {
          mySqlOptions.EnableRetryOnFailure(
          maxRetryCount: 10,
          maxRetryDelay: TimeSpan.FromSeconds(30),
          errorNumbersToAdd: null);
      }));


    builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    {
        var configuration = builder.Configuration.GetSection("Redis")["ConnectionString"];
        return ConnectionMultiplexer.Connect(configuration);
    });

}
