using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Year2021.Day16;

[PuzzleName("Packet Decoder")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 16;

    /// <summary>
    /// Returns the binary representation of the input
    /// </summary>
    /// <remarks>
    /// Padding is used to make each hex digit a binary representation with width 4
    /// </remarks>
    /// <param name="inputLines"></param>
    /// <returns></returns>
    private static string Input(string[] inputLines)
    {
        return inputLines[0]
            .Select(c =>
            {
                var dec = Convert.ToInt32(c.ToString(), fromBase: 16);
                var binary = Convert.ToString(dec, toBase: 2);

                return binary.PadLeft(totalWidth: 4, paddingChar: '0');
            })
            .Aggregate((s1, s2) => s1 + "" + s2);
    }

    public long SolvePartOne(string[] inputLines)
    {
        var input = Input(inputLines);

        var packet = Packet.Decode(input);
        return Packet.SumVersions(packet);
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var input = Input(inputLines);

        var packet = Packet.Decode(input);
        return packet.Value;
    }
}
