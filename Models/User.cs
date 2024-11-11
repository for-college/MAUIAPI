using System;
using System.Text.Json.Serialization;

namespace MAUIApi.Models;

public class User
{
  [JsonPropertyName("id")] public ulong Id { get; set; }

  [JsonPropertyName("name")] public string Name { get; set; }

  [JsonPropertyName("surname")] public string LastName { get; set; }

  [JsonPropertyName("patronymic")] public string? Patronymic { get; set; }

  [JsonPropertyName("password")] public string? Password { get; set; }

  [JsonPropertyName("email")] public string Email { get; set; }

  [JsonPropertyName("nickname")] public string Nickname { get; set; }

  [JsonPropertyName("phone")] public string Phone { get; set; }

  [JsonPropertyName("sex")] public bool Sex { get; set; }

  [JsonPropertyName("avatar")] public string? Avatar { get; set; }

  [JsonPropertyName("birthday")] public DateTime Birthday { get; set; }

  [JsonPropertyName("role_id")] public ulong RoleId { get; set; }

  [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }

  [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
}