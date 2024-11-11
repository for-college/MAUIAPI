using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MAUIApi.Models;
using Microsoft.Maui.Controls;

namespace MAUIApi.Views.Auth;

public partial class Login : ContentPage
{
  private readonly HttpClient _httpClient;

  public Login()
  {
    InitializeComponent();
    _httpClient = new HttpClient();
  }

  private async void OnRegisterTapped(object sender, EventArgs e)
  {
    await Navigation.PushAsync(new Register());
  }

  private async void OnLoginButtonClicked(object? sender, EventArgs e)
  {
    var email = EmailEntry.Text;
    var password = PasswordEntry.Text;

    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
    {
      await DisplayAlert("Error", "Please enter a valid email/password.", "OK");
      return;
    }

    var loginResponse = await AuthenticateUser(email, password);

    if (loginResponse == null) return;

    await Navigation.PushAsync(new Home(loginResponse.User, loginResponse.Token));
  }

  private async Task<AuthResponse?> AuthenticateUser(string email, string password)
  {
    try
    {
      var loginData = new { email, password };
      var json = JsonSerializer.Serialize(loginData);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var response = await _httpClient.PostAsync("http://127.0.0.1:8000/api/login", content);

      switch (response.StatusCode)
      {
        case HttpStatusCode.Unauthorized:
          await DisplayAlert("Error", "Invalid email/password typed.", "OK");
          return null;
        case HttpStatusCode.UnprocessableEntity:
          await DisplayAlert("Error", "Invalid data typed.", "OK");
          return null;
      }

      if (!response.IsSuccessStatusCode)
      {
        var errorMessage = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"[BAD]: {response.StatusCode}: {errorMessage}");
        return null;
      }

      var responseJson = await response.Content.ReadAsStringAsync();

      return JsonSerializer.Deserialize<AuthResponse>(responseJson);
    }
    catch
    {
      await DisplayAlert("Error", "Network error", "OK");
      return null;
    }
  }
}