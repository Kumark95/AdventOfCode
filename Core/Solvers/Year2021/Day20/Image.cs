namespace AdventOfCode.Core.Solvers.Year2021.Day20;

internal class Image
{
    public char[,] PixelMatrix { get; set; }
    private int DimLength => PixelMatrix.GetLength(0);
    public const char LIGHT_PIXEL = '#';
    public const char DARK_PIXEL = '.';

    public Image(char[,] pixelMatrix)
    {
        PixelMatrix = pixelMatrix;
    }

    /// <summary>
    /// Return the number of light pixels in the image
    /// </summary>
    /// <returns></returns>
    public int CountLightPixels()
    {
        var count = 0;
        for (int x = 0; x < DimLength; x++)
        {
            for (int y = 0; y < DimLength; y++)
            {
                if (PixelMatrix[x, y] == LIGHT_PIXEL)
                {
                    count++;
                }
            }
        }

        return count;
    }

    /// <summary>
    /// Expand the image in all directions
    /// </summary>
    /// <param name="fillValue"></param>
    /// <remarks>
    /// An image of 5x5 increases to 7x7
    /// </remarks>
    /// <returns></returns>
    public Image Expand(char fillValue)
    {
        var expandedDimLength = DimLength + 2;
        var expandedImage = new char[expandedDimLength, expandedDimLength];

        for (int x = 0; x < expandedDimLength; x++)
        {
            for (int y = 0; y < expandedDimLength; y++)
            {
                var imgI = x - 1;
                var imgJ = y - 1;

                expandedImage[x, y] = AccessPixelPosition(imgI, imgJ, fillValue);
            }
        }

        return new Image(expandedImage);
    }

    /// <summary>
    /// Returns a 3x3 matrix with the neighbor pixels
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="fillValue"></param>
    /// <returns></returns>
    public char[,] FindNeighbours(int i, int j, char fillValue)
    {
        var neighbours = new char[3, 3]
        {
            { AccessPixelPosition(i - 1, j - 1, fillValue), AccessPixelPosition(i - 1, j, fillValue), AccessPixelPosition(i - 1, j + 1, fillValue) },
            { AccessPixelPosition(i    , j - 1, fillValue), AccessPixelPosition(i    , j, fillValue), AccessPixelPosition(i    , j + 1, fillValue) },
            { AccessPixelPosition(i + 1, j - 1, fillValue), AccessPixelPosition(i + 1, j, fillValue), AccessPixelPosition(i + 1, j + 1, fillValue) },
        };

        return neighbours;
    }

    /// <summary>
    /// Access a position and returns the pixel value
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="fillValue"></param>
    /// <remarks>
    /// If the intended position falls outside the Image bounds returns the fill value
    /// </remarks>
    /// <returns></returns>
    private char AccessPixelPosition(int i, int j, char fillValue)
    {
        if (i < 0 || j < 0 || i >= DimLength || j >= DimLength)
        {
            return fillValue;
        }

        return PixelMatrix[i, j];
    }
}
