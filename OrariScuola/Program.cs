using Ical.Net.CalendarComponents;
using Ical.Net.Serialization;
using OrariScuola.Models;
using System.Text;
using System.Text.Json;

namespace OrariScuola;

internal class Program
{
    static async Task Main(string[] args)
    {
        var fileInfo = await PdfDownloader.GetFileInfo();

        var pdfReader = new PdfReader();

        var imagPath = pdfReader.GetImageFromPdf(fileInfo.Url);

        var savedColors = ImageReader.GetColorsFromImage(imagPath.Result);

        WeekGenerator weekGenerator = new();

        var days = weekGenerator.GetDaysFromColors(savedColors, fileInfo.StartDate);

        var weekCalendar = CalendarGenerator.GenerateCalendar(days);

        var calSerializer = new CalendarSerializer();

        string result = calSerializer.SerializeToString(weekCalendar);

        string pathCalendario = Directory.GetCurrentDirectory() + "/calendario.ics";

        File.WriteAllText(pathCalendario, result);

        string credentialsFile = await File.ReadAllTextAsync(Directory.GetCurrentDirectory() + "/appsettings.json");

        Credentials credentials = JsonSerializer.Deserialize<Credentials>(credentialsFile)!; 

        await EmailSender.SendEmailAsync(credentials.ToAddress!,
                                         credentials.FromAddress!,
                                         credentials.Password!,
                                         "School Calendar",
                                         "What up doe",
                                         pathCalendario);

    }

}
