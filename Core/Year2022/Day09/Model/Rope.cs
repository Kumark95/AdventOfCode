using System.Diagnostics;

namespace AdventOfCode.Core.Year2022.Day09.Model;

internal class Rope
{
    private List<Position> KnotPositions { get; init; }
    private HashSet<Position> UniqueTailPositions { get; init; }

    internal Rope(int knots)
    {
        if (knots < 2)
        {
            throw new Exception("The rope needs at least 2 knows for its head and tail");
        }

        KnotPositions = new List<Position>();
        for (int i = 0; i < knots; i++)
        {
            KnotPositions.Add(new Position(0, 0));
        }

        UniqueTailPositions = new HashSet<Position>();
    }

    internal void Move(MovementInstruction instruction)
    {
        var tailIndex = KnotPositions.Count - 1;

        for (int step = 1; step <= instruction.Steps; step++)
        {
            for (int index = 0; index < KnotPositions.Count; index++)
            {
                MoveKnot(index, instruction.Direction);

                if (index == tailIndex)
                {
                    UniqueTailPositions.Add(KnotPositions[tailIndex]);
                }
            }
        }
    }

    private void MoveKnot(int index, Direction direction)
    {
        var currentKnotPosition = KnotPositions[index];

        if (index == 0)
        {
            KnotPositions[0] = direction switch
            {
                Direction.Up => currentKnotPosition with { Y = currentKnotPosition.Y + 1 },
                Direction.Down => currentKnotPosition with { Y = currentKnotPosition.Y - 1 },
                Direction.Right => currentKnotPosition with { X = currentKnotPosition.X + 1 },
                Direction.Left => currentKnotPosition with { X = currentKnotPosition.X - 1 },
                _ => throw new UnreachableException("Invalid direction")
            };
        }
        else
        {
            var previousKnotPosition = KnotPositions[index - 1];
            if (previousKnotPosition.EuclideanDistance(currentKnotPosition) < 2)
            {
                // The knot is at most 1 step away
                return;
            }

            var positionDifference = previousKnotPosition - currentKnotPosition;

            var xMaxDistance = positionDifference.X switch
            {
                >= 1 => 1,
                <= -1 => -1,
                _ => 0
            };
            var yMaxDistance = positionDifference.Y switch
            {
                >= 1 => 1,
                <= -1 => -1,
                _ => 0
            };

            KnotPositions[index] = new Position(currentKnotPosition.X + xMaxDistance, currentKnotPosition.Y + yMaxDistance);
        }
    }

    internal int UniqueTailPositionCount()
    {
        return UniqueTailPositions.Count;
    }
}
