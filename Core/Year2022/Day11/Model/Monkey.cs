using System.Diagnostics;

namespace AdventOfCode.Core.Year2022.Day11.Model;

internal class Monkey
{
    public long InspectionCounter { get; private set; }

    private Queue<long> Items { get; init; }
    private Func<long, long> InspectionMethod { get; init; }
    private int Divisor { get; init; }
    private int PositiveConditionNextMonkeyPosition { get; init; }
    private int NegativeConditionNextMonkeyPosition { get; init; }

    private Monkey(Queue<long> items, Func<long, long> inspectionOperation, int divisor, int positiveConditionNextMonkey, int negativeConditionNextMonkey)
    {
        InspectionCounter = 0;

        Items = items;
        InspectionMethod = inspectionOperation;
        Divisor = divisor;
        PositiveConditionNextMonkeyPosition = positiveConditionNextMonkey;
        NegativeConditionNextMonkeyPosition = negativeConditionNextMonkey;
    }

    public (long newItem, int nextMonkeyPosition) Play(bool isWorryReliefActivated)
    {
        var currentItem = Items.Dequeue();

        var newItem = isWorryReliefActivated
            ? (long)Math.Floor(InspectionMethod(currentItem) / 3.0)
            : InspectionMethod(currentItem);
        InspectionCounter++;

        if (newItem % Divisor == 0)
        {
            return (newItem, PositiveConditionNextMonkeyPosition);
        }
        else
        {
            return (newItem, NegativeConditionNextMonkeyPosition);
        }
    }

    public bool HasItems()
    {
        return Items.Count > 0;
    }

    public void AddItem(long item)
    {
        Items.Enqueue(item);
    }

    #region PARSING
    public static Monkey Parse(string[] propertyLines)
    {
        var items = ParseItems(propertyLines[1]);
        var inspectionMethod = ParseInspectionMethod(propertyLines[2]);
        var divisor = ParseDivisor(propertyLines[3]);
        var positiveConditionNextMonkey = ParseNextMonkey(propertyLines[4]);
        var negativeConditionNextMonkey = ParseNextMonkey(propertyLines[5]);

        return new Monkey(items, inspectionMethod, divisor, positiveConditionNextMonkey, negativeConditionNextMonkey);
    }

    private static Queue<long> ParseItems(string content)
    {
        var itemList = content
            .Split(": ")[1]
            .Split(",")
            .Select(i => long.Parse(i))
            .ToList();

        return new Queue<long>(itemList);
    }

    private static Func<long, long> ParseInspectionMethod(string content)
    {
        var operationParts = content
                .Split("old ")[1]
                .Split(" ");
        var operation = (Operation)char.Parse(operationParts[0]);
        var operationValueStr = operationParts[1];
        long.TryParse(operationValueStr, out long operationValue);

        return operation switch
        {
            Operation.Multiplication => (subject) => subject * (operationValueStr == "old" ? subject : operationValue),
            Operation.Division => (subject) => subject / (operationValueStr == "old" ? subject : operationValue),
            Operation.Addition => (subject) => subject + (operationValueStr == "old" ? subject : operationValue),
            Operation.Subtraction => (subject) => subject - (operationValueStr == "old" ? subject : operationValue),
            _ => throw new UnreachableException("Invalid operation")
        };
    }

    private static int ParseDivisor(string content)
    {
        var divisorParts = content
                .Split("divisible by ");
        return int.Parse(divisorParts[1]);
    }

    private static int ParseNextMonkey(string content)
    {
        var divisorParts = content
                .Split("throw to monkey ");
        return int.Parse(divisorParts[1]);
    }
    #endregion
}
