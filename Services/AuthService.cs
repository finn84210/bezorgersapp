using bezorgersapp.Models;

namespace bezorgersapp.Services;

public record SignedInUser(string Name, string Email, string Role);

public class AuthService
{
    private readonly StoreService _storeService;
    private readonly List<(string Name, string Email, string Password, string Role)> _users =
    [
        ("Bezorger", "bezorger@matrix.nl", "Matrix123!", "Bezorger"),
        ("Admin", "admin@matrix.nl", "Matrix123!", "Admin"),
        ("Klant", "klant@matrix.nl", "Matrix123!", "Klant")
    ];

    public AuthService(StoreService storeService)
    {
        _storeService = storeService;
    }

    public SignedInUser? CurrentUser { get; private set; }

    public bool SignIn(string email, string password, out string message)
    {
        var user = _users.FirstOrDefault(user =>
            user.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase) &&
            user.Password == password);

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            message = "Geen toegang: e-mail of wachtwoord klopt niet.";
            return false;
        }

        CurrentUser = new SignedInUser(user.Name, user.Email, user.Role);
        Preferences.Set("current_user_email", CurrentUser.Email);
        Preferences.Set("current_user_role", CurrentUser.Role);
        message = "Inloggen gelukt.";
        return true;
    }

    public bool RegisterCustomer(string name, string email, string password, out string message)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            message = "Vul naam, e-mail en wachtwoord in.";
            return false;
        }

        if (_users.Any(user => user.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase)) ||
            _storeService.Customers.Any(customer => customer.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            message = "Dit e-mailadres bestaat al.";
            return false;
        }

        _users.Add((name.Trim(), email.Trim(), password, "Klant"));
        _storeService.AddCustomer(name.Trim(), email.Trim());
        message = "Account aangemaakt. Je kunt nu bestellen.";
        return true;
    }

    public void SignOut()
    {
        CurrentUser = null;
        Preferences.Remove("current_user_email");
        Preferences.Remove("current_user_role");
    }
}
