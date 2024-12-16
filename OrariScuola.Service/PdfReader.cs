using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;
using System.Drawing;
using System.Drawing.Imaging;
using OrariScuola.Service.Enums;

namespace OrariScuola.Service;

internal static class PdfReader
{
    /// <summary>
    /// Extracts the image from the specified PDF file,based on the section, crops it to a defined rectangle, and saves it as a JPEG file.
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

        Rectangle rectangle = new(444 + StudentSectionAdjustment(section), 182, 40, 947);

        Console.WriteLine(image);
        byte[] bitmap = ImageReader.CropImage(image.RawBytes.ToArray(), rectangle);
        string exportPath = Directory.GetCurrentDirectory() + $"\\image-test.jpg";
        await File.WriteAllBytesAsync(exportPath, bitmap);

        return exportPath;
    }

    /// <summary>
    /// Extracts the image from the specified PDF file,based on the section, crops it to a defined rectangle, and saves it as a JPEG file.
    /// </summary>
    /// <param name="path">The file path of the PDF from which to extract the image.</param>
    /// <returns>
    /// The file path of the cropped image saved as a JPEG.
    /// </returns>
    public static async Task<string> GetImageFromPdf(string? path, ProfessorsEnum prof)
    {
        using PdfDocument document = PdfDocument.Open(path);

        Page page;

        if (IsFirstPageSection(prof))
            page = document.GetPage(1);
        else page = document.GetPage(2);


        List<IPdfImage> images = page.GetImages().ToList();

        IPdfImage image = images[0];

        //await File.WriteAllBytesAsync(Directory.GetCurrentDirectory()+ "\\docenti.jpg", [.. image.RawBytes]);

        Rectangle rectangle = new(177, ProfessorSectionAdjustment(prof) + 231, 1474, 64); 

        Console.WriteLine(image);
        byte[] bitmap = ImageReader.CropImage(image.RawBytes.ToArray(), rectangle);
        string exportPath = Directory.GetCurrentDirectory() + $"\\image-test.jpg";
        await File.WriteAllBytesAsync(exportPath, bitmap);

        return exportPath;
    }

    private static int StudentSectionAdjustment(SectionsEnum section)
    {
        if (IsFirstPageSection(section))
            return 206 * (int)section;
        else return 206 * ((int)section - 6);
    }
    private static int ProfessorSectionAdjustment(ProfessorsEnum prof)
    {
        if (IsFirstPageSection(prof))
            return 64 * (int)prof;
        else return 64 * ((int)prof - 6);
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
    private static bool IsFirstPageSection(ProfessorsEnum prof)
    {
        return prof switch
        {
           ProfessorsEnum.ANNUNZIATA => true,
            _ => false
        };
    }

}