namespace AdventOfCode.Core.Solvers.Year2023.Day15.Model;

internal class BoxGroup
{
    private readonly Box[] _boxes;

    public BoxGroup()
    {
        _boxes = new Box[256];
        for (int i = 0; i < _boxes.Length; i++)
        {
            _boxes[i] = new Box();
        }
    }

    public void RunInstructions(Instruction[] instructions)
    {
        foreach (var instruction in instructions)
        {
            var boxIndex = AsciiStringHelper.Hash(instruction.Label);

            if (instruction.Operation == '=')
            {
                _boxes[boxIndex].AddLens(new Lens(instruction.Label, (int)instruction.FocalLength!));
            }
            else
            {
                _boxes[boxIndex].RemoveLens(instruction.Label);
            }
        }
    }

    public long CalculateFocusingPower()
    {
        var result = 0;

        for (int boxIdx = 0; boxIdx < _boxes.Length; boxIdx++)
        {
            var box = _boxes[boxIdx];

            for (int lensIdx = 0; lensIdx < box.Lenses.Count; lensIdx++)
            {
                var lens = box.Lenses[lensIdx];

                result += (boxIdx + 1) * (lensIdx + 1) * (lens.FocalLength);
            }
        }

        return result;
    }

    public void Print()
    {
        for (int i = 0; i < 256; i++)
        {
            var b = _boxes[i];
            if (b.Lenses.Count > 0)
            {
                Console.WriteLine($"Box {i}: {string.Join(' ', b.Lenses)}");
            }
        }
    }
}
