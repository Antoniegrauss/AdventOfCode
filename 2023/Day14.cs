using System.Drawing;
using System.Security.Authentication.ExtendedProtection;
using Xunit.Sdk;

namespace AoC23.Day14;

public class Day14
{
    // Rolls all boulder left
    private static string RollAllLeft(string input)
    {
        string temp = input;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == 'O')
            {
                temp = RollLeft(temp, i);
            }
        }
        return temp;
    }

    private static string RollAllRight(string input)
    {
        string reverseRow = new(input.Reverse().ToArray());
        return new(RollAllLeft(reverseRow).Reverse().ToArray());
    }

    // Rolls a single boulder left
    private static string RollLeft(string input, int index)
    {
        // If we cannot roll further, return original
        if (index == 0)
        {
            return input;
        }
        else if (input[index - 1] != '.')
        {
            return input;
        }

        // Add empty character + characters after rolling (will remain unchanged)
        string changed = "." + new string(input.Skip(index + 1).ToArray());
        for (int i = index - 1; i > 0; i--)
        {
            if (input[i - 1] != '.')
            {
                changed = "O" + changed;
                string result = new string(input.Take(i).ToArray()) + changed;
                return result;
            }
            changed = input[i] + changed;
        }

        // If we got to index 1, boulder must be at location 0
        return "O" + changed;
    }

    private static long SumProductBouldersFromLeftSide(string input)
    {
        int sum = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == 'O')
            {
                // Add distance of rock to the end of the row/col
                sum += input.Length - i;
            }
        }
        return sum;
    }

    public static long Part1(string input)
    {
        string[] rows = StringUtilities.SplitLines(input);
        string[] cols = StringUtilities.TransposeRowsAndCols(rows);

        string[] colsAfterRollLeft = [.. cols.Select(c => RollAllLeft(c))];
        return colsAfterRollLeft.Sum(c => SumProductBouldersFromLeftSide(c));
    }

    public static string[] RollCycle(string[] rows)
    {
        // North
        string[] cols = StringUtilities.TransposeRowsAndCols(rows);
        string[] colsAfterRollLeft = [.. cols.Select(c => RollAllLeft(c))];

        // West
        rows = StringUtilities.TransposeRowsAndCols(colsAfterRollLeft);
        rows = [.. rows.Select(r => RollAllLeft(r))];

        // South
        cols = StringUtilities.TransposeRowsAndCols(rows);
        cols = [.. cols.Select(c => RollAllRight(c))];

        // East
        rows = StringUtilities.TransposeRowsAndCols(cols);
        rows = [.. rows.Select(r => RollAllRight(r))];

        return rows;
    }

    private static long ScoreRows(string[] rows)
    {
        string[] cols = StringUtilities.TransposeRowsAndCols(rows);
        return cols.Sum(c => SumProductBouldersFromLeftSide(c));
    }

    public static long Part2(string input)
    {
        // Question is what is the score (stress on north beams) after 1_000_000_000 cycles
        string[] rows = StringUtilities.SplitLines(input);
        List<long> scores = new();
        // Add score for cycle 0
        scores.Add(ScoreRows(rows));

        long targetCycles = 1_000_000_000;

        while (true)
        {
            rows = RollCycle(rows);
            scores.Add(ScoreRows(rows));

            if (scores.Count() < 5) continue;
            if (Cycle.FindCycle(scores) != null)
            {
                Cycle cycle = Cycle.FindCycle(scores);
                int scoreIndex = (int)(targetCycles - cycle.startIndex) % cycle.cycle.Count;
                return cycle.cycle[scoreIndex];
            }
        }
    }
}