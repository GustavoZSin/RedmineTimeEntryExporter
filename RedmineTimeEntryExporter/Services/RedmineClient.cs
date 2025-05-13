using RedmineTimeEntryExporter.Models;
using System.Text.Json;

namespace RedmineExporter.Services;

public class RedmineClient : IRedmineClient
{
    private readonly HttpClient _httpClient;

    public RedmineClient(string baseUrl, string apiToken)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
        _httpClient.DefaultRequestHeaders.Add("X-Redmine-API-Key", apiToken);
    }

    public async Task<List<TimeEntry>> GetTimeEntriesAsync(int userId, DateTime from, DateTime to)
    {
        var all = new List<TimeEntry>();
        int offset = 0, limit = 100;
        bool more = true;

        while (more)
        {
            var url = $"/time_entries.json?user_id={userId}&from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}&limit={limit}&offset={offset}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(json);
                var entries = doc.RootElement.GetProperty("time_entries").Deserialize<List<TimeEntry>>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                all.AddRange(entries);
                offset += limit;
                more = offset < doc.RootElement.GetProperty("total_count").GetInt32();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching entries from Redmine: {ex.Message}");
                break;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error processing response JSON: {ex.Message}");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                break;
            }
        }

        return all;
    }
}