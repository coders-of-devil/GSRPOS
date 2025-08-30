using Microsoft.Maui.Storage;
using System.Text;

namespace Infrastructure.ServiceClass;

public class FileLogger
{
    private readonly string _logFilePath;

    public FileLogger()
    {
        var logFolder = FileSystem.AppDataDirectory;
        _logFilePath = Path.Combine(logFolder, "app_log.txt");
    }

    public void Log(string message, string context = "")
    {
        try
        {
            var timestamp = DateTime.UtcNow.AddHours(4).ToString("yyyy-MM-dd HH:mm:ss");
            var entry = new StringBuilder();
            entry.AppendLine("--------------------------------------------------");
            entry.AppendLine($"[{timestamp}] {context}");
            entry.AppendLine(message);
            entry.AppendLine();

            File.AppendAllText(_logFilePath, entry.ToString());
        }
        catch
        {
            // Avoid recursive crash on logging failure
        }
    }

    public string GetLogPath() => _logFilePath;
}
