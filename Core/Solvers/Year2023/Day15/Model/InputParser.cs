namespace AdventOfCode.Core.Solvers.Year2023.Day15.Model;

internal static class InputParser
{
    public static Instruction[] ParseInstructions(string input)
    {
        return input
            .Split(',')
            .Select(instr =>
            {
                var operationIndex = instr.IndexOfAny(['=', '-']);
                var label = instr[..operationIndex];
                var operation = instr[operationIndex];
                int? focalLength = operation == '='
                    ? int.Parse(instr[(operationIndex + 1)..])
                    : null;

                return new Instruction(label, operation, focalLength);
            })
            .ToArray();
    }
}
