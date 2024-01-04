namespace AdventOfCode.Core.Solvers.Year2023.Day20.Model;

internal class Machine
{
    private readonly List<Module> _modules;
    private readonly Module _broadcastModule;

    public Machine(List<Module> modules)
    {
        _modules = modules;
        _broadcastModule = modules.Single(m => m is BradcasterModule);
    }

    public (int lowPulses, int highPulses) PushButton()
    {
        // Always start with the button module sending a low pulse to the broadcast module
        var queue = new Queue<(Module sender, Module receiver, Pulse)>();
        queue.Enqueue((new ButtonModule(""), _broadcastModule, Pulse.Low));

        var lowPulses = 0;
        var highPulses = 0;
        while (queue.Count > 0)
        {
            var (senderModule, receiverModule, pulse) = queue.Dequeue();

            // TODO: Include the sender for the state config
            receiverModule.ReceivePulse(pulse);
        }

        return (lowPulses, highPulses);
    }
}

