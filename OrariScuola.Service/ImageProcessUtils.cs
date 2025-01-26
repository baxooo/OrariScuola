using System.Drawing;

namespace OrariScuola.Service;
#pragma warning disable CA1416
public class ImageProcessUtils
{
    private static readonly string _processedImagesDir = Directory.GetCurrentDirectory() + "\\testImgs\\processedImages";
    public static string RepeatCharsInSameLine(string path)
    {
        if (!Path.Exists(_processedImagesDir))
            Directory.CreateDirectory(_processedImagesDir);
        
        using Bitmap originalImage = new Bitmap(path);

        // Area to crop of the two chars
        Rectangle cropArea1 = new(17, 25, 11, 14);
        Rectangle cropArea2 = new(27, 25, 13, 14);

        using Bitmap croppedImage1 = originalImage.Clone(cropArea1, originalImage.PixelFormat);
        using Bitmap croppedImage2 = originalImage.Clone(cropArea2, originalImage.PixelFormat);

        // New Image Dimensions
        int outputWidth = croppedImage1.Width * 4 + croppedImage2.Width * 4 + 160; 
        int outputHeight = croppedImage1.Height + 160;

        using Bitmap outputImage = new Bitmap(outputWidth, outputHeight);
        using Graphics g = Graphics.FromImage(outputImage);
        
        //get color from image at 3px 3px
        Color color = originalImage.GetPixel(3, 3);

        g.Clear(color);

        // Repeat characters in image horizontally
        for (int i = 0; i < 4; i++)
        {
            int x = i * croppedImage1.Width  + 80;
            g.DrawImage(croppedImage1, x, 80);
        }
        for (int i = 4; i < 8; i++)
        {
            int x = i * croppedImage2.Width  + 70;
            g.DrawImage(croppedImage2, x + 1 * i, 80);
        }


        var ext = Path.GetExtension(path);
        var filePath = _processedImagesDir + "\\" +
                       Path.GetFileNameWithoutExtension(path)  +ext;

        
        outputImage.Save(filePath);


        return filePath;
    }
    
    public static string ConvertToNegative(string img)
    {
        using Bitmap originalImage = new Bitmap(img);
        
        using Bitmap negativeImage = new Bitmap(originalImage.Width, originalImage.Height);
        
        for (int y = 0; y < originalImage.Height; y++)
        {
            for (int x = 0; x < originalImage.Width; x++)
            {
                Color originalColor = originalImage.GetPixel(x, y);

                Color negativeColor = Color.FromArgb(
                    originalColor.A,
                    255 - originalColor.R,
                    255 - originalColor.G,
                    255 - originalColor.B 
                );

                negativeImage.SetPixel(x, y, negativeColor);
            }
        }

        string outputPath = img.Replace(".jpg", "x.jpg");
        negativeImage.Save(outputPath, System.Drawing.Imaging.ImageFormat.Jpeg);

        return outputPath;
    }

    public static string PostProcess(string text)
    {
        text = text.Trim();
        if (text.Contains("0"))
            text = text.Replace("0", "O");
        return $"{text[0]}{text[4]}";
    }
}