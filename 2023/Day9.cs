namespace AoC23.Day9;

public struct Pyramid
{
    static public Pyramid Parse(string input)
    {
        Pyramid pyramid = new();
        pyramid.levels = new List<List<int>>();
        List<int> firstLevel = input.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => int.Parse(s)).ToList();
        pyramid.levels.Add(firstLevel);

        return pyramid;
    }
    public List<List<int>> levels;

    public int PredictPreviousValue()
    {
        // Generate levels until we have a level with all zeroes
        while (!LastLevelAllZeroes())
        {
            GenerateNextLevel();
        }

        return CalucatePreviousValueFirstRow();
    }

    public int PredictNextValue()
    {
        // Generate levels until we have a level with all zeroes
        while (!LastLevelAllZeroes())
        {
            GenerateNextLevel();
        }

        // Then add one row to the pyramid by:
        // Adding a zero to the highest level
        // Backwards engineer the previous values
        return CalculateNextValueFirstRow();
    }

    private readonly bool LastLevelAllZeroes()
    {
        return levels[^1].All(v => v == 0);
    }

    private int CalculateNextValueFirstRow()
    {
        var reversedLevels = levels;
        reversedLevels.Reverse();
        int lastValuePreviousLevel = 0;
        foreach (int lastInLevel in reversedLevels.Skip(1).Select(level => level[^1]))
        {
            lastValuePreviousLevel += lastInLevel;
        }
        return lastValuePreviousLevel;
    }

    private int CalucatePreviousValueFirstRow()
    {
        var reversedLevels = levels;
        reversedLevels.Reverse();
        int firstValuePreviousLevel = 0;
        foreach (int lastInLevel in reversedLevels.Skip(1).Select(level => level[0]))
        {
            firstValuePreviousLevel = lastInLevel - firstValuePreviousLevel;
        }
        return firstValuePreviousLevel;
    }

    private void GenerateNextLevel()
    {
        List<int> nextLevel = new();
        foreach (var pair in levels[^1].Zip(levels[^1].Skip(1)))
        {
            nextLevel.Add(pair.Second - pair.First);
        }
        levels.Add(nextLevel);
    }
}

public class Day9
{
    public static long Part1(string input)
    {
        List<Pyramid> pyramids = ParsePyramids(input);

        return pyramids.Sum(p => p.PredictNextValue());
    }

    private static List<Pyramid> ParsePyramids(string input)
    {
        List<Pyramid> pyramids = new();
        foreach (string pyramidString in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            pyramids.Add(Pyramid.Parse(pyramidString));
        }

        return pyramids;
    }

    public static long Part2(string input)
    {
        List<Pyramid> pyramids = ParsePyramids(input);

        return pyramids.Sum(p => p.PredictPreviousValue()); // Placeholder
    }
}