using RedmineTimeEntryExporter.Models;

namespace RedmineExporter.Services;

public interface IRedmineClient
{
    Task<List<TimeEntry>> GetTimeEntriesAsync(int userId, DateTime from, DateTime to);
}