namespace AdventOfCode.Core.Solvers.Year2023.Day02.Model;

internal record struct GameRound(int RedCubes, int GreenCubes, int BlueCubes)
{
    internal static GameRound[] ParseGame(string gameDefinition)
    {
        var separatorIndex = gameDefinition.IndexOf(':');
        var roundDefinitions = gameDefinition.Substring(separatorIndex + 1).Split("; ");

        return roundDefinitions
            .Select(r => ParseRound(r))
            .ToArray();
    }

    internal static GameRound ParseRound(string roundDefinition)
    {
        var redCubes = 0;
        var greenCubes = 0;
        var blueCubes = 0;

        foreach (var cube in roundDefinition.Trim().Split(", "))
        {
            var cubeParts = cube.Split(' ');
            var cubeNumber = int.Parse(cubeParts[0]);
            var cubeColor = cubeParts[1];

            if (cubeColor == "red")
            {
                redCubes += cubeNumber;
            }
            else if (cubeColor == "green")
            {
                greenCubes += cubeNumber;

            }
            else if (cubeColor == "blue")
            {
                blueCubes += cubeNumber;
            }
        }

        return new GameRound(redCubes, greenCubes, blueCubes);
    }

    internal readonly bool IsValid(int availableRedCubes, int availableGreenCubes, int availableBlueCubes)
    {
        return RedCubes <= availableRedCubes
            && GreenCubes <= availableGreenCubes
            && BlueCubes <= availableBlueCubes;
    }
}
