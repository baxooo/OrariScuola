using System.Drawing;

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
}
