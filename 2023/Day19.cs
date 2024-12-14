using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace AoC23.Day19;


public enum Comparison
{
    Less,
    Greater
}

public struct Evaluation
{
    public string target; // x, m, a, or s
    public Comparison comparator; // Less or greater than
    public int compareNumber;
    public string goTo;

    public Evaluation(string input)
    {
        target = input.Substring(0, 1);
        if (input.Substring(1, 1) == "<") comparator = Comparison.Less;
        else comparator = Comparison.Greater;
        string[] lastParts = input.Substring(2, input.Length - 2).Split(":");
        compareNumber = int.Parse(lastParts[0]);
        goTo = lastParts[1];
    }

    public MachinePartCombinations SplitRanges(ref MachinePartCombinations originalRanges)
    {
        int endOfFirstRange = compareNumber;
        // Put range end before or after number depending on comparison type
        if (comparator == Comparison.Less) endOfFirstRange -= 1;

        if (!originalRanges.NumberInRange(GetMachineField(), endOfFirstRange)) return null;

        var splitOffRange = originalRanges.Divide(GetMachineField(), endOfFirstRange, comparator);

        // Depending on comparator assign the new function to one of the ranges
        splitOffRange.previousFunction = splitOffRange.currentFunction;
        splitOffRange.currentFunction = goTo;
        originalRanges.previousFunction = originalRanges.currentFunction;

        return splitOffRange;
    }

    private MachineFields GetMachineField()
    {
        return target switch
        {
            "x" => MachineFields.X,
            "m" => MachineFields.M,
            "a" => MachineFields.A,
            "s" => MachineFields.S,
        };
    }
}

public struct MachineFunction
{
    public string id;
    public Evaluation[] evaluatorFunctions;
    // Category to sort into if all conditions fail
    public string defaultCategory;

    public string EvaluatePart(MachinePart machinePart)
    {
        foreach (Evaluation evalutorFunction in evaluatorFunctions)
        {
            (bool, string) evaluationResult = machinePart.DoEvaluation(evalutorFunction);
            if (evaluationResult.Item1) return evaluationResult.Item2;
        }
        return defaultCategory;
    }

    public List<MachinePartCombinations> SplitRange(MachinePartCombinations ranges)
    {
        string inFunction = ranges.currentFunction;
        List<MachinePartCombinations> newRanges = [];
        foreach (Evaluation evaluator in evaluatorFunctions)
        {
            MachinePartCombinations splitOff = evaluator.SplitRanges(ref ranges);
            if (splitOff != null) newRanges.Add(splitOff);
        }

        // Set original range (or what is left) to point to the default
        // And also add it to the list
        newRanges.Add(ranges);
        foreach (MachinePartCombinations range in newRanges)
        {
            if (range.currentFunction == inFunction)
            {
                range.previousFunction = range.currentFunction;
                range.currentFunction = defaultCategory;
                break;
            }
        }

        return newRanges;
    }

    public MachineFunction(string input)
    {
        string[] parts = input.Replace("}", "").Split("{");
        id = parts[0];
        parts = StringUtilities.SplitWithTrim(parts[1], ",");
        defaultCategory = parts[^1];

        evaluatorFunctions = [.. parts.Take(parts.Length - 1).Select(str => new Evaluation(str))];
    }

}

public record MachinePart
{
    public int x { get; }
    public int m { get; }
    public int a { get; }
    public int s { get; }

    public MachinePart(string input)
    {
        input = input.Replace("{", "").Replace("}", "");
        string[] parts = input.Split(",");
        x = int.Parse(parts[0].Substring(2));
        m = int.Parse(parts[1].Substring(2));
        a = int.Parse(parts[2].Substring(2));
        s = int.Parse(parts[3].Substring(2));
    }

    public (bool, string) DoEvaluation(Evaluation evaluator)
    {
        // Use reflection to find the field
        int machineNumber = (int)GetType().GetProperty(evaluator.target).GetValue(this);
        return evaluator.comparator switch
        {
            Comparison.Less => machineNumber < evaluator.compareNumber ? (true, evaluator.goTo) : (false, ""),
            Comparison.Greater => machineNumber > evaluator.compareNumber ? (true, evaluator.goTo) : (false, ""),
            _ => throw new NotImplementedException(),
        };
    }

    public int Sum()
    {
        return x + m + a + s;
    }
}

public class Day19
{
    public static long Part1(string input)
    {
        string[] parts = StringUtilities.SplitByEmptyLines(input);
        List<MachineFunction> machineFunctions = [.. StringUtilities.SplitLines(parts[0]).Select(str => new MachineFunction(str))];
        List<MachinePart> machineParts = [.. StringUtilities.SplitLines(parts[1]).Select(str => new MachinePart(str))];
        Dictionary<string, MachineFunction> functionFinder = new();
        foreach (MachineFunction function in machineFunctions)
        {
            functionFinder[function.id] = function;
        }

        List<MachinePart> allA = [];

        foreach (MachinePart machine in machineParts)
        {
            string function = "in";
            while (functionFinder.ContainsKey(function))
            {
                function = functionFinder[function].EvaluatePart(machine);
            }
            if (function == "A") allA.Add(machine);
        }

        return allA.Sum(part => part.Sum());
    }

    public static long Part2(string input)
    {
        string[] parts = StringUtilities.SplitByEmptyLines(input);
        List<MachineFunction> machineFunctions = [.. StringUtilities.SplitLines(parts[0]).Select(str => new MachineFunction(str))];
        Dictionary<string, MachineFunction> functionFinder = new();
        foreach (MachineFunction function in machineFunctions)
        {
            functionFinder[function.id] = function;
        }

        List<MachinePartCombinations> accepted = [];
        List<MachinePartCombinations> rejected = [];
        var toCheckQueue = new Queue<MachinePartCombinations>([new MachinePartCombinations{
            currentFunction="in",
            xOptions= new Range{start=1, end=4000},
            mOptions= new Range{start=1, end=4000},
            aOptions= new Range{start=1, end=4000},
            sOptions= new Range{start=1, end=4000},
        }]);

        while (toCheckQueue.Count > 0)
        {
            ProcessQueueItem(functionFinder, accepted, rejected, toCheckQueue);
        }

        // Sanity check that we did not lose any ranges
        List<MachinePartCombinations> allRanges = [.. accepted, .. rejected];
        if (!MachinePartCombinations.FormCompleteRangeTogether(allRanges)) throw new Exception("Lost combinations somewhere");

        // X, M, A, S can be 1..4000
        // How many combinations can these values have and go to A in the end?
        long sum = 0;
        foreach (MachinePartCombinations accept in accepted)
        {
            sum += accept.TotalCombinations();
        }
        return sum;
    }

    private static void ProcessQueueItem(
        Dictionary<string, MachineFunction> functionFinder,
        List<MachinePartCombinations> accepted,
        List<MachinePartCombinations> rejected,
        Queue<MachinePartCombinations> toCheckQueue)
    {
        MachinePartCombinations rangesToCheck = toCheckQueue.Dequeue();
        if (!functionFinder.ContainsKey(rangesToCheck.currentFunction)) throw new Exception("Function does not exist??");

        long currentScore = rangesToCheck.TotalCombinations();

        var newRanges = functionFinder[rangesToCheck.currentFunction].SplitRange(rangesToCheck);
        if (newRanges.Sum(range => range.TotalCombinations()) != currentScore) throw new Exception("Lost combinations");

        foreach (var newRange in newRanges)
        {
            if (newRange.currentFunction == "A")
            {
                accepted.Add(newRange);
                continue;
            }
            else if (newRange.currentFunction == "R")
            {
                rejected.Add(newRange);
                continue;
            }

            toCheckQueue.Enqueue(newRange);
        }
    }
}

public enum MachineFields
{
    X,
    M,
    A,
    S,
}

public class MachinePartCombinations
{
    public string currentFunction;
    public string previousFunction;
    public Range xOptions;
    public Range mOptions;
    public Range aOptions;
    public Range sOptions;

    public static bool FormCompleteRangeTogether(List<MachinePartCombinations> ranges)
    {
        long totalOptions = (long)4000 * (long)4000 * (long)4000 * (long)4000;
        long rangesSum = ranges.Sum(range => range.TotalCombinations());
        return totalOptions == rangesSum;
    }

    private bool IsFullRange()
    {
        return xOptions.start == 1 && xOptions.end == 4000 &&
        mOptions.start == 1 && mOptions.end == 4000 &&
        aOptions.start == 1 && aOptions.end == 4000 &&
        sOptions.start == 1 && sOptions.end == 4000;
    }

    public bool NumberInRange(MachineFields field, int firstCorrectId)
    {
        return field switch
        {
            MachineFields.X => xOptions.IsWithinRange(firstCorrectId),
            MachineFields.M => mOptions.IsWithinRange(firstCorrectId),
            MachineFields.A => aOptions.IsWithinRange(firstCorrectId),
            MachineFields.S => sOptions.IsWithinRange(firstCorrectId),
        };
    }

    public MachinePartCombinations Divide(MachineFields field, int endFirstRange, Comparison comparator)
    {
        MachinePartCombinations newCombinations = new();
        newCombinations.xOptions = xOptions;
        newCombinations.mOptions = mOptions;
        newCombinations.aOptions = aOptions;
        newCombinations.sOptions = sOptions;
        newCombinations.currentFunction = currentFunction;
        newCombinations.previousFunction = currentFunction;

        (Range, Range) cutRange;
        switch (field)
        {
            case MachineFields.X:
                cutRange = xOptions.Cut(endFirstRange);
                if (comparator == Comparison.Greater)
                {
                    xOptions = cutRange.Item1;
                    newCombinations.xOptions = cutRange.Item2;
                    break;
                }
                xOptions = cutRange.Item2;
                newCombinations.xOptions = cutRange.Item1;
                break;
            case MachineFields.M:
                cutRange = mOptions.Cut(endFirstRange);
                if (comparator == Comparison.Greater)
                {
                    mOptions = cutRange.Item1;
                    newCombinations.mOptions = cutRange.Item2;
                    break;
                }
                mOptions = cutRange.Item2;
                newCombinations.mOptions = cutRange.Item1;
                break;
            case MachineFields.A:
                cutRange = aOptions.Cut(endFirstRange);
                if (comparator == Comparison.Greater)
                {
                    aOptions = cutRange.Item1;
                    newCombinations.aOptions = cutRange.Item2;
                    break;
                }
                aOptions = cutRange.Item2;
                newCombinations.aOptions = cutRange.Item1;
                break;
            case MachineFields.S:
                cutRange = sOptions.Cut(endFirstRange);
                if (comparator == Comparison.Greater)
                {
                    sOptions = cutRange.Item1;
                    newCombinations.sOptions = cutRange.Item2;
                    break;
                }
                sOptions = cutRange.Item2;
                newCombinations.sOptions = cutRange.Item1;
                break;
        }

        return newCombinations;
    }

    public long TotalCombinations()
    {
        return xOptions.Length() * mOptions.Length() * aOptions.Length() * sOptions.Length();
    }

    public MachinePartCombinations Combine(MachinePartCombinations other)
    {
        return new MachinePartCombinations
        {
            currentFunction = currentFunction,
            previousFunction = previousFunction,

            xOptions = aOptions.Combine(other.xOptions),
            mOptions = aOptions.Combine(other.mOptions),
            aOptions = aOptions.Combine(other.aOptions),
            sOptions = aOptions.Combine(other.sOptions),
        };
    }
}

public record Range
{
    public long start;
    public long end;

    // Assume this number is between start and end - 1
    public (Range, Range) Cut(long endFirstRange)
    {
        return (new Range { start = start, end = endFirstRange }, new Range { start = endFirstRange + 1, end = end });
    }

    public long Length()
    {
        return end - start + 1;
    }

    public bool IsWithinRange(long number)
    {
        return number >= start && number <= end;
    }

    public Range Combine(Range other)
    {
        return new Range
        {
            start = Math.Min(start, other.start),
            end = Math.Max(end, other.end)
        };
    }
}
