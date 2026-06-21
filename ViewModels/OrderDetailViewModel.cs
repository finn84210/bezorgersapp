using System.Windows.Input;
using bezorgersapp.Models;
using bezorgersapp.Services;

namespace bezorgersapp.ViewModels;

public class OrderDetailViewModel : BaseViewModel, IQueryAttributable
{
    private readonly OrderService _orderService;
    private Order? _order;
    private string _selectedStatus = string.Empty;
    private string _locationText = "Nog geen locatie opgehaald.";
    private ImageSource? _deliveryPhoto;

    public OrderDetailViewModel(OrderService orderService)
    {
        _orderService = orderService;

        StatusOptions =
        [
            "In verwerking",
            "Onderweg",
            "Bezorgd",
            "Niet bezorgd"
        ];

        LoadOrderCommand = new Command<int>(async id => await LoadOrderAsync(id));
        UpdateStatusCommand = new Command(async () => await UpdateStatusAsync());
        GetLocationCommand = new Command(async () => await GetLocationAsync());
        TakePhotoCommand = new Command(async () => await TakePhotoAsync());
    }

    public Order? Order
    {
        get => _order;
        set => SetProperty(ref _order, value);
    }

    public string SelectedStatus
    {
        get => _selectedStatus;
        set => SetProperty(ref _selectedStatus, value);
    }

    public List<string> StatusOptions { get; }

    public string LocationText
    {
        get => _locationText;
        set => SetProperty(ref _locationText, value);
    }

    public ImageSource? DeliveryPhoto
    {
        get => _deliveryPhoto;
        set => SetProperty(ref _deliveryPhoto, value);
    }

    public ICommand LoadOrderCommand { get; }
    public ICommand UpdateStatusCommand { get; }
    public ICommand GetLocationCommand { get; }
    public ICommand TakePhotoCommand { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var idValue) && int.TryParse(idValue.ToString(), out var id))
        {
            LoadOrderCommand.Execute(id);
        }
    }

    private async Task LoadOrderAsync(int id)
    {
        try
        {
            IsBusy = true;
            Order = await _orderService.GetOrderByIdAsync(id);

            if (Order is null)
            {
                await Shell.Current.DisplayAlertAsync("Niet gevonden", "Deze bestelling kon niet worden gevonden.", "OK");
                return;
            }

            SelectedStatus = Order.Status;
        }
        catch
        {
            await Shell.Current.DisplayAlertAsync("Fout", "De bestelling kon niet worden geladen.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task UpdateStatusAsync()
    {
        if (Order is null)
        {
            return;
        }

        try
        {
            IsBusy = true;

            // De gekozen status wordt opgeslagen in de lokale orderservice.
            var updateGelukt = await _orderService.UpdateOrderStatusAsync(Order.Id, SelectedStatus);

            if (updateGelukt)
            {
                Order.Status = SelectedStatus;
                OnPropertyChanged(nameof(Order));
                await Shell.Current.DisplayAlertAsync("Status bijgewerkt", "De nieuwe status is opgeslagen.", "OK");
            }
            else
            {
                Order.Status = SelectedStatus;
                OnPropertyChanged(nameof(Order));
                await Shell.Current.DisplayAlertAsync("Niet gelukt", "De status kon niet worden bijgewerkt.", "OK");
            }
        }
        catch
        {
            await Shell.Current.DisplayAlertAsync("Fout", "De status kon niet worden opgeslagen.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task GetLocationAsync()
    {
        try
        {
            IsBusy = true;

            // Mobiele functie: huidige GPS locatie ophalen.
            var location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Medium,
                Timeout = TimeSpan.FromSeconds(10)
            });

            LocationText = location is null
                ? "Locatie kon niet worden opgehaald."
                : $"Latitude: {location.Latitude:F6}, Longitude: {location.Longitude:F6}";
        }
        catch
        {
            LocationText = "Locatie kon niet worden opgehaald. Controleer de locatie-permissie.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task TakePhotoAsync()
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await Shell.Current.DisplayAlertAsync("Camera", "Camera wordt niet ondersteund op dit apparaat.", "OK");
                return;
            }

            // Mobiele functie: foto maken als bewijs van bezorging.
            var photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo is not null)
            {
                DeliveryPhoto = ImageSource.FromFile(photo.FullPath);
            }
        }
        catch
        {
            await Shell.Current.DisplayAlertAsync("Foto mislukt", "De foto kon niet worden gemaakt.", "OK");
        }
    }
}
