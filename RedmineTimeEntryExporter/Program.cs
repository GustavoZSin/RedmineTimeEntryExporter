using Microsoft.Extensions.Configuration;
using RedmineExporter.Services;
using RedmineTimeEntryExporter.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string url = config["Redmine:Url"];
            string token = config["Redmine:ApiToken"];
            int userId = int.Parse(config["Redmine:UserId"]);
            DateTime initialDate = DateTime.Parse(config["Redmine:InitialDate"]);
            DateTime finalDate = DateTime.Parse(config["Redmine:FinalDate"]);

            var client = new RedmineClient(url, token);
            var service = new TimeEntryService(client);
            var entries = await service.GetAllEntriesAsync(userId, initialDate, finalDate);

            await XlsxExporter.ExportAsync(entries, "TimeEntries.xlsx");

            Console.WriteLine("Export completed.");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Data formatting error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}