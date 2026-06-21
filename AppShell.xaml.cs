using bezorgersapp.Views;

namespace bezorgersapp;

public partial class AppShell : Shell
{
    public AppShell(
        DashboardPage dashboardPage,
        OrdersPage ordersPage,
        MessagesPage messagesPage,
        ProfilePage profilePage)
    {
        InitializeComponent();

        Items.Add(new TabBar
        {
            Route = "MainTabs",
            Items =
            {
                new ShellContent
                {
                    Title = "Overzicht",
                    Route = nameof(DashboardPage),
                    Content = dashboardPage
                },
                new ShellContent
                {
                    Title = "Taken",
                    Route = nameof(OrdersPage),
                    Content = ordersPage
                },
                new ShellContent
                {
                    Title = "Berichten",
                    Route = nameof(MessagesPage),
                    Content = messagesPage
                },
                new ShellContent
                {
                    Title = "Profiel",
                    Route = nameof(ProfilePage),
                    Content = profilePage
                }
            }
        });

        Routing.RegisterRoute(nameof(OrderDetailPage), typeof(OrderDetailPage));
        Routing.RegisterRoute(nameof(EarningsPage), typeof(EarningsPage));
    }
}
