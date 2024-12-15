using System.Drawing;
using FluentAssertions;
using OrariScuola.Service;

namespace OrariScuola.Tests.ImageReaderTests;



public class ImageReaderTests
{
    [Fact]
    public void ImageReader_GetColorsFromImage_ReturnListOfColors()
    {
        string path = Directory.GetCurrentDirectory() + "\\image-test-A1.jpg";

        List<Color> expectedColors = [Color.FromArgb(255, 254, 193, 192), 
                                      Color.FromArgb(255, 254, 193, 192), 
                                      Color.FromArgb(255, 255, 255, 159), 
                                      Color.FromArgb(255, 255, 255, 128), 
                                      Color.FromArgb(255, 255, 255, 255), 
                                      Color.FromArgb(255, 255, 255, 128), 
                                      Color.FromArgb(255, 255, 160, 254), 
                                      Color.FromArgb(255, 192, 192, 0),
                                      Color.FromArgb(255, 192, 192, 0),
                                      Color.FromArgb(255, 255, 255, 255), 
                                      Color.FromArgb(255, 193, 255, 192), 
                                      Color.FromArgb(255, 193, 255, 192), 
                                      Color.FromArgb(255, 255, 255, 128), 
                                      Color.FromArgb(255, 255, 192, 128), 
                                      Color.FromArgb(255, 255, 255, 255), 
                                      Color.FromArgb(255, 128, 127, 254), 
                                      Color.FromArgb(255, 192, 224, 127), 
                                      Color.FromArgb(255, 255, 112, 254), 
                                      Color.FromArgb(255, 0,   160, 160), 
                                      Color.FromArgb(255, 255, 255, 255), 
                                      Color.FromArgb(255, 255, 159, 0),
                                      Color.FromArgb(255, 255, 255, 159), 
                                      Color.FromArgb(255, 255, 112, 254), 
                                      Color.FromArgb(255, 255, 112, 254), 
                                      Color.FromArgb(255, 255, 255, 255)
            ];

        var actualColors = ImageReader.GetColorsFromImage(path);
        Action action = () => ImageReader.GetColorsFromImage(path);

        actualColors.Should().NotBeNullOrEmpty();
        actualColors.Should().Equal(expectedColors);
        action.Should().NotThrow();
    }

    [Fact]
    public void ImageReader_GetColorsFromImage_ReturnsException()
    {
        string path = "C:\\PathNotFound";

        Action action = () => ImageReader.GetColorsFromImage(path);

        action.Should().Throw<Exception>();
    }
}
