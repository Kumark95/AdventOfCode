namespace AdventOfCode.Core.Year2021.Day21.Model;

internal class DeterministicDie
{
    public int RollCounter { get; set; }
    private int Sides { get; init; }

    public DeterministicDie(int sides)
    {
        RollCounter = 0;
        Sides = sides;
    }

    public int Roll()
    {
        RollCounter++;

        // Subtract 1 before modulo. Add 1 after as the numbers start from 1 not 0
        return (RollCounter - 1) % Sides + 1;
    }
}
