namespace AdventOfCode.Core.Solvers.Year2023.Day20.Model;

internal class BradcasterModule : Module
{
    public BradcasterModule(string name) : base(name)
    {
    }

    public override Pulse? ReceivePulse(Pulse pulse)
    {
        return pulse;
    }

    protected override void AfterInputs(List<Module> inputs)
    {
    }
}

