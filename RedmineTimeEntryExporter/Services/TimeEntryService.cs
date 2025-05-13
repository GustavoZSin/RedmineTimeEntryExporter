using RedmineTimeEntryExporter.Models;

namespace RedmineExporter.Services;

public class TimeEntryService
{
    private readonly IRedmineClient _client;

    public TimeEntryService(IRedmineClient client)
    {
        _client = client;
    }

    public async Task<List<TimeEntry>> GetAllEntriesAsync(int userId, DateTime from, DateTime to)
    {
        if (from > to)
        {
            Console.WriteLine("Error: Start date cannot be later than end date.");
            return new List<TimeEntry>();
        }

        try
        {
            var entries = await _client.GetTimeEntriesAsync(userId, from, to);
            return entries.OrderBy(x => x.SpentOn).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting entries: {ex.Message}");
            return new List<TimeEntry>();
        }
    }
}