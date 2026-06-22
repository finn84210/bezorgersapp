using System.Windows.Input;
using bezorgersapp.Services;

namespace bezorgersapp.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly AuthService _authService;
    private string _email = "bezorger@matrix.nl";
    private string _password = "Matrix123!";
    private string _message = "Demo accounts: bezorger@matrix.nl, admin@matrix.nl, klant@matrix.nl";

    public LoginViewModel(AuthService authService)
    {
        _authService = authService;
        LoginCommand = new Command(Login);
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public ICommand LoginCommand { get; }

    private void Login()
    {
        if (_authService.SignIn(Email, Password, out var message))
        {
            Message = message;
            ((AppShell)Shell.Current).ShowRoleShell(_authService.CurrentUser!.Role);
            return;
        }

        Message = message;
    }
}
