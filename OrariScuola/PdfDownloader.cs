using System.Globalization;

namespace OrariScuola;
//questa classe si occuperà di scaricare il pdf
internal static class PdfDownloader
{
    static string _url = string.Empty;
    static DateTime _date = DateTime.Now;
    static int _currentDayOfTheWeek = (int)_date.DayOfWeek;
    static int _monday = 0;

    private static void GenerateUrl()
    {
        if (_currentDayOfTheWeek == 6)
        {
            DateTime temp = _date.AddDays(1);
            _monday = temp.Day;
        }
        else if (_currentDayOfTheWeek == 0)
        {
            DateTime temp = _date.AddDays(2);
            _monday = temp.Day;
        }
        else
        {
            DateTime temp = _date.AddDays(-(_currentDayOfTheWeek - 1));
            _monday = temp.Day;
        }
    
        _url = "https://itisfermiserale.wordpress.com/wp-content/uploads/" + _date.Year.ToString() + "/" +
            DateTime.Now.Month.ToString() + 
            $"/orario_provvisorio_dal-{_monday}-{_date.ToString("MMMM", new CultureInfo("it_IT"))}_classi.pdf"; //this last part needs to be replaced sooner or later
    }

    public static void GetFile()
    {
        GenerateUrl();
    }

}
