﻿using System.Globalization;

namespace OrariScuola;
//questa classe si occuperà di scaricare il pdf
internal static class PdfDownloader
{
    static string _url = string.Empty;
    static DateTime _date = DateTime.Now;
    static string _giorno = string.Empty;

    private static void GenerateUrl()
    {
        int currentDayOfTheWeek = (int)_date.DayOfWeek;
        int monday;

        //if it is Sunday or Saturday, most likely the schedule has been updated so i can add 1 or 2 days,
        //otherwise i just remove the days untill monday
        if (currentDayOfTheWeek == 6)
        {
            _date = _date.AddDays(1);
            monday = _date.Day;
        }
        else if (currentDayOfTheWeek == 0)
        {
            _date = _date.AddDays(2);
            monday = _date.Day;
        }
        else
        {
            _date = _date.AddDays(-(currentDayOfTheWeek - 1));
            monday = _date.Day;
        }
    
        _giorno = $"{ monday} -{ _date.ToString("MMMM", new CultureInfo("it_IT"))}";

        _url = "https://itisfermiserale.wordpress.com/wp-content/uploads/" + _date.Year.ToString() + "/" +
            DateTime.Now.Month.ToString("00") + 
            $"/orario_provvisorio_dal-{monday}-{_date.ToString("MMMM", new CultureInfo("it_IT"))}_classi.pdf"; //this last part needs to be replaced sooner or later
    }

    public static async Task GetFile()
    {
        GenerateUrl();

        using (HttpClient client = new HttpClient())
        {
            try
            {
                byte[] fileBytes = await client.GetByteArrayAsync(_url);

                string path = Directory.GetCurrentDirectory() + $"\\orario-dal-{_giorno}.pdf";

                await File.WriteAllBytesAsync(path, fileBytes);

                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine("File downloaded successfully");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to download the file");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine(ex.Message);
            }
        }
    }

}
