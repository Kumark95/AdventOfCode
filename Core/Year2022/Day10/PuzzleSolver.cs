using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Year2022.Day10.Model;

namespace AdventOfCode.Core.Year2022.Day10;

[PuzzleName("Cathode-Ray Tube")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2022;
    public int Day => 10;

    public long SolvePartOne(string[] inputLines)
    {
        var registerX = 1;
        var cycleCounter = 1;

        var result = 0;
        foreach (var instruction in inputLines)
        {
            if (instruction == Operation.NoOperation)
            {
                if (cycleCounter % 40 == 20)
                {
                    result += cycleCounter * registerX;
                }
                cycleCounter++;

                continue;
            }

            var value = int.Parse(instruction.Split(' ')[1]);

            //
            if (cycleCounter % 40 == 20)
            {
                result += cycleCounter * registerX;
            }
            cycleCounter++;

            //
            if (cycleCounter % 40 == 20)
            {
                result += cycleCounter * registerX;
            }
            cycleCounter++;

            registerX += value;
        }

        return result;
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var crt = new CRT();

        foreach (var instruction in inputLines)
        {
            if (instruction == Operation.NoOperation)
            {
                crt.DrawPixel();
                continue;
            }

            // AddX operation
            var value = int.Parse(instruction.Split(' ')[1]);

            crt.DrawPixel();
            crt.DrawPixel();
            crt.MoveSprite(value);
        }

        crt.DrawImage();

        return 1;
    }
}
