using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace AxiomSysPortal.Infrastructure;

public static class Helper
{
    private static readonly HttpClient _client = new HttpClient();

    public static string? GetClaimValue(ClaimsPrincipal user, string claimType)
    {
        return user.FindFirst(claimType)?.Value;
    }

    public static async Task<HttpResponseMessage> GetRequest(string url, string? token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        AddToken(request, token);
        return await _client.SendAsync(request);
    }

    public static async Task<HttpResponseMessage> PostRequest(string url, string json, string? token)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        AddToken(request, token);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _client.SendAsync(request);
    }

    public static async Task<HttpResponseMessage> PutRequest(string url, string json, string? token)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, url);
        AddToken(request, token);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _client.SendAsync(request);
    }

    public static async Task<HttpResponseMessage> DeleteRequest(string url, string? token)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, url);
        AddToken(request, token);
        return await _client.SendAsync(request);
    }

    private static void AddToken(HttpRequestMessage request, string? token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
