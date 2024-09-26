namespace OrariScuola;

internal class Program
{
    static async Task Main(string[] args)
    {
        var filePath = await PdfDownloader.GetFile();

        var imagPath = PdfReader.GetImageFromPdf(filePath);

        var savedColors = ImageReader.GetColorsFromImage(imagPath);

        WeekGenerator weekGenerator = new();

        var days = weekGenerator.GetDaysFromColors(savedColors);

        foreach (var day in days) 
            Console.WriteLine(day.ToString());
    }

}
