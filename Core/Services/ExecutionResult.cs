namespace AdventOfCode.Core.Services;

internal readonly record struct ExecutionResult(string Filename, ulong ExpectedValue, ulong ActualValue, ExecutionMetrics Metrics);
