﻿using System.Drawing;
using OrariScuola.Service.Models;
using OrariScuola.Service.Enums;

namespace OrariScuola.Service;

internal static class WeekGenerator
{
    private readonly static List<Day> _days = [];
    private static DateTime _date;

    public static List<Day> GetDays(List<Color> colors, DateTime startDate)
    {
        _days.Clear();
        _date = startDate;
        Day day = new();
        int current;

        for (int i = 0; i < colors.Count; i++)
        {
            current = i % 5;

            day.Hours[current] = GetHourSubject(colors[i]);

            if (current == 4)
            {
                SetDayName(day);
                _days.Add(day);
                day = new();
            }
        }

        return _days;
    }
    public static List<Day> GetDays(List<string> sections, DateTime startDate)
    {
        _days.Clear();
        _date = startDate;
        Day day = new();
        int current;

        for (int i = 0; i < sections.Count; i++)
        {
            Console.WriteLine(sections[i]);
             
            current = i % 5;

            day.Hours[current] = sections[i] == "\n" ? string.Empty : sections[i];

            if (current == 4)
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
            "ffffff80" => "Matematica",
            "ff807ffe" => "Lab. fisica",
            "ffffa0fe" => "Lab. Disegno",
            "fffec1c0" => "Disegno",
            "ffc0c000" => "Fisica",
            "ffffc080" => "Lab. Chimica",
            "ffa08fff" => "SISEL",
            "ffa0ffa1" => "Lab. SISEL",
            "ff7ffffe" => "Storia",
            "ffb1b0fe" => "Lab. ELETT.",
            "ff00ff01" => "Lab. TPSEL",
            "fffe4040" => "Elettronica",
            "ffc0c080" => "TPSEL",
            "ffffff00" => "TLC",
            "ffbf00c0" => "Lab. TPSIT",
            "ffff00fe" => "Lab. TLC",
            "ffc0e0df" => "Lab. INFO",
            "ff81ff81" => "SISIT",
            "ffff8060" => "Informatica",
            "ffff0080" => "Lab. SISIT",
            "ff0080e1" => "TPSIT",
            "ff0000fe" => "Lab. GPO",
            "fffe8081" => "GPO",
            "ffd0c0ff" => "STA",
            "ff2090fe" => "Diritto",
            _ => string.Empty,
        };
    }
}



