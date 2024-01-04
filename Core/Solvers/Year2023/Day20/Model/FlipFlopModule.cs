namespace AdventOfCode.Core.Solvers.Year2023.Day20.Model;

internal class FlipFlopModule : Module
{
    private bool _state = false;

    public FlipFlopModule(string name) : base(name)
    {
    }

    public override Pulse? ReceivePulse(Pulse pulse)
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

    protected override void AfterInputs(List<Module> inputs)
    {
    }
}

