using ClosedXML.Excel;
using RedmineTimeEntryExporter.Models;

namespace RedmineTimeEntryExporter.Services;

public static class XlsxExporter
{
    public static async Task ExportAsync(List<TimeEntry> entries, string filePath)
    {
        if (entries == null || !entries.Any())
        {
            Console.WriteLine("Error: No entries found for export.");
            return;
        }

        try
        {
            using var workbook = new XLWorkbook();

            var grouped = entries
                .OrderBy(e => e.SpentOn)
                .GroupBy(e => $"{e.SpentOn:MM-yyyy}");

            foreach (var group in grouped)
            {
                var worksheet = workbook.Worksheets.Add(group.Key);

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Project";
                worksheet.Cell(1, 3).Value = "Activity";
                worksheet.Cell(1, 5).Value = "Spent_Hours";
                worksheet.Cell(1, 6).Value = "Date";
                worksheet.Cell(1, 7).Value = "Issue ID";
                worksheet.Cell(1, 8).Value = "User Name";
                worksheet.Cell(1, 4).Value = "Comment";

                var row = 2;
                foreach (var entry in group)
                {
                    worksheet.Cell(row, 1).Value = entry.Id;
                    worksheet.Cell(row, 2).Value = entry.Project?.Name;
                    worksheet.Cell(row, 3).Value = entry.Activity?.Name;
                    worksheet.Cell(row, 5).Value = entry.Hours;
                    worksheet.Cell(row, 6).Value = entry.SpentOn.ToString("yyyy-MM-dd");
                    worksheet.Cell(row, 7).Value = entry.Issue?.Id;
                    worksheet.Cell(row, 8).Value = entry.User?.Name;
                    worksheet.Cell(row, 4).Value = entry.Comments;
                    row++;
                }

                worksheet.Columns().AdjustToContents();
            }

            workbook.SaveAs(filePath);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error exporting to .xlsx:{ex.Message}");
        }
    }
}