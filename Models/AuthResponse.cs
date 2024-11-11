using System.Text.Json.Serialization;

namespace MAUIApi.Models;

public class AuthResponse
{
  [JsonPropertyName("token")] public string Token { get; set; }

  [JsonPropertyName("user")] public User User { get; set; }
}