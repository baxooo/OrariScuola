using System.Drawing;
using OrariScuola.Models;

namespace OrariScuola;

internal static class WeekGenerator
{
    private static List<Day> _days = [];
    private static DateTime _date;

    public static List<Day> GetDaysFromColors(List<Color> colors, DateTime startDate)
    {
        _date = startDate;
        Day day = new();
        int current;

        for (int i = 0; i < colors.Count; i++)
        {
            current = i % 6;

            day.Hours[current] = GetHourSubject(colors[i]);

            if (current == 5)
            {
                SetDayName(day);
                _days.Add(day);
                day = new();
            }
        }

        return _days;
    }

    private static void SetDayName(Day day)
    {
        switch (_days.Count)
        {
            case 0:
                day.Name = DaysEnum.LUNEDI.ToString(); 
                day.Date = _date;
                break;
            case 1:
                day.Name = DaysEnum.MARTEDI.ToString();
                day.Date = _date.AddDays(1);
                break;
            case 2:
                day.Name = DaysEnum.MERCOLEDI.ToString();
                day.Date = _date.AddDays(2);
                break;
            case 3:
                day.Name = DaysEnum.GIOVEDI.ToString();
                day.Date = _date.AddDays(3);
                break;
            case 4:
                day.Name = DaysEnum.VENERDI.ToString();
                day.Date = _date.AddDays(4);
                break;
            default:
                break;
        }
    }

    private static string GetHourSubject(Color color) // to be updated
    {
        return color.Name switch
        {
            "ff2020fe" or "ffffff9f" or "ffff9f00" => "TI",
            "ffff70fe" => "Italiano",
            "ffc1ffc0" => "Inglese",
            "ffc0e07f" => "Chimica",
            "ff00a0a0" => "Religione",
            _ => string.Empty,
        };
    }
}


