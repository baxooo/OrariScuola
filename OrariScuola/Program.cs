namespace OrariScuola;

internal class Program
{
    static async Task Main(string[] args)
    {
        var filePath = await PdfDownloader.GetFile();

        var imagPath = PdfReader.GetImageFromPdf(filePath);

        /*var savedText = */ImageReader.GetTextFromImage(imagPath);
    }

}
