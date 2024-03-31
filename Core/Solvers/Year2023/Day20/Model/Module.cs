namespace AdventOfCode.Core.Solvers.Year2023.Day20.Model;

internal abstract class Module
{
    public string Name { get; }
    public List<Module> Inputs { get; } = [];
    public List<Module> Outputs { get; } = [];

    protected Module(string name)
    {
        Name = name;
    }

    public virtual void AddInputs(List<Module> inputs)
    {
        Inputs.AddRange(inputs);
    }

    public virtual void AddOutputs(List<Module> outputs)
    {
        Outputs.AddRange(outputs);
    }

    public virtual Pulse? ReceivePulse(Module sender, Pulse pulse, long buttonPressesCount) => null;
}

internal class ButtonModule : Module
{
    public ButtonModule(string name) : base(name)
    {
    }
}

internal class BradcasterModule : Module
{
    public BradcasterModule(string name) : base(name)
    {
    }

    public override Pulse? ReceivePulse(Module sender, Pulse pulse, long buttonPressesCount) => pulse;
}

/// <summary>
/// Some modules only receive inputs like the "output" module in the example, or "rx" in the input file
/// </summary>
internal class DummyModule : Module
{
    public DummyModule(string name) : base(name)
    {
    }
}

internal class FlipFlopModule : Module
{
    private bool _state = false;

    public FlipFlopModule(string name) : base(name)
    {
    }

    public override Pulse? ReceivePulse(Module sender, Pulse pulse, long buttonPressesCount)
    {
        if (pulse == Pulse.High)
        {
            return null;
        }

        // Send a pulse based on the previous state and then flips
        var prevState = _state;
        _state = !_state;

        return prevState ? Pulse.Low : Pulse.High;
    }
}

internal class ConjunctionModule : Module
{
    private readonly Dictionary<string, Pulse> _previousInputPulses = new();

    public Dictionary<string, long> MinButtonPressesForHighPulse { get; } = new();

    public ConjunctionModule(string name) : base(name)
    {
    }

    public override Pulse? ReceivePulse(Module sender, Pulse pulse, long buttonPressesCount)
    {
        // Update state
        _previousInputPulses[sender.Name] = pulse;

        // Store when a high pulse is received for Part 2
        if (pulse == Pulse.High && !MinButtonPressesForHighPulse.ContainsKey(sender.Name))
        {
            MinButtonPressesForHighPulse[sender.Name] = buttonPressesCount;
        }

        return _previousInputPulses.Values.Any(p => p == Pulse.Low)
            ? Pulse.High
            : Pulse.Low;
    }

    public override void AddInputs(List<Module> inputs)
    {
        base.AddInputs(inputs);

        // Initially default to remembering a low pulse
        foreach (var input in inputs)
        {
            _previousInputPulses[input.Name] = Pulse.Low;
        }
    }
}
