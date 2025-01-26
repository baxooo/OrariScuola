using System.Drawing;
using OpenCvSharp;
using Tesseract;

namespace OrariScuola.Service;

#pragma warning disable CA1416
internal static class ImageReader
{
    public static List<Color> GetColorsFromImage(string path)
    {
        int x = 20, y = 20;
        using Bitmap bitmap = new(path);
        List<Color> colors = [];

        for (int i = 0; i < 25; i++)
        {
            Color pixelColor = bitmap.GetPixel(x, y);
            //bitmap.SetPixel(x, y, Color.Red); //to be commented out, here for debugging purposes
            colors.Add(pixelColor);
            y += 38;
        }

        //foreach (var color in colors)
        //{
        //    Console.WriteLine(color + " " + color.Name);
        //}

        //bitmap.Save(path + ".png"); //same as line 20

        return colors;
    }

    public static List<string> ReadSectionsFromImage(string path)
    {

        List<string> risultati = [];

        var saveFilesDir = MakeStartingCroppedImages(path);

        var images = Directory.GetFiles(saveFilesDir, "*.jpg");
        
        foreach (var imgPath in images)
        {
            if (IsBlank(imgPath))
            {
                risultati.Add("\n");
                continue;
            }

            if (IsDispo(imgPath))
            {
                risultati.Add("DISPONIBILE");
                continue;
            }
            
            var img = ImageProcessUtils.RepeatCharsInSameLine(imgPath);
            Mat preprocessedImage = new Mat(img, ImreadModes.Color);
            
            string extractedText = PerformOCR(preprocessedImage);

            extractedText = extractedText.Replace(" ", "");
            
            //If null let's first try grayscale.
            if (string.IsNullOrEmpty(extractedText))
            {
                preprocessedImage = new Mat(img, ImreadModes.Grayscale);
                extractedText = PerformOCR(preprocessedImage);
            }
        
            // Then if grayscale does not work, a convertion to negative should fix any issues,
            // Tesseract really does not like white text, and by my tests is what creates mayhem.
            // Like in this case we know that E does not exist in data set, this means it isn't reading properly
            // due to the text being white, as soon as the text color gets inverted the problem vanishes
            if (extractedText.Contains("E"))
            {
                img = ImageProcessUtils.ConvertToNegative(img);
                preprocessedImage = new Mat(img, ImreadModes.Grayscale);
                extractedText = PerformOCR(preprocessedImage);
            }

            extractedText = ImageProcessUtils.PostProcess(extractedText);
            risultati.Add(extractedText);
            
            var name = Path.GetFileNameWithoutExtension(imgPath);
            Console.WriteLine($"Testo Estratto da {name}: {extractedText}");
            Console.WriteLine();
            
        }
        
        return risultati;
    }

    private static bool IsBlank(string path)
    {
        using Bitmap image = new Bitmap(path);

        Color color = image.GetPixel(3, 3);

        return color.Name switch
        {
            "ffffffff" or "fffffffd" => true,
            _ => false
        };
    }
    
    private static bool IsDispo(string path)
    {
        using Bitmap image = new Bitmap(path);

        Color color = image.GetPixel(3, 3);

        Console.WriteLine(color.Name);

        return color.Name switch
        {
            "ff8e01ff" or "ff9000ff" => true,
            _ => false
        };
    }

    private static string MakeStartingCroppedImages(string path)
    {
        using Bitmap image = new(path);

        string saveFilesDir = Directory.GetCurrentDirectory() + $"\\testImgs";
        
        for (int i = 0; i < 25; i++)
        {
            Rectangle rectangle = new(59 * i, 0, 59 , image.Height);
            byte[] bitmap = CropImage(image, rectangle);
            
            string exportPath = saveFilesDir + $"\\{i:000}.jpg";
                
            File.WriteAllBytes(exportPath, bitmap);
        }

        return saveFilesDir;
    }

    private static string PerformOCR(Mat preprocessedImage)
    {
        byte[] imageBytes = preprocessedImage.ImEncode(".png");
        
        using var engine = new TesseractEngine( Directory.GetCurrentDirectory() + "\\TrainedData", "eng", EngineMode.TesseractAndLstm);
        using var img = Pix.LoadFromMemory(imageBytes);
        using var page = engine.Process(img);

        return page.GetText();
    }

    public static byte[] CropImage(byte[] image, Rectangle cropRectangle)
    {
        using MemoryStream sourceStream = new(image);

#pragma warning disable CA1416
        Bitmap? sourceImage = Image.FromStream(sourceStream) as Bitmap;

        using MemoryStream targetStream = new();

        using var targetImage = new Bitmap(cropRectangle.Width, cropRectangle.Height);

        using Graphics g = Graphics.FromImage(targetImage);

        g.DrawImage(sourceImage!, new Rectangle(0, 0, targetImage.Width, targetImage.Height), cropRectangle, GraphicsUnit.Pixel);

        targetImage.Save(targetStream, System.Drawing.Imaging.ImageFormat.Png);

        return targetStream.ToArray();
    }

    private static byte[] CropImage(Bitmap sourceImage, Rectangle cropRectangle)
    {
        using MemoryStream targetStream = new();

        using var targetImage = new Bitmap(cropRectangle.Width, cropRectangle.Height);

        using Graphics g = Graphics.FromImage(targetImage);

        g.DrawImage(sourceImage!, new Rectangle(0, 0, targetImage.Width, targetImage.Height), cropRectangle, GraphicsUnit.Pixel);

        targetImage.Save(targetStream, System.Drawing.Imaging.ImageFormat.Png);

        return targetStream.ToArray();
    }
}
