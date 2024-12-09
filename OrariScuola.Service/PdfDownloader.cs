using System.Globalization;

namespace OrariScuola.Service;

internal static class PdfDownloader
{
    private static string GetDate()
    {
        var date = GetCurrentMonday();

        return $"{date.Day}-{date.ToString("MMMM", new CultureInfo("it_IT"))}";
    }

    public static DateTime GetCurrentMonday()
    {
        DateTime date = DateTime.Now.Date;
        int currentDayOfTheWeek = (int)date.DayOfWeek;

        //if it is Sunday or Saturday, most likely the schedule has been updated so i can add 1 or 2 days,
        //otherwise i just remove the days untill monday

        date = currentDayOfTheWeek switch
        {
            6 => date.AddDays(1),
            0 => date.AddDays(2),
            _ => date.AddDays(-(currentDayOfTheWeek - 1)),
        };
        TimeOnly timeSpan = new(17, 00, 00);
        DateOnly dateOnly = new(date.Date.Year, date.Date.Month, date.Date.Day);
        return new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day, timeSpan.Hour, timeSpan.Minute, timeSpan.Second);
    }

    public static async Task<string> GetUrl()
    {
        string site = "https://itisfermiserale.wordpress.com/";

        string target = "https://itisfermiserale.wordpress.com/wp-content/uploads/";

        using HttpClient client = new();
        try
        {
            var html = await client.GetStringAsync(site);
            string[] list = html.Split(new string[] { "\n" }, StringSplitOptions.None);

            string[] urls = list.Where(s => s.Contains(target)).ToArray();
            string url = urls.Where(s => s.Contains("orari")).First();

            int index1 = url.IndexOf("\"https");
            url = url.Remove(0, index1 +1);
            int index2 = url.IndexOf("\"");
            url = url.Remove(index2, url.Length - index2);

            return url;
        }
        catch (Exception ex)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Environment.Exit(0);
            return "";
        }
    }

    /// <summary>
    /// Downloads a PDF file from the school site, saves it to the current directory, and returns the start date of the current monday.
    /// </summary>
    /// <returns>
    /// A <see cref="string"/> containing the start date of the week of the downloaded PDF school schedule.
    /// </returns>
    public static async Task<string> GetFile()
    {
        string url = await GetUrl();

        using HttpClient client = new();
        try
        {
            byte[] fileBytes = await client.GetByteArrayAsync(url);

            string path = Directory.GetCurrentDirectory() + $"\\orario-dal-{GetDate()}.pdf";

            await File.WriteAllBytesAsync(path, fileBytes);

            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("File downloaded successfully");
            Console.BackgroundColor = ConsoleColor.Black;

            return path;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Failed to download the file");
            Console.WriteLine("Closing Application");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(ex.Message);

            Environment.Exit(0);
            return null;
        }
    }
}


