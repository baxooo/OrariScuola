using Ical.Net.CalendarComponents;
using Ical.Net.Serialization;
using System.Text;

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

        File.WriteAllText(Directory.GetCurrentDirectory() + "/calendario.ics", result);

        Console.WriteLine("aaa");
    }

}
