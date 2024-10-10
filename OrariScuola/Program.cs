using Ical.Net;
using Ical.Net.Serialization;
using OrariScuola.Models;
using System.Text.Json;

namespace OrariScuola;

internal class Program
{
    static async Task Main(string[] args)
    {
        string urlFilePath = Directory.GetCurrentDirectory() + "\\url.txt";

        string url = await PdfDownloader.GetUrl();
        
        if(!File.Exists(urlFilePath))
            File.Create(urlFilePath).Close();

        string urlFileData = File.ReadAllText(urlFilePath);
        string[] fileUrl = urlFileData.Split(new string[] { "\n" }, StringSplitOptions.None);
        string pathCalendario = Directory.GetCurrentDirectory() + "\\calendario.ics";
        DateTime monday = PdfDownloader.GetCurrentMonday();

        if (!string.IsNullOrEmpty(urlFileData) && fileUrl[0] == url && fileUrl[1] == monday.ToString())
        {
            //if the url is the same the file has already been downloaded so it's ready to be sended
            //but it's important to check if the calendar week is the current week too
            await SendMail(pathCalendario);
        }
        else
        {
            File.WriteAllText(urlFilePath, url + "\n" + monday.ToString());

            string pdfFilePath = await PdfDownloader.GetFile();

            var imagPath = await PdfReader.GetImageFromPdf(pdfFilePath);

            var savedColors = ImageReader.GetColorsFromImage(imagPath);

            var days = WeekGenerator.GetDaysFromColors(savedColors, monday);

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
