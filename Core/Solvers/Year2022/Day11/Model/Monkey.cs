using System.Diagnostics;

namespace AdventOfCode.Core.Solvers.Year2022.Day11.Model;

internal class Monkey
{
    public long InspectionCounter { get; private set; }

    private Queue<long> WorryLevelItems { get; init; }
    private Func<long, long> InspectionMethod { get; init; }
    private int Divisor { get; init; }
    private int CommonDivisor { get; set; }
    private int PositiveConditionNextMonkeyPosition { get; init; }
    private int NegativeConditionNextMonkeyPosition { get; init; }

    private Monkey(Queue<long> items, Func<long, long> inspectionOperation, int divisor, int positiveConditionNextMonkey, int negativeConditionNextMonkey)
    {
        InspectionCounter = 0;

        WorryLevelItems = items;
        InspectionMethod = inspectionOperation;
        Divisor = divisor;
        PositiveConditionNextMonkeyPosition = positiveConditionNextMonkey;
        NegativeConditionNextMonkeyPosition = negativeConditionNextMonkey;
    }

    public (long newWorryLevel, int nextMonkeyPosition) Play(bool isWorryReliefActivated)
    {
        InspectionCounter++;

        var worryLevel = WorryLevelItems.Dequeue();

        // CommonDivisor is required to prevent an overflow of values during multiplication
        // P.ex: Two monkeys with divisors 19 and 23
        // -> GCD = 19 * 23
        // -> 50 % 19 = 50 % (19 * 23) % 19
        // -> 50 % 23 = 50 % (19 * 23) % 23
        var newWorryLevel = InspectionMethod(worryLevel) % CommonDivisor;
        if (isWorryReliefActivated)
        {
            newWorryLevel = (long)Math.Floor(newWorryLevel / 3.0);
        }

        if (newWorryLevel % Divisor == 0)
        {
            return (newWorryLevel, PositiveConditionNextMonkeyPosition);
        }
        else
        {
            return (newWorryLevel, NegativeConditionNextMonkeyPosition);
        }
    }

    public bool HasItems()
    {
        return WorryLevelItems.Count > 0;
    }

    public void AddItem(long item)
    {
        WorryLevelItems.Enqueue(item);
    }

    public int GetTestDivisor()
    {
        return Divisor;
    }

    public void SetTroopCommonDivisor(int commonDivisor)
    {
        CommonDivisor = commonDivisor;
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
