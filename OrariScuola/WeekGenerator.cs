using System.Drawing;

namespace OrariScuola;

internal class WeekGenerator
{
    private List<Day> _days = [];

    public List<Day> GetDaysFromColors(List<Color> colors)
    {
        Day day = new();
        for(int i= 0; i <= colors.Count; i++)
        {
            int current = i % 6;
            if (current == 0 && i != 0)
            {
                SetDayName(day);
                _days.Add(day);
                day = new();
                if (i == 30)
                    break;
            }
            
#pragma warning disable CS8601
            day.Hours[current] = GetHourSubject(colors[i]);
#pragma warning restore CS8601

        }

        return _days;
    }

    private void SetDayName(Day day)
    {
        switch (_days.Count)
        {
            case 0:
                day.Name = Days.LUNEDI.ToString(); 
                break;
            case 1:
                day.Name = Days.MARTEDI.ToString();
                break;
            case 2:
                day.Name = Days.MERCOLEDI.ToString();
                break;
            case 3:
                day.Name = Days.GIOVEDI.ToString();
                break;
            case 4:
                day.Name = Days.VENERDI.ToString();
                break;
            default:
                break;
        }
    }

    private static string? GetHourSubject(Color color)
    {
        return color.Name switch
        {
            "ff2020fe" or "ffffff9f" or "ffff9f00" => "TI",
            "ffff70fe" => "Italiano",
            "ffc1ffc0" => "Inglese",
            "ffc0e07f" => "Chimica",
            "ff00a0a0" => "Religione",
            _ => null,
        };
    }
}

internal enum Days
{
    LUNEDI,
    MARTEDI,
    MERCOLEDI,
    GIOVEDI,
    VENERDI
}
