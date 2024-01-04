using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2023.Day20.Model;

internal static partial class InputParser
{
    public static List<Module> ParseInput(string[] inputLines)
    {
        var modules = new List<Module>();

        var moduleInputs = new Dictionary<string, List<string>>();
        var moduleOutputs = new Dictionary<string, List<string>>();

        var regex = ModuleRegex();
        foreach (var line in inputLines)
        {
            var match = regex.Match(line);
            if (!match.Success)
            {
                throw new InvalidOperationException("Could not extract data");
            }
            var moduleName = match.Groups["Name"].Value;

            Module module = match.Groups["Type"].Value switch
            {
                "broadcaster" => new BradcasterModule(moduleName),
                "%" => new FlipFlopModule(moduleName),
                "&" => new ConjunctionModule(moduleName),
                _ => throw new UnreachableException()
            };

            modules.Add(module);

            // TODO: improve
            var destinations = match.Groups["Destinations"].Value
                .Split(", ")
                .ToList();

            moduleOutputs.Add(moduleName, destinations);
            if (module is not BradcasterModule)
            {
                foreach (var destination in destinations)
                {
                    if (moduleInputs.TryGetValue(destination, out var inputs))
                    {
                        inputs.Add(moduleName);
                    }
                    else
                    {
                        moduleInputs.Add(destination, [moduleName]);
                    }
                }
            }
        }

        // Add the inputs and outputs
        foreach (var module in modules)
        {
            var inputs = module is BradcasterModule
                ? new List<Module>()
                : modules.Where(m => moduleInputs[module.Name].Contains(m.Name)).ToList();
            var outputs = modules.Where(m => moduleOutputs[module.Name].Contains(m.Name)).ToList();

            module.AddInputs(inputs);
            module.AddOutputs(outputs);
        }

        return modules;
    }

    [GeneratedRegex(@"(?<Type>%|&|broadcaster)(?<Name>\w+)? -> (?<Destinations>.*)")]
    private static partial Regex ModuleRegex();
}

