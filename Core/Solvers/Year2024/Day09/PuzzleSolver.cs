using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Solvers.Year2024.Day09;

[PuzzleName("Disk Fragmenter")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2024;
    public int Day => 9;

    [PuzzleInput(filename: "example.txt", expectedResult: 1928)]
    [PuzzleInput(filename: "input.txt", expectedResult: 6337367222422)]
    public object SolvePartOne(string[] inputLines)
    {
        var numbers = ParseNumbers(inputLines[0]);

        const int EMPTY = -1;
        (int[] memoryField, _, _) = GenerateField(numbers);

        var firstEmptyPos = 0;
        for (var pos = memoryField.Length - 1; pos >= 0; pos--)
        {
            var number = memoryField[pos];
            if (number == EMPTY)
            {
                continue;
            }

            firstEmptyPos = Array.IndexOf(memoryField, EMPTY, firstEmptyPos);
            if (firstEmptyPos > pos)
            {
                continue;
            }

            memoryField[firstEmptyPos] = number;
            memoryField[pos] = EMPTY;
        }

        return CalculateChecksum(ref memoryField);
    }



    [PuzzleInput(filename: "example.txt", expectedResult: 2858)]
    [PuzzleInput(filename: "input.txt", expectedResult: 6361380647183)]
    public object SolvePartTwo(string[] inputLines)
    {
        var numbers = ParseNumbers(inputLines[0]);

        const int EMPTY = -1;
        (int[] memoryField, Stack<FileInfo> fileRanges, List<RangeInfo> emptyRanges) = GenerateField(numbers, emptyNum: EMPTY);

        while (fileRanges.Count > 0)
        {
            FileInfo fileInfo = fileRanges.Pop();
            RangeInfo fileRange = fileInfo.Range;

            // Check empty ranges
            for (int emptyIdx = 0; emptyIdx < emptyRanges.Count; emptyIdx++)
            {
                RangeInfo emptyRange = emptyRanges[emptyIdx];

                // Prevent files from moving to the end of the disk
                if (emptyRange.Length == 0 || emptyRange.Start > fileRange.Start)
                {
                    continue;
                }

                if (fileRange.Length <= emptyRange.Length)
                {
                    MoveFile(ref memoryField, fileRange, emptyRange);

                    // Update empty spaces
                    emptyRanges[emptyIdx] = new RangeInfo(Start: emptyRange.Start + fileRange.Length, Length: emptyRange.Length - fileRange.Length);

                    break;
                }
            }
        }

        return CalculateChecksum(ref memoryField);
    }

    private record struct FileInfo(int FileId, RangeInfo Range);
    private record struct RangeInfo(int Start, int Length)
    {
        public readonly int End => Start + Length - 1;
    };

    private static int[] ParseNumbers(string input) => input.Select(c => c - '0').ToArray();

    private static void MoveFile(ref int[] memoryField, RangeInfo fileRange, RangeInfo emptyRange)
    {
        int filePos = fileRange.Start;
        int emptyPos = emptyRange.Start;

        for (int i = 0; i < fileRange.Length; i++)
        {
            memoryField[emptyPos++] = memoryField[filePos];
            memoryField[filePos++] = -1;
        }
    }

    private static (int[] MemoryField, Stack<FileInfo> FileRanges, List<RangeInfo> emptyRanges) GenerateField(int[] numbers)
    {
        var capacity = numbers.Sum();
        var memoryField = new int[capacity];
        var fileRanges = new Stack<FileInfo>();
        var emptyRanges = new List<RangeInfo>();

        var currentBlockId = 0;
        var isFile = true;
        var pos = 0;

        foreach (var number in numbers)
        {
            var fillValue = isFile ? currentBlockId : -1;
            Array.Fill(memoryField, fillValue, startIndex: pos, count: number);

            // Update ranges
            if (isFile)
            {
                var fileInfo = new FileInfo(currentBlockId, new RangeInfo(pos, number));
                fileRanges.Push(fileInfo);

                currentBlockId++;
            }
            else
            {
                emptyRanges.Add(new RangeInfo(pos, number));
            }

            pos += number;

            // Alternate between file and free space layout
            isFile = !isFile;
        }

        return (memoryField, fileRanges, emptyRanges);
    }

    private static long CalculateChecksum(ref int[] memoryField)
    {
        long result = 0;

        for (var i = 0; i < memoryField.Length; i++)
        {
            long number = memoryField[i];
            if (number == -1)
            {
                continue;
            }
            result += number * i;
        }

        return result;
    }
}
