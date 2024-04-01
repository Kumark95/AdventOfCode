using Spectre.Console;

namespace AdventOfCode.Core.Services;

internal static class ConsoleDisplayManager
{
    internal static void DisplayHeader()
    {
        AnsiConsole.Write(new FigletText("Advent of code").Color(Color.Green));
    }

    internal static void DisplayResults(IEnumerable<ExecutionResult> results)
    {
        var table = new Table();
        table.AddColumn("File");
        table.AddColumn("Expected");
        table.AddColumn("Value");
        table.AddColumn("Result");
        table.AddColumn("Execution time");
        table.AddColumn("Memory used");

        foreach (var result in results)
        {
            var resultHighlight = result.ExpectedValue == result.ActualValue
                ? "[green]Ok[/]"
                : "[red]Fail[/]";

            table.AddRow(
                result.Filename,
                result.ExpectedValue.ToString(),
                result.ActualValue.ToString(),
                resultHighlight,
                FormatTimeSpan(result.Metrics.ExecutionTime),
                FormatBytes(result.Metrics.MemoryUsed)
            );
        }

        AnsiConsole.Write(table);
    }

    private static string FormatTimeSpan(TimeSpan elapsed)
    {
        return elapsed.Minutes > 0
                ? $"{elapsed.Minutes}m {elapsed.Seconds}s {elapsed.Milliseconds}ms {elapsed.Microseconds}μs"
                : $"{elapsed.Seconds}s {elapsed.Milliseconds}ms {elapsed.Microseconds}μs";
    }

    private static string FormatBytes(long bytes)
    {
        const int scale = 1024;
        string[] orders = ["GB", "MB", "KB", "Bytes"];
        long max = (long)Math.Pow(scale, orders.Length - 1);

        foreach (string order in orders)
        {
            if (bytes > max)
            {
                return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);
            }

            max /= scale;
        }

        return "0 Bytes";
    }
}
