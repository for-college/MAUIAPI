using MAUIApi.Models;

namespace MAUIApi.Views;

public partial class Home : ContentPage
{
  private readonly HttpClient _httpClient;
  private string _token;
  private readonly User? _user;

  public Home(User user, string token)
  {
    InitializeComponent();

    _user = user;
    _token = token;
    _httpClient = new HttpClient();

    LoadAvatarAsync();

    nickname.Text = user.Nickname;
  }

  private async Task LoadAvatarAsync()
  {
    if (string.IsNullOrEmpty(_user?.Avatar)) return;

    try
    {
      var avatarUrl = $"http://localhost:8000/storage/{_user.Avatar}";
      var avatarStream = await _httpClient.GetStreamAsync(avatarUrl);
      avatar.Source = ImageSource.FromStream(() => avatarStream);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Failed to load avatar: {ex.Message}");
    }
  }
}