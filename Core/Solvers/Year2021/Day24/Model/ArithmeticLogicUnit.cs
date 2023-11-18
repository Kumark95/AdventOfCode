using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2021.Day24.Model;

internal class ArithmeticLogicUnit
{
    /// <summary>
    /// Run the instruction group
    /// </summary>
    /// <param name="inputDigit"></param>
    /// <param name="z"></param>
    /// <param name="parameters"></param>
    /// <remarks>
    /// Each instruction group contains 14 instructions and differ only in 3 parameters
    /// </remarks>
    /// <returns></returns>
    static long? RunInstructionGroup(int inputDigit, long z, InstructionParameters parameters)
    {
        // Each instruction group contain 14 instructions
        // The registers w, x, y are reset during the execution of each group
        // Only the value of z is persisted between executions

        // The instructions used can be simplified in fewer instructions
        var x = z % 26;

        // We can skip the execution if we know that z will only grow bigger and never reach 0
        if (parameters.AddX < 0 && x + parameters.AddX != inputDigit)
        {
            return null;
        }
        else
        {
            if (parameters.AddX < 0)
            {
                // Truncate z
                z /= 26;
            }

            if (x + parameters.AddX != inputDigit)
            {
                z *= 26;
                z += inputDigit + parameters.AddY;
            }
        }

        return z;
    }

    /// <summary>
    /// Find the model number recursively
    /// </summary>
    /// <param name="z"></param>
    /// <param name="position"></param>
    /// <param name="candidateNumber"></param>
    /// <param name="parameters"></param>
    /// <param name="digits"></param>
    /// <returns></returns>
    static long? FindModelNumberRecursive(long z, int position, long candidateNumber, List<InstructionParameters> parameters, int[] digits)
    {
        if (position == parameters.Count)
        {
            return z == 0 ? candidateNumber : null;
        }

        var positionParameters = parameters[position];
        foreach (var digit in digits)
        {
            // We can skip the digits where the value of z increases
            var newZ = RunInstructionGroup(digit, z, positionParameters);
            if (!newZ.HasValue)
            {
                continue;
            }

            var result = FindModelNumberRecursive(newZ.Value, position + 1, candidateNumber * 10 + digit, parameters, digits);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    /// <summary>
    /// Find the model number that matches the search mode
    /// </summary>
    /// <param name="instructions"></param>
    /// <param name="searchMode"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static long FindModelNumber(string[] instructions, SearchMode searchMode)
    {
        var parameters = Parse(instructions);

        var digits = searchMode switch
        {
            SearchMode.Maximum => new int[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 },
            SearchMode.Minimum => new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
            _ => throw new Exception("Invalid")
        };

        var result = FindModelNumberRecursive(z: 0, position: 0, candidateNumber: 0, parameters, digits);
        if (!result.HasValue)
        {
            throw new Exception("The model number was not computed");
        }

        return result.Value;
    }

    /// <summary>
    /// Parse the instructions and extract the key values
    /// </summary>
    /// <param name="instructions"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static List<InstructionParameters> Parse(string[] instructions)
    {
        var regex = new Regex(@"inp w
mul x 0
add x z
mod x 26
div z (?<Div>-?\d+)
add x (?<AddX>-?\d+)
eql x w
eql x 0
mul y 0
add y 25
mul y x
add y 1
mul z y
mul y 0
add y w
add y (?<AddY>-?\d+)
mul y x
add z y");

        // The instruction set consists of groups of 18 instructions (14 times)
        // that differ only in 3 values
        var instructionValues = new List<InstructionParameters>();
        foreach (var instructionGroup in instructions.Chunk(18))
        {
            // Extract the different values
            var match = regex.Match(string.Join(Environment.NewLine, instructionGroup));
            if (!match.Success)
            {
                throw new Exception("Could not parse the instruction set");
            }

            var divValue = int.Parse(match.Groups["Div"].Value);
            var addXValue = int.Parse(match.Groups["AddX"].Value);
            var addYValue = int.Parse(match.Groups["AddY"].Value);

            instructionValues.Add(new InstructionParameters(divValue, addXValue, addYValue));
        }

        return instructionValues;
    }
}


internal record struct InstructionParameters(int DivX, int AddX, int AddY);

internal enum SearchMode
{
    Maximum,
    Minimum
}