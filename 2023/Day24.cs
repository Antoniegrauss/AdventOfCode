using AoC23.Day8;

namespace AoC23.Day24;

public struct Bounds
{
    public double min;
    public double max;
}

public struct SolutionXY()
{
    public double? x = null;
    public double? y = null;

    public readonly bool WithinBounds(Bounds bounds)
    {
        if (x == null || y == null) return false;
        return x >= bounds.min &&
            x <= bounds.max &&
            y >= bounds.min &&
            y <= bounds.max;
    }
}

public class HailStone : IEquatable<HailStone>
{
    double x, y, z, vx, vy, vz;
    public int id = 0;

    static int currentID = 0;

    // Convert to the form y = ax + b
    // Solve for x = (b2 - b1) / (a1 - a2)
    // Substitute in y = ax + b to get y
    public SolutionXY IntersectionPointXY(HailStone other)
    {
        double a1 = vy / vx;
        double b1 = -x * vy / vx + y;

        double a2 = other.vy / other.vx;
        double b2 = -other.x * other.vy / other.vx + other.y;

        double div = a1 - a2;
        if (div == 0)
        {
            return new SolutionXY();
        }

        SolutionXY solution = new();
        solution.x = (b2 - b1) / div;
        solution.y = a1 * solution.x + b1;

        // Check whether solution lies in the past or future
        bool x1InFuture = (solution.x - x) / vx > 0;
        bool x2InFuture = (solution.x - other.x) / other.vx > 0;
        bool y1InFuture = (solution.y - y) / vy > 0;
        bool y2InFuture = (solution.y - other.y) / other.vy > 0;
        if (x1InFuture && y1InFuture
            && x2InFuture && y2InFuture)
        {
            return solution;
        }
        return new SolutionXY();
    }

    public static HailStone Parse(string input)
    {
        input = input.Replace('@', ',');
        string[] numbers = StringUtilities.SplitWithTrim(input, ",");
        currentID += 1;
        return new HailStone
        {
            x = long.Parse(numbers[0]),
            y = long.Parse(numbers[1]),
            z = long.Parse(numbers[2]),
            vx = long.Parse(numbers[3]),
            vy = long.Parse(numbers[4]),
            vz = long.Parse(numbers[5]),
            id = currentID
        };
    }

    public bool Equals(HailStone? other)
    {
        return id == other.id;
    }
}

public class Day24
{
    public static long Part1(string input, Bounds bounds)
    {
        List<HailStone> stones = Parse(input);
        var pairs = GetPairs(stones);
        return pairs.Where(pair => PairCollidesWithinBounds(pair, bounds)).Count();
    }

    public static long Part2(string input, Bounds bounds)
    {
        return -1;
    }

    private static List<HailStone> Parse(string input)
    {
        string[] stonesStr = StringUtilities.SplitLines(input);
        return [.. stonesStr.Select(s => HailStone.Parse(s))];
    }

    private static IEnumerable<Tuple<HailStone, HailStone>> GetPairs(
        List<HailStone> stones)
    {
        return from item1 in stones
               from item2 in stones
               where !item1.Equals(item2) && item1.id < item2.id
               select Tuple.Create(item1, item2);
    }

    private static bool PairCollidesWithinBounds(
        Tuple<HailStone, HailStone> pair,
        Bounds bounds)
    {
        return pair.Item1.IntersectionPointXY(pair.Item2).WithinBounds(bounds);
    }
}