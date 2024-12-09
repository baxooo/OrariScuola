using Ical.Net;
using Ical.Net.Serialization;
using OrariScuola.Service.Enums;
using OrariScuola.Service.Models;
using System.Text.Json;

namespace OrariScuola.Service;

public static class GenerateNewCalendars
{
    private static readonly string _urlInfoFilePath = Directory.GetCurrentDirectory() + "\\urlInfo.txt";

    /// <summary>
    /// Generates a new calendar.
    /// </summary>
    /// <param name="mail"></param>
    /// <param name="section"></param>
    /// <returns>A string rappresenting the calendar file path.</returns>
    public static async Task<string> Generate(string? mail, SectionsEnum section)
    {
        string pathCalendario = Directory.GetCurrentDirectory() + "\\calendario" + "_" + section.ToString() + ".ics";

        string url = await PdfDownloader.GetUrl();

        if (!File.Exists(_urlInfoFilePath))
            File.Create(_urlInfoFilePath).Close();

        string urlFileData = File.ReadAllText(_urlInfoFilePath);
        string[] fileUrl = urlFileData.Split(new string[] { "\n" }, StringSplitOptions.None);
        DateTime monday = PdfDownloader.GetCurrentMonday();

        if (!string.IsNullOrEmpty(urlFileData) && 
            fileUrl[0] == url && 
            fileUrl[1] == monday.ToString() && 
            fileUrl.Contains(section.ToString())&&
            File.Exists(pathCalendario))
        {
            //if the url is the same the file has already been downloaded so it's ready to be sended
            //but it's important to check if the calendar week is the current week too,
            //and lastly check if a calendar for that section has been already generated
            if (!string.IsNullOrEmpty(mail))
                await SendMail(pathCalendario, mail);

            return pathCalendario;
        }

        File.WriteAllText(_urlInfoFilePath, url + "\n" + monday.ToString());

        string pdfFilePath = await PdfDownloader.GetFile();

        var imagPath = await PdfReader.GetImageFromPdf(pdfFilePath, section);

        var savedColors = ImageReader.GetColorsFromImage(imagPath);

        var days = WeekGenerator.GetDaysFromColors(savedColors, monday);

        var weekCalendar = CalendarGenerator.GenerateCalendar(days);

        SerializeCalendar(weekCalendar, pathCalendario);

        if (!string.IsNullOrEmpty(mail))
            await SendMail(pathCalendario, mail);

        return pathCalendario;
    }

    private static void SerializeCalendar(Calendar calendar, string pathCalendario)
    {
        var calSerializer = new CalendarSerializer();

        string result = calSerializer.SerializeToString(calendar);

        File.WriteAllText(pathCalendario, result);
    }

    private static async Task SendMail(string pathCalendario, string mail)
    {
        string credentialsFile = await File.ReadAllTextAsync(Directory.GetCurrentDirectory() + "/appsettings.json");

        Credentials credentials = JsonSerializer.Deserialize<Credentials>(credentialsFile)!;

        string messageBody = "Il tuo Calendario per questa settimana! " +
                             "\n\nQuesto servizio non è offerto dall'istituto e non c'è nessuna affiliazione con esso.\n" +
                             "il tuo indirizzo e-mail non viene salvato.";

        await EmailSender.SendEmailAsync(mail,
                                         credentials.FromAddress!,
                                         credentials.Password!,
                                         "School Calendar",
                                         messageBody,
                                         pathCalendario);
    }
}