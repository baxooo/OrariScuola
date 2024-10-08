using Ical.Net;
using Ical.Net.Serialization;
using OrariScuola.Models;
using System.Text.Json;

namespace OrariScuola;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("0 Same - 1 Try New");
        var input = Console.ReadLine();
        int choice = 0;

        if(!Int32.TryParse(input, out choice))
        {
            Console.WriteLine("incorrect input, closing app");
            Environment.Exit(0);
        }

        string pathCalendario = Directory.GetCurrentDirectory() + "/calendario.ics";

        if (choice == 0)
        {
            if (!File.Exists(pathCalendario))
            {
                Console.WriteLine("Calendar File does not exist, next time press 1 to make a new one\nClosing app");
                Environment.Exit(0);
            }

            var updatedCalendar = CalendarGenerator.UpdateCalendar(pathCalendario);

            SerializeCalendar(updatedCalendar, pathCalendario);

            await SendMail(pathCalendario);

        }
        else
        {
            var fileInfo = await PdfDownloader.GetFileInfo();

            var imagPath = PdfReader.GetImageFromPdf(fileInfo.Url);

            var savedColors = ImageReader.GetColorsFromImage(imagPath.Result);

            var days = WeekGenerator.GetDaysFromColors(savedColors, fileInfo.StartDate);

            var weekCalendar = CalendarGenerator.GenerateCalendar(days);

            SerializeCalendar(weekCalendar, pathCalendario);

            await SendMail(pathCalendario);
        }
    }

    private static void SerializeCalendar(Calendar calendar, string pathCalendario)
    {
        var calSerializer = new CalendarSerializer();

        string result = calSerializer.SerializeToString(calendar);

        File.WriteAllText(pathCalendario, result);
    }

    private static async Task SendMail(string pathCalendario)
    {
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
