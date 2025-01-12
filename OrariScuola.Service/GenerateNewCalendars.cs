using Ical.Net;
using Ical.Net.Serialization;
using OrariScuola.Service.Enums;
using OrariScuola.Service.Models;
using System.Text.Json;
using static System.Collections.Specialized.BitVector32;

namespace OrariScuola.Service;

public static class GenerateNewCalendars
{
    private static readonly string _urlInfoFilePath = Directory.GetCurrentDirectory() + "\\urlInfo.txt";
    private static readonly string _urlProfInfoFilePath = Directory.GetCurrentDirectory() + "\\profUrlInfo.txt";
    private static readonly string[] _separator = ["\n"];

    /// <summary>
    /// Generates a new calendar.
    /// </summary>
    /// <param name="mail"></param>
    /// <param name="section"></param>
    /// <returns>A string rappresenting the calendar file path.</returns>
    public static async Task<string> GenerateStudent(string? mail, SectionsEnum section)
    {
        DateTime monday = PdfDownloader.GetCurrentMonday();

        string pathCalendario = Directory.GetCurrentDirectory() + "\\calendario" + "_" + section.ToString() ;
        pathCalendario += "_" + monday.ToString("dd-MMMM") + ".ics";

        if (File.Exists(pathCalendario))
        {
            //if the the file for the section with date exist, there's no need to do any more operation and it is possible to send it
            if (!string.IsNullOrEmpty(mail))
                await SendMail(pathCalendario, mail);

            return pathCalendario;
        }

        string pdfFilePath = await PdfDownloader.GetFile(true);

        var imagPath = await PdfReader.GetImageFromPdf(pdfFilePath, section);

        var savedColors = ImageReader.GetColorsFromImage(imagPath);

        var days = WeekGenerator.GetDays(savedColors, monday);

        var weekCalendar = CalendarGenerator.GenerateCalendar(days);

        SerializeCalendar(weekCalendar, pathCalendario);

        if (!string.IsNullOrEmpty(mail))
            await SendMail(pathCalendario, mail);

        return pathCalendario;
    }

    public static async Task<string> GenerateProfessor(string? mail, ProfessorsEnum prof)
    {
        DateTime monday = PdfDownloader.GetCurrentMonday();

        string pathCalendario = Directory.GetCurrentDirectory() + "\\calendario" + "_" + prof.ToString();
        pathCalendario += "_" + monday.ToString("dd-MMMM") + ".ics";

        if (File.Exists(pathCalendario))
        {
            //if the the file for the section with date exist, there's no need to do any more operation and it is possible to send it
            if (!string.IsNullOrEmpty(mail))
                await SendMail(pathCalendario, mail);

            return pathCalendario;
        }

        string pdfFilePath = await PdfDownloader.GetFile(false);

        var imagPath = await PdfReader.GetImageFromPdf(pdfFilePath, prof);

        var savedColors = ImageReader.ReadSectionsFromImage(imagPath);

        var days = WeekGenerator.GetDays(savedColors, monday);

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