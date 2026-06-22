using bezorgersapp.Services;
using bezorgersapp.ViewModels;
using bezorgersapp.Views;
using Microsoft.Extensions.Logging;

namespace bezorgersapp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<AppShell>();

        builder.Services.AddSingleton<StoreService>();
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<OrderService>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<DashboardViewModel>();
        builder.Services.AddSingleton<OrdersViewModel>();
        builder.Services.AddSingleton<CustomerPortalViewModel>();
        builder.Services.AddSingleton<AdminPanelViewModel>();
        builder.Services.AddTransient<EarningsViewModel>();
        builder.Services.AddTransient<OrderDetailViewModel>();

        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<DashboardPage>();
        builder.Services.AddSingleton<OrdersPage>();
        builder.Services.AddSingleton<MessagesPage>();
        builder.Services.AddSingleton<ProfilePage>();
        builder.Services.AddSingleton<CustomerPortalPage>();
        builder.Services.AddSingleton<AdminPanelPage>();
        builder.Services.AddTransient<EarningsPage>();
        builder.Services.AddTransient<OrderDetailPage>();

        return builder.Build();
    }
}
