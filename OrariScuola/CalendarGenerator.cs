using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using OrariScuola.Models;

namespace OrariScuola;

internal static class CalendarGenerator
{
    //as of now i can't generate it programmatically, since there aren't enough teachers to distinguish
    //wich day is a long day and wich day is a short day, but I know that Mon and Fry are always short
    //and the rest long
    public static Calendar GenerateCalendar(List<Day> days)
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
            {
                calendar.Events.Add(calendarEvent);
            }
        }

        return calendar;
    }

    //long = 50m/1h/1h/50m/50m 
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
