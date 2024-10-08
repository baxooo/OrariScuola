using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using OrariScuola.Models;
using System.Text.Json;

namespace OrariScuola;

internal static class CalendarGenerator
{
    /// <summary>
    /// Generates a calendar populated with events based on the provided list of days.
    /// For each day, a set of calendar events is created and added to the calendar.
    /// The event generation logic varies based on the day's name.
    /// </summary>
    /// <param name="days">An enumerable collection of <see cref="Day"/> objects used to generate calendar events.</param>
    /// <returns>A <see cref="Calendar"/> object containing the generated calendar events.</returns>
    public static Calendar GenerateCalendar(IEnumerable<Day> days)
    {
        var calendar = new Calendar();

        foreach (Day day in days)
        {
            IEnumerable<CalendarEvent> result;

            if(day.Name == "LUNEDI"|| day.Name == "VENERDI")
                result = GenerateCalendarEvent(day, false); 
            else
                result = GenerateCalendarEvent(day, true);

            foreach (var calendarEvent in result)
                calendar.Events.Add(calendarEvent);
        }

        return calendar;
    }

    public static Calendar UpdateCalendar(string calendarPath)
    {
        using StreamReader sr = new(calendarPath);
        string content = sr.ReadToEnd();

        Calendar calendar = Calendar.Load(content);

        var events = calendar.Events;
        Calendar newCal = new();

        foreach(var e in events)
        {
            e.Start = new CalDateTime(e.Start.AddDays(7));

            newCal.Events.Add(e);
        }

        return newCal;
    }


    private static IEnumerable<CalendarEvent> GenerateCalendarEvent(Day day, bool isLongDay)
    {
        IEnumerable<CalendarEvent> events = [];
        CalendarEvent calendarEvent;

        for (int i = 0; i < day.Hours.Length; i++)
        {
            if (day.Hours[i] == string.Empty)
                continue;
            calendarEvent = new()
            {
                Summary = day.Hours[i],
                Description = day.Hours[i],
                Start = new CalDateTime(day.Date)
            };

            if (!isLongDay || i == 1 || i == 2)
            {
                day.Date = day.Date.AddMinutes(60);
                calendarEvent.End = new CalDateTime(day.Date);
            }
            else
            {
                day.Date = day.Date.AddMinutes(50);
                calendarEvent.End = new CalDateTime(day.Date);
            }

            events = events.Append(calendarEvent);
        }
        
        return events;
    } 


}
