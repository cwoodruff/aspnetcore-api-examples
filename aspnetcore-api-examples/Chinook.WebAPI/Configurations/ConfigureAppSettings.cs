namespace Chinook.WebAPI.Configurations;

public static class ConfigureAppSettings
{
    public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(s => configuration.GetSection("AppSettings").Bind(s));

        return services;
    }
}