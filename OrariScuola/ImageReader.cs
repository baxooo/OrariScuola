using System.Drawing;

namespace OrariScuola;
#pragma warning disable CA1416

internal static class ImageReader
{
    // i'm not going to use any kind of image recognition, it's way simpler to achieve what i need,
    // using the schema my school uses wich is color based. I'm also going to move that way.


    public static List<Color> GetColorsFromImage(string path)
    {
        int x = 440, y = 100;
        using Bitmap bitmap = new(path);
        List<Color> colors = [];

        for (int i = 0; i < 30; i++)
        {
            Color pixelColor = bitmap.GetPixel(x, y);
            bitmap.SetPixel(x, y, Color.Red); //to be commented out, here for debugging purposes
            colors.Add(pixelColor);
            y += 31;
        }

        foreach (var color in colors)
        {
            Console.WriteLine(color + " " + color.Name);
        }

        bitmap.Save(path + ".png"); //same as line 21

        return colors;
    }

}
