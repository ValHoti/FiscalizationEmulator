using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public sealed class BitSefClient : IDisposable
{
    private readonly string _baseUrl;
    private readonly string _secret;
    private readonly HttpClient _http = new HttpClient();

    public BitSefClient(string baseUrl, string sharedSecret)
    {
        _baseUrl = baseUrl.TrimEnd('/');
        _secret = sharedSecret;
    }

    public Task<string> FiscalJsonAsync(string json) => PostAsync("/api/bitsef/fiscal", json, "application/json");
    public Task<string> CommandJsonAsync(string json) => PostAsync("/api/bitsef/command", json, "application/json");
    public Task<string> CopyAsync(string invoiceNo) => PostAsync("/api/bitsef/copy/" + invoiceNo, "", null);
    public Task<string> PdfAsync(string invoiceNo) => PostAsync("/api/bitsef/pdf/" + invoiceNo, "", null);
    public Task<string> DirectCsvAsync(string type, string invoiceNo, string csv) => PostAsync("/api/bitsef/csv/" + type + "/" + invoiceNo, csv, "text/plain");

    public async Task<string> StatusAsync(string requestId)
    {
        using (var response = await _http.GetAsync(_baseUrl + "/api/bitsef/status/" + requestId))
        {
            string text = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            return text;
        }
    }

    private async Task<string> PostAsync(string path, string body, string mediaType)
    {
        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        string nonce = Guid.NewGuid().ToString("N");
        string bodyHash = Sha256Hex(body ?? "");
        string canonical = "POST\n" + path + "\n" + timestamp + "\n" + nonce + "\n" + bodyHash;
        string signature = HmacSha256Hex(canonical, _secret);

        using (var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl + path))
        {
            request.Headers.Add("X-BIT-Timestamp", timestamp);
            request.Headers.Add("X-BIT-Nonce", nonce);
            request.Headers.Add("X-BIT-Signature", signature);
            if (mediaType != null)
                request.Content = new StringContent(body ?? "", Encoding.UTF8, mediaType);

            using (var response = await _http.SendAsync(request))
            {
                string text = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    throw new Exception("BIT-SEF HTTP " + (int)response.StatusCode + ": " + text);
                return text;
            }
        }
    }

    private static string Sha256Hex(string value)
    {
        using (var sha = SHA256.Create())
            return ToHex(sha.ComputeHash(Encoding.UTF8.GetBytes(value)));
    }

    private static string HmacSha256Hex(string value, string secret)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            return ToHex(hmac.ComputeHash(Encoding.UTF8.GetBytes(value)));
    }

    private static string ToHex(byte[] bytes)
    {
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (byte b in bytes) sb.Append(b.ToString("x2"));
        return sb.ToString();
    }

    public void Dispose() => _http.Dispose();
}
