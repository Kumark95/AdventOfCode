namespace AdventOfCode.Core.Solvers.Year2023.Day20.Model;

internal class Machine
{
    private readonly List<Module> _modules;
    private readonly Module _broadcastModule;

    public long TotalLowPulses { get; private set; } = 0;
    public long TotalHighPulses { get; private set; } = 0;
    public long TotalButtonPresses { get; private set; } = 0;

    public Machine(List<Module> modules)
    {
        _modules = modules;
        _broadcastModule = modules.Single(m => m is BradcasterModule);
    }

    public Module GetModule(string name)
    {
        return _modules.First(m => m.Name == name);
    }

    public void PushButton()
    {
        TotalButtonPresses++;

        // Always start with the button module sending a low pulse to the broadcast module
        var queue = new Queue<(Module sender, Module receiver, Pulse)>();
        queue.Enqueue((new ButtonModule(""), _broadcastModule, Pulse.Low));

        while (queue.Count > 0)
        {
            var (senderModule, receiverModule, pulse) = queue.Dequeue();

            // Update stats
            if (pulse == Pulse.Low)
            {
                TotalLowPulses++;
            }
            else
            {
                TotalHighPulses++;
            }

            // Send pulses to the outputs
            var pulseToSend = receiverModule.ReceivePulse(senderModule, pulse, TotalButtonPresses);
            if (pulseToSend is null)
            {
                continue;
            }

            foreach (var output in receiverModule.Outputs)
            {
                queue.Enqueue((receiverModule, output, (Pulse)pulseToSend));
            }
        }
    }
}
