using bezorgersapp.Views;

namespace bezorgersapp;

public partial class AppShell : Shell
{
    private readonly LoginPage _loginPage;
    private readonly DashboardPage _dashboardPage;
    private readonly OrdersPage _ordersPage;
    private readonly MessagesPage _messagesPage;
    private readonly ProfilePage _profilePage;
    private readonly CustomerPortalPage _customerPortalPage;
    private readonly AdminPanelPage _adminPanelPage;

    public AppShell(
        LoginPage loginPage,
        DashboardPage dashboardPage,
        OrdersPage ordersPage,
        MessagesPage messagesPage,
        ProfilePage profilePage,
        CustomerPortalPage customerPortalPage,
        AdminPanelPage adminPanelPage)
    {
        InitializeComponent();
        _loginPage = loginPage;
        _dashboardPage = dashboardPage;
        _ordersPage = ordersPage;
        _messagesPage = messagesPage;
        _profilePage = profilePage;
        _customerPortalPage = customerPortalPage;
        _adminPanelPage = adminPanelPage;

        ShowLogin();

        Routing.RegisterRoute(nameof(OrderDetailPage), typeof(OrderDetailPage));
        Routing.RegisterRoute(nameof(EarningsPage), typeof(EarningsPage));
    }

    public void ShowLogin()
    {
        Items.Clear();
        Items.Add(new ShellContent
        {
            Title = "Inloggen",
            Route = nameof(LoginPage),
            Content = _loginPage
        });
    }

    public void ShowRoleShell(string role)
    {
        Items.Clear();

        if (role == "Admin")
        {
            Items.Add(new ShellContent
            {
                Title = "Admin",
                Route = nameof(AdminPanelPage),
                Content = _adminPanelPage
            });
            return;
        }

        if (role == "Klant")
        {
            Items.Add(new ShellContent
            {
                Title = "Klant",
                Route = nameof(CustomerPortalPage),
                Content = _customerPortalPage
            });
            return;
        }

        Items.Add(new TabBar
        {
            Route = "MainTabs",
            Items =
            {
                new ShellContent
                {
                    Title = "Overzicht",
                    Route = nameof(DashboardPage),
                    Content = _dashboardPage
                },
                new ShellContent
                {
                    Title = "Taken",
                    Route = nameof(OrdersPage),
                    Content = _ordersPage
                },
                new ShellContent
                {
                    Title = "Berichten",
                    Route = nameof(MessagesPage),
                    Content = _messagesPage
                },
                new ShellContent
                {
                    Title = "Profiel",
                    Route = nameof(ProfilePage),
                    Content = _profilePage
                }
            }
        });
    }
}
