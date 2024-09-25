namespace OrariScuola;

internal class Program
{
    static async Task Main(string[] args)
    {
        await PdfDownloader.GetFile();
    }
}
