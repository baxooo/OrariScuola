using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;
using System.Drawing;
using System.Drawing.Imaging;

namespace OrariScuola.Service;

internal static class PdfReader
{
    /// <summary>
    /// Extracts the first image from the specified PDF file, crops it to a defined rectangle, and saves it as a JPEG file.
    /// </summary>
    /// <param name="path">The file path of the PDF from which to extract the image.</param>
    /// <returns>
    /// The file path of the cropped image saved as a JPEG.
    /// </returns>
    public static async Task<string> GetImageFromPdf(string? path)
    {
        using PdfDocument document = PdfDocument.Open(path);

        Page page = document.GetPage(1);

        List<IPdfImage> images = page.GetImages().ToList();

        IPdfImage image = images[0];

        Rectangle rectangle = new(38, 103, 610, 1022);

        Console.WriteLine(image);
        byte[] bitmap = CropImage(image.RawBytes.ToArray(), rectangle);
        string exportPath = Directory.GetCurrentDirectory() + $"\\image-test.jpg";
        await File.WriteAllBytesAsync(exportPath, bitmap);

        return exportPath;
    }

    private static byte[] CropImage(byte[] image, Rectangle cropRectangle)
    {
        using MemoryStream sourceStream = new(image);

#pragma warning disable CA1416
        Bitmap? sourceImage = Image.FromStream(sourceStream) as Bitmap;

        using MemoryStream targetStream = new();

        using var targetImage = new Bitmap(cropRectangle.Width, cropRectangle.Height);

        using Graphics g = Graphics.FromImage(targetImage);

        g.DrawImage(sourceImage!, new Rectangle(0, 0, targetImage.Width, targetImage.Height), cropRectangle, GraphicsUnit.Pixel);

        targetImage.Save(targetStream, ImageFormat.Png);

        return targetStream.ToArray();
    }

}