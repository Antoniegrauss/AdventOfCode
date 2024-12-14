namespace AoC23;

public class Cycle
{
    public int startIndex;
    public List<long> cycle;

    public Cycle(int cycleStart, List<long> cycle)
    {
        this.cycle = cycle;
        startIndex = cycleStart;
    }

    public static Cycle FindCycle(List<long> series)
    {
        const int MIN_CYCLE_LENGTH = 3;
        for (int cycleLength = MIN_CYCLE_LENGTH; cycleLength < series.Count / 2; cycleLength++)
        {
            for (int cycleStart = 0; cycleStart < series.Count / 2; cycleStart++)
            {
                List<long> cycle = [.. series.Skip(cycleStart).Take(cycleLength)];
                List<long> cycleEqualityCheck = [.. series.Skip(cycleStart + cycleLength).Take(cycleLength)];

                if (ListEquals(cycle, cycleEqualityCheck))
                {
                    return new Cycle(cycleStart, cycle);
                }
            }
        }
        return null;
    }

    public static bool ListEquals<T>(List<T> a, List<T> b)
    {
        if (a.Count() != b.Count()) return false;
        foreach (var pair in a.Zip(b))
        {
            if (!pair.Item1.Equals(pair.Item2)) return false;
        }
        return true;
    }
}
