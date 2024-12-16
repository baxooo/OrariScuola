using System.Drawing;
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
        string trainedDataPath = Directory.GetCurrentDirectory() + "\\TrainedData";
        using var ocrEngine = new TesseractEngine(trainedDataPath, "eng",EngineMode.TesseractOnly);

        List<string> risultati = [];

        using Bitmap image = new(path);

        Bitmap bwImage = ConvertToGrayscale(image);

        for (int i = 0; i < 10; i++)
        {
            Rectangle rectangle = new(59 * i, 0, 59, 64);
            byte[] bitmap = CropImage(bwImage, rectangle);

            using (var img = Pix.LoadFromMemory(bitmap))
            using (var page = ocrEngine.Process(img))
            {
                string resultText = page.GetText();
                if (string.IsNullOrEmpty(resultText))
                    risultati.Add("  \n");
                else risultati.Add(resultText);


                string exportPath = Directory.GetCurrentDirectory() + $"\\testImgs\\{i}.jpg";
                File.WriteAllBytes(exportPath, bitmap);
            }
        }

        return risultati;
    }

    private static Bitmap ConvertToGrayscale(Bitmap original)
    {
        Bitmap grayBitmap = new Bitmap(original.Width, original.Height);

        for (int x = 0; x < original.Width; x++)
        {
            for (int y = 0; y < original.Height; y++)
            {
                Color pixelColor = original.GetPixel(x, y);

                int grayScale = (int)((pixelColor.R * 0.3) + (pixelColor.G * 0.59) + (pixelColor.B * 0.11));
                Color grayColor = Color.FromArgb(grayScale, grayScale, grayScale);

                grayBitmap.SetPixel(x, y, grayColor);
            }
        }

        return grayBitmap;
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
