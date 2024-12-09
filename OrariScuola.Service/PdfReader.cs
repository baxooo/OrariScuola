using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;
using System.Drawing;
using System.Drawing.Imaging;
using OrariScuola.Service.Enums;

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
    public static async Task<string> GetImageFromPdf(string? path,SectionsEnum section)
    {
        using PdfDocument document = PdfDocument.Open(path);

        Page page;

        if (IsFirstPageSection(section))
            page = document.GetPage(1);
        else page = document.GetPage(2);


        List<IPdfImage> images = page.GetImages().ToList();

        IPdfImage image = images[0];

        //206px

        Rectangle rectangle = new(444 + SectionAdjustment(section), 182, 40, 947);

        Console.WriteLine(image);
        byte[] bitmap = CropImage(image.RawBytes.ToArray(), rectangle);
        string exportPath = Directory.GetCurrentDirectory() + $"\\image-test.jpg";
        await File.WriteAllBytesAsync(exportPath, bitmap);

        return exportPath;
    }

    private static int SectionAdjustment(SectionsEnum section)
    {
        if (IsFirstPageSection(section))
            return 206 * (int)section;
        else return 206 * ((int)section - 6);
    }

    private static bool IsFirstPageSection(SectionsEnum section)
    {
        return section switch
        {
            SectionsEnum.A1 or
            SectionsEnum.A2 or
            SectionsEnum.A3 or
            SectionsEnum.N3 or
            SectionsEnum.O3 or
            SectionsEnum.A4 => true,
            _ => false
        };
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