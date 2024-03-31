using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2023.Day20.Model;

internal static partial class InputParser
{
    [GeneratedRegex(@"(?<Type>%|&|broadcaster)(?<Name>\w+)? -> (?<Destinations>.*)")]
    private static partial Regex ModuleRegex();

    public static List<Module> ParseInput(string[] inputLines)
    {
        var modules = new List<Module>();
        var regex = ModuleRegex();

        // Build the relations between the modules
        var moduleInputNames = new Dictionary<string, List<string>>();
        var moduleOutputNames = new Dictionary<string, List<string>>();
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

            // Build outputs
            var destinations = match.Groups["Destinations"].Value
                .Split(", ")
                .ToList();

            moduleOutputNames[moduleName] = destinations;

            // Build the inverse relations
            foreach (var destination in destinations)
            {
                if (moduleInputNames.TryGetValue(destination, out var inputs))
                {
                    inputs.Add(moduleName);
                }
                else
                {
                    moduleInputNames.Add(destination, [moduleName]);
                }

                // Init an emprty array of output for the dummy modules
                if (!moduleOutputNames.ContainsKey(destination))
                {
                    moduleOutputNames[destination] = [];
                }
            }
        }

        // Detect modules that do not have any destination themselves but receive inputs
        var dummyModules = moduleOutputNames
            .Where(kv => kv.Value.Count == 0)
            .Select(kv => new DummyModule(kv.Key));
        modules.AddRange(dummyModules);

        // Add the inputs and outputs to each module
        foreach (var module in modules)
        {
            var inputs = module is BradcasterModule
                ? new List<Module>()
                : modules.Where(m => moduleInputNames[module.Name].Contains(m.Name)).ToList();
            module.AddInputs(inputs);

            var outputs = modules.Where(m => moduleOutputNames[module.Name].Contains(m.Name)).ToList();
            module.AddOutputs(outputs);
        }

        return modules;
    }
}
