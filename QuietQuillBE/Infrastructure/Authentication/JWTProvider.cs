using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Application.Abstraction.Authentication;

namespace Infrastructure.Authentication;

internal sealed class JWTProvider : IJWTProvider
{
    private readonly HttpClient _httpClient;
    
    public JWTProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetForCredentialsAsync(string email, string password)
    {
        var request = new { email, password, returnSecureToken = true };
        var response = await _httpClient.PostAsJsonAsync("", request);
        
        var authToken = await response.Content.ReadFromJsonAsync<AuthToken>();
        
        
        return authToken.IdToken;
    }
}

public class AuthToken
{
    [JsonPropertyName("kind")]
    public string Kind { get; set; }
    [JsonPropertyName("localId")]
    public string LocalId { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }
    [JsonPropertyName("idToken")]
    public string IdToken { get; set; }
    [JsonPropertyName("registered")]
    public bool Registered { get; set; }
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
    [JsonPropertyName("expiresIn")]
    public string ExpiresIn { get; set; }
}