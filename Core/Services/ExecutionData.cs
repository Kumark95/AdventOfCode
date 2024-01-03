namespace AdventOfCode.Core.Services;

internal readonly record struct ExecutionData(object Result, TimeSpan ExecutionTime, long MemoryUsed);
