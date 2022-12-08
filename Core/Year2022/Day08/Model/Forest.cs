namespace AdventOfCode.Core.Year2022.Day08.Model;

internal class Forest
{
    internal char[,] HeightMap { get; }
    internal bool[,] VisibilityMap { get; }
    internal int[,] ScenicScore { get; }
    private int RowDimension { get; }
    private int ColDimension { get; }

    internal Forest(string[] lines)
    {
        RowDimension = lines.Length;
        ColDimension = lines[0].Length;

        HeightMap = new char[RowDimension, ColDimension];
        for (int row = 0; row < RowDimension; row++)
        {
            for (int col = 0; col < ColDimension; col++)
            {
                HeightMap[row, col] = lines[row][col];
            }
        }

        VisibilityMap = new bool[RowDimension, ColDimension];
        ScenicScore = new int[RowDimension, ColDimension];

        for (int row = 0; row < RowDimension; row++)
        {
            for (int col = 0; col < ColDimension; col++)
            {
                VisibilityMap[row, col] = IsVisible(row, col);
                ScenicScore[row, col] = CalculateScenicScore(row, col);
            }
        }
    }

    internal int CountVisibleTrees()
    {
        var count = 0;
        for (int row = 0; row < RowDimension; row++)
        {
            for (int col = 0; col < ColDimension; col++)
            {
                if (VisibilityMap[row, col])
                {
                    count++;
                }
            }
        }

        return count;
    }

    internal int HeighestScenicScore()
    {
        var maxScore = 0;
        for (int row = 0; row < RowDimension; row++)
        {
            for (int col = 0; col < ColDimension; col++)
            {
                maxScore = Math.Max(maxScore, ScenicScore[row, col]);
            }
        }

        return maxScore;
    }

    private bool IsVisible(int row, int col)
    {
        if (row == 0 || col == 0)
        {
            // Trees around the edges are always visible
            return true;
        }
        var currentValue = HeightMap[row, col];

        var topVisibility = true;
        for (int targetRow = row - 1; targetRow >= 0; targetRow--)
        {
            topVisibility = topVisibility && currentValue > HeightMap[targetRow, col];
        }
        if (topVisibility)
        {
            return true;
        }

        var bottomVisibility = true;
        for (int targetRow = row + 1; targetRow < RowDimension; targetRow++)
        {
            bottomVisibility = bottomVisibility && currentValue > HeightMap[targetRow, col];
        }
        if (bottomVisibility)
        {
            return true;
        }

        var leftVisibility = true;
        for (int targetCol = col - 1; targetCol >= 0; targetCol--)
        {
            leftVisibility = leftVisibility && currentValue > HeightMap[row, targetCol];
        }
        if (leftVisibility)
        {
            return true;
        }

        var rightVisibility = true;
        for (int targetCol = col + 1; targetCol < ColDimension; targetCol++)
        {
            rightVisibility = rightVisibility && currentValue > HeightMap[row, targetCol];
        }
        if (rightVisibility)
        {
            return true;
        }

        return false;
    }

    private int CalculateScenicScore(int row, int col)
    {
        if (row == 0 || col == 0)
        {
            return 0;
        }
        var currentValue = HeightMap[row, col];

        var topDistance = 0;
        for (int targetRow = row - 1; targetRow >= 0; targetRow--)
        {
            topDistance++;

            if (currentValue <= HeightMap[targetRow, col])
            {
                break;
            }
        }

        var bottomDistance = 0;
        for (int targetRow = row + 1; targetRow < RowDimension; targetRow++)
        {
            bottomDistance++;

            if (currentValue <= HeightMap[targetRow, col])
            {
                break;
            }
        }

        var leftDistance = 0;
        for (int targetCol = col - 1; targetCol >= 0; targetCol--)
        {
            leftDistance++;

            if (currentValue <= HeightMap[row, targetCol])
            {
                break;
            }
        }

        var rightDistance = 0;
        for (int targetCol = col + 1; targetCol < ColDimension; targetCol++)
        {
            rightDistance++;

            if (currentValue <= HeightMap[row, targetCol])
            {
                break;
            }
        }

        return topDistance * bottomDistance * leftDistance * rightDistance;
    }

    internal void Draw()
    {
        Console.WriteLine("-- Height map --");
        for (int row = 0; row < RowDimension; row++)
        {
            for (int col = 0; col < ColDimension; col++)
            {
                Console.Write(HeightMap[row, col] + " ");
            }
            Console.WriteLine();
        }

        Console.WriteLine("-- Visibility map --");
        for (int row = 0; row < RowDimension; row++)
        {
            for (int col = 0; col < ColDimension; col++)
            {
                var val = VisibilityMap[row, col] ? "T" : ".";
                Console.Write(val + " ");
            }
            Console.WriteLine();
        }
    }
}
