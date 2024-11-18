using Ical.Net;
using Ical.Net.Serialization;
using OrariScuola.Service.Enums;
using OrariScuola.Service.Models;
using System;
using System.Text.Json;

namespace OrariScuola.Service;

public static class GenerateNewCalendars
{
    private static readonly string _urlInfoFilePath = Directory.GetCurrentDirectory() + "\\urlInfo.txt";
    private static readonly string _pathCalendario = Directory.GetCurrentDirectory() + "\\calendario.ics";

    public static async Task<string> Generate(bool sendMail, SectionsEnum section)
    {
        string url = await PdfDownloader.GetUrl();

        if (!File.Exists(_urlInfoFilePath))
            File.Create(_urlInfoFilePath).Close();

        string urlFileData = File.ReadAllText(_urlInfoFilePath);
        string[] fileUrl = urlFileData.Split(new string[] { "\n" }, StringSplitOptions.None);
        DateTime monday = PdfDownloader.GetCurrentMonday();

        if (!string.IsNullOrEmpty(urlFileData) && fileUrl[0] == url && fileUrl[1] == monday.ToString())
        {
            //if the url is the same the file has already been downloaded so it's ready to be sended
            //but it's important to check if the calendar week is the current week too
            if (sendMail)
                await SendMail(_pathCalendario);

            return _pathCalendario;
        }
        else
        {
            File.WriteAllText(_urlInfoFilePath, url + "\n" + monday.ToString());

            string pdfFilePath = await PdfDownloader.GetFile();

            var imagPath = await PdfReader.GetImageFromPdf(pdfFilePath);

            var savedColors = ImageReader.GetColorsFromImage(imagPath);

            var days = WeekGenerator.GetDaysFromColors(savedColors, monday);

            var weekCalendar = CalendarGenerator.GenerateCalendar(days);

            SerializeCalendar(weekCalendar, _pathCalendario);

            if (sendMail)
                await SendMail(_pathCalendario);

            return _pathCalendario;
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