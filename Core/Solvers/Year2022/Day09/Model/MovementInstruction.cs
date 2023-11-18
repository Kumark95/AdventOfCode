namespace AdventOfCode.Core.Solvers.Year2022.Day09.Model;

internal record struct MovementInstruction(Direction Direction, int Steps)
{
    internal static MovementInstruction Parse(string content)
    {
        var parts = content.Split(' ');
        var direction = (Direction)char.Parse(parts[0]);
        var steps = int.Parse(parts[1]);

        return new MovementInstruction(direction, steps);
    }
}
