using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2022.Day05.Model;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2022.Day05;

[PuzzleName("Supply Stacks")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2022;
    public int Day => 5;

    private static (Stack<char>[] crateStacks, List<CraneMovement> movements) Input(string[] inputLines)
    {
        // Every single crate but the last one is 4 characters "[X] "
        var nStacks = (int)Math.Ceiling(inputLines[0].Length / 4.0);

        // Create lists of items as the stack will reverse the values
        var crateItemLists = new List<char>[nStacks];
        for (int i = 0; i < nStacks; i++)
        {
            crateItemLists[i] = new List<char>();
        }

        var craneMovements = new List<CraneMovement>();

        // Parse
        var crateRegex = new Regex(@"(?<Letter>[A-Z])", RegexOptions.Compiled);
        var movementRegex = new Regex(@"move (?<Crates>\d+) from (?<Start>\d+) to (?<End>\d+)", RegexOptions.Compiled);
        foreach (var line in inputLines)
        {
            var momentMatches = movementRegex.Match(line);
            if (momentMatches.Success)
            {
                craneMovements.Add(new CraneMovement(
                        int.Parse(momentMatches.Groups["Crates"].Value),
                        int.Parse(momentMatches.Groups["Start"].Value),
                        int.Parse(momentMatches.Groups["End"].Value)
                    ));
            }
            else
            {
                foreach (Match match in crateRegex.Matches(line))
                {
                    var stackNumber = (int)Math.Truncate(match.Index / 4.0);

                    crateItemLists[stackNumber].Add(char.Parse(match.Value));
                }
            }
        }

        // Generate the stacks from the lists
        var crateStacks = new Stack<char>[nStacks];
        for (int i = 0; i < nStacks; i++)
        {
            var list = crateItemLists[i];
            list.Reverse(); // Needed to put the items in the correct order

            crateStacks[i] = new Stack<char>(list);
        }

        return (crateStacks, craneMovements);
    }

    public object SolvePartOne(string[] inputLines)
    {
        var (crateStacks, craneMovements) = Input(inputLines);

        foreach (var movement in craneMovements)
        {
            var sourceStackIndex = movement.Start - 1;
            var targetStackIndex = movement.End - 1;

            for (int i = 0; i < movement.CrateQuantity; i++)
            {
                var crate = crateStacks[sourceStackIndex].Pop();
                crateStacks[targetStackIndex].Push(crate);
            }
        }

        var result = "";
        foreach (var stack in crateStacks)
        {
            result += stack.Pop();
        }

        Console.WriteLine(result);

        return 1;
    }

    public object SolvePartTwo(string[] inputLines)
    {
        var (crateStacks, craneMovements) = Input(inputLines);

        foreach (var movement in craneMovements)
        {
            var sourceStackIndex = movement.Start - 1;
            var targetStackIndex = movement.End - 1;

            var cratesToMove = new List<char>();
            for (int i = 0; i < movement.CrateQuantity; i++)
            {
                cratesToMove.Add(crateStacks[sourceStackIndex].Pop());
            }

            cratesToMove.Reverse();

            foreach (var crate in cratesToMove)
            {
                crateStacks[targetStackIndex].Push(crate);
            }
        }

        var result = "";
        foreach (var stack in crateStacks)
        {
            result += stack.Pop();
        }

        Console.WriteLine(result);

        return 1;
    }
}
