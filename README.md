# Redmine Time Entry Exporter

This project is a tool designed to export time entries from Redmine to an Excel file. It uses the Redmine API to fetch time entries for a specific user over a defined date range and exports them to a `.xlsx` file containing project details, activity, hours, comments, and other relevant data.

## Project Structure

### 1. `Program.cs`

The `Program.cs` file contains the application’s entry point. It configures dependencies, reads settings from `appsettings.json`, calls the Redmine API, and generates the export file.

- **Configuration**:
    - Reads Redmine URL, API token, user ID, and date range from `appsettings.json`.

- **Redmine API Call**:
    - Uses the `RedmineClient` to fetch all time entries within the given range.

- **Excel Export**:
    - Uses the `ExcelExporter` service to generate a `.xlsx` file from the time entry data.

### 2. `TimeEntryService.cs`

The `TimeEntryService.cs` file defines the logic for accessing time entries via the Redmine API.

- **Main Method**: `GetAllEntriesAsync`
    - Accepts a user ID and a date range and returns all time entries ordered by the `SpentOn` date.
    - Uses the `IRedmineClient` to retrieve the entries.

### 3. `RedmineClient.cs`

The `RedmineClient.cs` implements communication with the Redmine API.

- **Main Method**: `GetTimeEntriesAsync`
    - Performs paginated requests to the Redmine API to retrieve time entries for a user in the given date range.
    - Accumulates all pages until the full dataset is retrieved.

### 4. `ExcelExporter.cs`

The `ExcelExporter.cs` file contains logic for exporting time entries to an Excel file using the ClosedXML library.

- **Main Method**: `ExportAsync`
    - Accepts a list of time entries and generates an Excel file.
    - Entries are grouped by month and year, with each group written to a separate worksheet.
    - Each row includes ID, Project, Activity, Comments, Hours, Date, Issue ID, and User Name.

### 5. `Models/TimeEntry.cs`

The `Models/TimeEntry.cs` file defines the data models representing time entry data and their relationships to related entities like `Project`, `Activity`, `User`, `Issue`, and `CustomField`.

- **Key Classes**:
    - `TimeEntry`: Represents a time entry record in Redmine.
    - `Project`: Project associated with the time entry.
    - `Issue`: The issue/ticket linked to the time entry.
    - `User`: The user who logged the time entry.
    - `Activity`: The type of activity performed.
    - `CustomField`: Any additional custom fields attached to the entry.

## Execution Flow

1. **Load Configuration**:
    - Configuration is read from `appsettings.json`.

2. **Query Redmine API**:
    - The Redmine API is queried via `RedmineClient` for time entries matching the configured user ID and date range.

3. **Export to Excel**:
    - Time entries are grouped by year/month and written to separate worksheets.
    - Columns include detailed time entry information.

4. **Save the File**:
    - The `.xlsx` file is generated and saved in the execution directory.

## Error Handling

The code includes basic error handling. Suggested improvements include:

1. **Network Failure Resilience**:
    - Implement retry logic and timeouts for API requests.

2. **Configuration Validation**:
    - Validate presence and format of required configuration entries (e.g., API token, dates).

3. **Data Processing Exceptions**:
    - Ensure time entry data is valid before processing/exporting.
    - Wrap critical sections (e.g., API calls, file I/O) in `try-catch` blocks.

4. **Deserialization Failures**:
    - Validate Redmine API responses before deserialization to handle format changes.

## Dependencies

- **Microsoft.Extensions.Configuration** – For reading configuration from JSON files.
- **RedmineTimeEntryExporter.Models** – Contains model classes for deserialized API data.
- **ClosedXML** – Used to generate and manipulate Excel files.
- **System.Net.Http** – For performing HTTP requests to the Redmine API.

## Example `appsettings.json`

```json
{
  "Redmine": {
    "Url": "https://your-redmine-instance.com",
    "ApiToken": "your-api-token",
    "UserId": "123",
    "InitialDate": "2025-01-01",
    "FinalDate": "2025-12-31"
  }
}
```

## License

This project is licensed under the **Creative Commons Attribution-NonCommercial 4.0 International License (CC BY-NC 4.0)**.  
You may use, share, and adapt it for personal and non-commercial purposes.  
See the [LICENSE](./LICENSE) file for more details.
