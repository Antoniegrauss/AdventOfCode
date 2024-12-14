using Xunit.Sdk;

namespace AoC23.Day20;

public class Network
{
    // These are connected to the broadcaster
    List<string> _startModules = new();
    // Press button until state is start state again
    List<(string, bool)> _startStates = new();

    Dictionary<string, Module> _modules = new();
    Queue<Signal> _signalsToProcess = new();

    public Network(string input)
    {
        string[] lines = StringUtilities.SplitLines(input);
        foreach (string line in lines)
        {
            if (line.Contains("broadcaster"))
            {
                _startModules = [..
                    StringUtilities.SplitWithTrim(
                    StringUtilities.SplitWithTrim(line, "->")[1], ",")];
                continue;
            }
            Module newModule = new(line);
            _modules.Add(newModule._name, newModule);
        }

        AddOutputModules();
        WireInputConnections();
        _startStates = GetState();
    }

    private void AddOutputModules()
    {
        List<Module> outputModules = [];
        foreach (Module module in _modules.Values)
        {
            foreach (string output in module._outputConnections)
            {
                if (!_modules.ContainsKey(output))
                {
                    // Add a leading symbol for the module parser
                    Module outputModule = new Module
                    {
                        _name = output
                    };
                    outputModules.Add(outputModule);
                }
            }
        }

        foreach (Module output in outputModules) _modules.Add(output._name, output);
    }

    private void WireInputConnections()
    {
        bool defaultStartValue = false;
        // For all modules connect the outputs of each sender
        // as inputs to the receiver
        foreach (Module module in _modules.Values)
        {
            foreach (string output in module._outputConnections)
            {
                _modules[output]._inputConnections.Add(module._name, defaultStartValue);
            }
        }
    }

    public long CalculateScorePart1(long buttonPresses)
    {
        int steps = 0;
        do
        {
            steps += 1;
            ProcessAllSignals();

        } while (!StateIsSameAsStart() && steps < buttonPresses);

        return CalculateFinalScore(buttonPresses, steps);
    }

    private long CalculateFinalScore(long buttonPresses, int steps)
    {
        (long lowsTotal, long highsTotal) = Scores();
        // Note that the button sends a low pulse to the broadcaster, which
        // sends a low pulse to all the starting modules connected to it.
        lowsTotal += steps * (_startModules.Count + 1);

        long remainder = buttonPresses % steps;
        if (remainder != 0) throw new Exception("Remainder should be 0");
        long numLoops = buttonPresses / steps;
        return numLoops * lowsTotal * numLoops * highsTotal;
    }

    public static long FindCycleFor(string module, string input)
    {
        Network network = new(input);
        long steps = 0;
        for (int i = 0; i < 20000; i++)
        {
            if (network.ProcessAllSignals(toWatch: module, searchedValue: true))
            {
                return steps + 1;
            }
            steps += 1;
        }

        return -1;
    }

    private void ProcessAllSignals()
    {
        PressButton();
        while (_signalsToProcess.Count > 0) ProcessSignal();
    }

    private void ProcessSignal()
    {
        Signal toProcess = _signalsToProcess.Dequeue();
        List<Signal> newOutputs =
            _modules[toProcess._receiver].DetermineOutput(toProcess);
        foreach (Signal newOutput in newOutputs) _signalsToProcess.Enqueue(newOutput);
    }

    private bool ProcessAllSignals(string toWatch, bool searchedValue)
    {
        PressButton();
        bool result = false;
        while (_signalsToProcess.Count > 0)
        {
            if (ProcessSignal(toWatch, searchedValue)) result = true;
        }
        return result;
    }

    private bool ProcessSignal(string toWatch, bool searchedValue)
    {
        Signal toProcess = _signalsToProcess.Dequeue();
        List<Signal> newOutputs =
            _modules[toProcess._receiver].DetermineOutput(toProcess);
        if (toProcess._receiver == toWatch
            && _modules[toProcess._receiver]._outputState == searchedValue) return true;
        foreach (Signal newOutput in newOutputs) _signalsToProcess.Enqueue(newOutput);
        return false;
    }

    private void PressButton()
    {
        _signalsToProcess.Clear();
        foreach (string startModule in _startModules)
        {
            _signalsToProcess.Enqueue(new Signal
            {
                _receiver = startModule,
                _value = false
            });
        }
    }

    private bool StateIsSameAsStart()
    {
        foreach (var pair in _startStates.Zip(GetState()))
        {
            if (pair.First.Item2 != pair.Second.Item2) return false;
            if (pair.First.Item1 != pair.Second.Item1) throw new Exception("States do not have the same modules");
        }
        return true;
    }

    private List<(string, bool)> GetState()
    {
        return [.. _modules.Values.Select(module =>
            (module._name, module._outputState))];
    }

    // Returns the low and high pulses per loop
    private (long, long) Scores()
    {
        return (_modules.Values.Sum(module => module._lowPulses),
            _modules.Values.Sum(module => module._highPulses));
    }
}

public struct Signal
{
    public string _receiver;
    public string _sender;
    public bool _value;
}

public class Module
{
    public Dictionary<string, bool> _inputConnections = new();
    public HashSet<string> _outputConnections = new();

    private ModuleType _type;

    public bool _outputState = false;
    public string _name;
    public bool _outputNodeTriggered = false;

    public long _lowPulses = 0;
    public long _highPulses = 0;

    private enum ModuleType
    {
        FlipFlop,
        Conjunction,
        Output
    }

    public Module()
    {
    }

    public Module(string input)
    {
        char moduleType = input[0];
        if (moduleType == '%') _type = ModuleType.FlipFlop;
        else if (moduleType == '&')
        {
            _type = ModuleType.Conjunction;
            // Since all inputs default to false, the output should default to true
            _outputState = true;
        }
        else _type = ModuleType.Output;

        input = input.Remove(0, 1);
        string[] parts = StringUtilities.SplitWithTrim(input, "->");
        _name = parts[0];

        if (parts.Length < 2)
        {
            _outputConnections = [];
            return;
        }
        _outputConnections = [.. StringUtilities.SplitWithTrim(parts[1], ",")];
    }

    public List<Signal> DetermineOutput(Signal input)
    {
        bool? result = null;
        if (_type == ModuleType.FlipFlop) result = FlipFlop(input);
        else if (_type == ModuleType.Conjunction) result = Conjunction(input);
        if (_type == ModuleType.Output)
        {
            _outputNodeTriggered = input._value;
        }
        if (result == null) return [];

        IncrementPulses();
        string sender = _name;
        return [.. _outputConnections.Select(output => new Signal{
            _value=result.Value,
            _sender=sender,
            _receiver=output
        })];
    }

    private void IncrementPulses()
    {
        if (_outputState) _highPulses += _outputConnections.Count;
        else _lowPulses += _outputConnections.Count;
    }

    private bool? FlipFlop(Signal input)
    {
        if (input._value) return null;
        // If low, flip and send state
        _outputState = !_outputState;
        return _outputState;
    }

    private bool? Conjunction(Signal input)
    {
        // Remember new value
        _inputConnections[input._sender] = input._value;

        // Acts like inverter + AND
        _outputState = !_inputConnections.Values.All(x => x);
        return _outputState;
    }
}

public class Day20
{
    public static long Part1(string input)
    {
        // Add all new output signals in a queue, FIFO
        // Start with low signal from broadcaster
        Network network = new(input);
        return network.CalculateScorePart1(buttonPresses: 1000);
    }

    public static long Part2(string input, List<string> conjunctionModules)
    {
        // Check when the 4 conjunction modules that are connected to the one before rx
        // pl & mz & lz & zm -> bn -> rx
        // send a high pulse together
        List<long> cycles = [.. conjunctionModules.Select(module => Network.FindCycleFor(module, input))];

        // Then get the LCM of those 4
        return AoCMath.LCM(cycles);
    }
}