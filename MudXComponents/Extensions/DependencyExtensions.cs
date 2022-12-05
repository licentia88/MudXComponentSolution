using Microsoft.Extensions.DependencyInjection;

namespace MudXComponents.Extensions;

public static class DependencyExtensions
{
    public static void RegisterMudXComponents(this IServiceCollection service)
    {
        InjectViewModels(service);
        InjectServices(service);
    }

    public static void InjectViewModels(this IServiceCollection service)
    {
        
        //service.AddScoped<ObservableCollection<NotificationMessage>>();
    }

    public static void InjectServices(this IServiceCollection service)
    {
        //service.AddMemoryCache();
        //service.AddScoped<JavaScriptService>();
        //service.AddScoped<PageMemoryService>();
        //service.AddScoped<SiteManagerService>();
    }
}
