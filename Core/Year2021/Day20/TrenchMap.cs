namespace AdventOfCode.Core.Year2021.Day20;

internal class TrenchMap
{
    public char[] EnhancementAlgorithm { get; init; }
    public Image Image { get; init; }

    public TrenchMap(char[] enhancementAlgorithm, char[,] image)
    {
        EnhancementAlgorithm = enhancementAlgorithm;
        Image = new Image(image);
    }

    public TrenchMap(string enhancementAlgorithm, string[] imageLines)
    {
        var matrixLenght = imageLines[0].Length;
        var image = new char[matrixLenght, matrixLenght];

        for (int i = 0; i < matrixLenght; i++)
        {
            var line = imageLines[i];
            for (int j = 0; j < matrixLenght; j++)
            {
                image[i, j] = line[j];
            }
        }

        EnhancementAlgorithm = enhancementAlgorithm.ToArray();
        Image = new Image(image);
    }

    /// <summary>
    /// Apply the enhancement algorithm n times
    /// </summary>
    /// <param name="iterations"></param>
    /// <returns></returns>
    public TrenchMap ApplyEnhancement(int iterations)
    {
        var baseImage = Image;
        for (int iteration = 1; iteration <= iterations; iteration++)
        {
            // The fill value is used when accessing one of the "infinite" positions
            var fillValue = Image.DARK_PIXEL;
            if (EnhancementAlgorithm[0] == Image.LIGHT_PIXEL && iteration % 2 == 0)
            {
                // When the algorithm starts with a light pixel, all "infinite2 positions became lit
                // Then on the next iteration dark again
                fillValue = Image.LIGHT_PIXEL;
            }

            // The output image expands in both dimensions (e.g. 5x5 -> 7x7)
            var expandedImage = baseImage.Expand(fillValue);
            var expandedImgDimLength = expandedImage.PixelMatrix.GetLength(0);

            var outputPixelMatrix = new char[expandedImgDimLength, expandedImgDimLength];

            // Build the new image
            for (int i = 0; i < expandedImgDimLength; i++)
            {
                for (int j = 0; j < expandedImgDimLength; j++)
                {
                    outputPixelMatrix[i, j] = EnhancePixel(expandedImage, i, j, fillValue);
                }
            }

            //
            baseImage = new Image(outputPixelMatrix);
        }

        return new TrenchMap(EnhancementAlgorithm, baseImage.PixelMatrix);
    }

    /// <summary>
    /// Enhance a single pixel
    /// </summary>
    /// <param name="image"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="fillValue"></param>
    /// <remarks>
    /// Uses the neighbor pixels to determine the index in the algorithm
    /// </remarks>
    /// <returns></returns>
    private char EnhancePixel(Image image, int x, int y, char fillValue)
    {
        // Check neighbors
        var neighbours = image.FindNeighbours(x, y, fillValue);
        var neighboursLength = neighbours.GetLength(0);

        // Generate binary representation
        var binaryRepresentation = "";
        for (int i = 0; i < neighboursLength; i++)
        {
            for (int j = 0; j < neighboursLength; j++)
            {
                binaryRepresentation += neighbours[i, j] == Image.LIGHT_PIXEL ? "1" : "0";
            }
        }

        var algorithmIndex = Convert.ToInt32(binaryRepresentation, fromBase: 2);
        return EnhancementAlgorithm[algorithmIndex];
    }
}
