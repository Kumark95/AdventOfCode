namespace AdventOfCode.Core.Services;

internal readonly record struct ExecutionData(long? Result, TimeSpan ExecutionTime, long MemoryUsed);
