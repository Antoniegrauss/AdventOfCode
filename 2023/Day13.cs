using System.Text;
namespace AoC23.Day13;

public class Day13
{

    public static long Part1(string input)
    {
        string[] lavaFields = input.Split(Environment.NewLine + Environment.NewLine,
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        long sum = 0;
        foreach (string lavaField in lavaFields)
        {
            sum += ScoreReflectionLine(lavaField.Split(Environment.NewLine,
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }

        return sum;
    }

    public static long Part2(string input)
    {
        string[] lavaFields = input.Split(Environment.NewLine + Environment.NewLine,
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        long sum = 0;
        foreach (string lavaField in lavaFields)
        {
            sum += ScoreSmudgedReflectionLine(lavaField.Split(Environment.NewLine,
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }
        return sum;
    }

    public static long ScoreSmudgedReflectionLine(string[] rows)
    {
        long? reflectionRow = FindReflectionRowWidhtSmudges(rows);
        if (reflectionRow != null)
        {
            return 100 * reflectionRow.Value;
        }
        return FindReflectionRowWidhtSmudges(StringUtilities.TransposeRowsAndCols(rows)).Value;
    }

    private static long ScoreReflectionLine(string[] rows)
    {
        long? reflectionRow = FindReflectionRow(rows);
        if (reflectionRow != null)
        {
            return 100 * reflectionRow.Value;
        }
        return FindReflectionCol(rows).Value;
    }


    private static long? FindReflectionCol(string[] rows)
    {
        return FindReflectionRow(StringUtilities.TransposeRowsAndCols(rows));
    }

    private static long? FindReflectionRow(string[] rows)
    {
        for (int mirrorRowIndex = 1; mirrorRowIndex < rows.Length; mirrorRowIndex++)
        {
            if (IsMirroredHorizontallyAtRow(rows, mirrorRowIndex))
            {
                return mirrorRowIndex;
            }
        }
        return null;
    }

    private static bool IsMirroredHorizontallyAtRow(string[] lavaField, int mirrorRowIndex)
    {
        string[] rowsMiddleToUp = [.. lavaField.Take(mirrorRowIndex)];
        rowsMiddleToUp = [.. rowsMiddleToUp.Reverse()];
        string[] rowsMiddleToDown = [.. lavaField.Skip(mirrorRowIndex)];
        foreach ((string, string) pair in rowsMiddleToUp.Zip(rowsMiddleToDown))
        {
            if (pair.Item1 != pair.Item2)
            {
                return false;
            }
        }
        return true;
    }

    private static long? FindReflectionRowWidhtSmudges(string[] rows)
    {
        for (int mirrorRowIndex = 1; mirrorRowIndex < rows.Length; mirrorRowIndex++)
        {
            if (IsMirroredHorizontallyAtRowWithSmudge(rows, mirrorRowIndex))
            {
                return mirrorRowIndex;
            }
        }
        return null;
    }

    private static bool IsMirroredHorizontallyAtRowWithSmudge(string[] lavaField, int mirrorRowIndex)
    {
        string[] rowsMiddleToUp = [.. lavaField.Take(mirrorRowIndex)];
        rowsMiddleToUp = [.. rowsMiddleToUp.Reverse()];
        string[] rowsMiddleToDown = [.. lavaField.Skip(mirrorRowIndex)];

        int smudges = 0;
        const int SMUDGE_THRESHOLD = 1;
        foreach ((string, string) pair in rowsMiddleToUp.Zip(rowsMiddleToDown))
        {
            if (pair.Item1 != pair.Item2)
            {
                foreach ((char, char) location in pair.Item1.Zip(pair.Item2))
                {
                    if (location.Item1 != location.Item2)
                    {
                        smudges += 1;
                        if (smudges > SMUDGE_THRESHOLD) return false;
                    }
                }
            }
        }
        return smudges == 1;
    }
}