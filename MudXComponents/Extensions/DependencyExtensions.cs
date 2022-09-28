using System;
using Microsoft.Extensions.DependencyInjection;

namespace MudXComponents.Extensions;

public static class DependencyExtensions
{
    public static void AddMudUIServices(this IServiceCollection service)
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
        //service.AddScoped<SiteManagerService>();
    }
}
