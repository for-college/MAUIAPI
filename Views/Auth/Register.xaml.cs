using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using MAUIApi.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace MAUIApi.Views.Auth;

public partial class Register : ContentPage
{
  private readonly HttpClient _httpClient;

  private Stream? _avatarStream;

  public Register()
  {
    InitializeComponent();
    _httpClient = new HttpClient();
  }

  private async void OnPickAvatarButtonClicked(object? sender, EventArgs e)
  {
    try
    {
      var result = await FilePicker.PickAsync(new PickOptions
      {
        FileTypes = FilePickerFileType.Images,
        PickerTitle = "Выберите изображение"
      });

      if (result == null) return;

      await using var stream = await result.OpenReadAsync();
      var tempFilePath = Path.Combine(FileSystem.CacheDirectory, result.FileName);

      await using var tempFileStream = File.Create(tempFilePath);
      await stream.CopyToAsync(tempFileStream);
      tempFileStream.Close();

      _avatarStream = File.OpenRead(tempFilePath);

      await DisplayAlert("Аватар выбран", result.FileName, "OK");
    }
    catch (Exception exception)
    {
      Console.WriteLine(exception);
      await DisplayAlert("Ошибка", "Не удалось выбрать изображение", "OK");
    }
  }

  private async void OnRegisterButtonClicked(object? sender, EventArgs e)
  {
    try
    {
      var user = new User
      {
        Name = NameEntry.Text,
        Password = PasswordEntry.Text,
        LastName = SurnameEntry.Text,
        Patronymic = PatronymicEntry.Text,
        Sex = MaleRadioButton.IsChecked,
        Birthday = BirthdayPicker.Date,
        Email = EmailEntry.Text,
        Nickname = NicknameEntry.Text,
        Phone = PhoneEntry.Text
      };

      var content = new MultipartFormDataContent();

      content.Add(new StringContent(user.Name), "name");
      content.Add(new StringContent(user.Password), "password");
      content.Add(new StringContent(user.LastName), "surname");
      content.Add(new StringContent(user.Patronymic ?? ""), "patronymic");
      content.Add(new StringContent(user.Sex ? "1" : "0"), "sex");
      content.Add(new StringContent(user.Birthday.ToString("yyyy-MM-dd")), "birthday");
      content.Add(new StringContent(user.Email), "email");
      content.Add(new StringContent(user.Nickname), "nickname");
      content.Add(new StringContent(user.Phone), "phone");

      if (_avatarStream != null) content.Add(new StreamContent(_avatarStream), "avatar", "avatar.jpg");

      var response = await _httpClient.PostAsync("http://127.0.0.1:8000/api/register", content);

      Console.WriteLine($"Response status code: {response.StatusCode}");

      var responseJson = await response.Content.ReadAsStringAsync();
      Console.WriteLine($"Response body: {responseJson}");

      if (!response.IsSuccessStatusCode)
      {
        await DisplayAlert("Ошибка", "Не удалось зарегистрировать пользователя", "OK");
        return;
      }

      var data = JsonSerializer.Deserialize<AuthResponse>(responseJson);

      if (data == null) return;

      await Navigation.PushAsync(new Home(data.User, data.Token));
    }
    catch (Exception ex)
    {
      Console.WriteLine($"[BAD]: {ex.Message}");
      await DisplayAlert("Ошибка", "Произошла ошибка при регистрации", "OK");
    }
  }
}