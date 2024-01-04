namespace AdventOfCode.Core.Solvers.Year2023.Day20.Model;

internal class ConjunctionModule : Module
{
    private readonly Dictionary<Module, Pulse> _previousInputPulses = new();

    public ConjunctionModule(string name) : base(name)
    {
    }

    public override Pulse? ReceivePulse(Pulse pulse)
    {
        throw new NotImplementedException();
    }

    protected override void AfterInputs(List<Module> inputs)
    {
        foreach (var input in inputs)
        {
            // Initially default to remembering a low pulse
            _previousInputPulses.Add(input, Pulse.Low);
        }
    }
}

