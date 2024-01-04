namespace AdventOfCode.Core.Solvers.Year2023.Day20.Model;

internal abstract class Module
{
    public string Name { get; }
    public List<Module> Inputs { get; private set; }
    public List<Module> Outputs { get; private set; }

    protected Module(string name)
    {
        Name = name;
        Inputs = [];
        Outputs = [];
    }

    public void AddInputs(List<Module> inputs)
    {
        Inputs.AddRange(inputs);

        AfterInputs(inputs);
    }

    public void AddOutputs(List<Module> outputs)
    {
        Outputs.AddRange(outputs);
    }

    protected abstract void AfterInputs(List<Module> inputs);
    public abstract Pulse? ReceivePulse(Pulse pulse);
}

