using System.Text;
namespace AoC23;

public class StringUtilities
{
    // Transposes rows to cols and vice versa
    public static string[] TransposeRowsAndCols(string[] rowString)
    {
        StringBuilder columnBuilder = new StringBuilder();
        string[] columns = [];

        for (int columnIndex = 0; columnIndex < rowString[0].Length; columnIndex++)
        {
            columnBuilder.Clear();
            for (int rowIndex = 0; rowIndex < rowString.Length; rowIndex++)
            {
                columnBuilder.Append(rowString[rowIndex][columnIndex]);
            }

            columns = [.. columns.Append(columnBuilder.ToString())];
        }
        return columns;
    }

    public static string[] SplitLines(string input)
    {
        return input.Split(Environment.NewLine,
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    public static string[] SplitByEmptyLines(string input)
    {
        return input.Split(Environment.NewLine + Environment.NewLine,
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    public static string[] SplitWithTrim(string input, string? separator) =>
        input.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}