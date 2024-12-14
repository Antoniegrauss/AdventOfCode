using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace AoC23.Day18;

public struct DigInstruction
{
    public Direction2D _direction { get; private set; }
    public int _amount { get; private set; }
    public string _colorCode { get; private set; }

    public static DigInstruction Parse(string line)
    {
        string[] parts = StringUtilities.SplitWithTrim(line, " ");

        DigInstruction instruction = new();
        instruction._direction = ParseDirection(parts[0]);
        instruction._amount = int.Parse(parts[1]);
        // Color code contains ()
        instruction._colorCode = parts[2].Substring(1, parts[2].Length - 2);

        return instruction;
    }

    public static DigInstruction ParsePart2(string line)
    {
        string[] parts = StringUtilities.SplitWithTrim(line, " ");

        DigInstruction instruction = new();

        // Color code contains ()
        instruction._colorCode = parts[2].Substring(1, parts[2].Length - 2);
        instruction._direction = instruction._colorCode[^1] switch
        {
            '0' => Direction2D.Right,
            '1' => Direction2D.Down,
            '2' => Direction2D.Left,
            '3' => Direction2D.Up,
            _ => throw new Exception("Last digit should be 0, 1, 2, 3")
        };
        instruction._amount = int.Parse(instruction._colorCode.Substring(1, 5), System.Globalization.NumberStyles.HexNumber);

        return instruction;
    }

    public static Direction2D ParseDirection(string input)
    {
        return input switch
        {
            "R" => Direction2D.Right,
            "L" => Direction2D.Left,
            "U" => Direction2D.Up,
            "D" => Direction2D.Down,
            _ => throw new Exception("Invalid direction char, should be in 'RLUD'")
        };
    }

    public readonly void AddStepsToPath(ref Path path)
    {
        for (int i = 0; i < _amount; i++)
        {
            path.AddStep(
                        new Step(path.LastCoord().Move(_direction),
                        _direction));
        }
    }

    public readonly void AddEdge(ref Polygon polygon)
    {
        polygon.AddEdge(polygon.LastCoord().Move(_direction, _amount));
    }
}

public class Day18
{
    public static long Part1(string input)
    {
        List<DigInstruction> digInstructions = [.. StringUtilities.SplitLines(input).Select(s => DigInstruction.Parse(s))];
        Path path = new();
        foreach (DigInstruction instruction in digInstructions) instruction.AddStepsToPath(ref path);

        HashSet<Coordinate> nextToPath = path.FindCoordinatesNextToPath(Direction2D.Right);
        HashSet<Coordinate> coloredBorders = path.FindCoordinatesNextToPath(Direction2D.Left);

        Area area = new(nextToPath);
        area.GrowWithinBorder([.. path.AllCoords()]);
        area.Combine(new Area([.. path.AllCoords()]));

        return area._coords.Count;
    }

    public static long Part2(string input)
    {
        List<DigInstruction> digInstructions = [.. StringUtilities.SplitLines(input).Select(s => DigInstruction.ParsePart2(s))];
        Polygon path = new();
        foreach (DigInstruction instruction in digInstructions) instruction.AddEdge(ref path);

        // Use triangle formula to calculate the total area of the polygon
        long sum = 0;
        foreach (Edge edge in path._edges)
        {
            long startX = edge._start.x;
            long startY = edge._start.y;
            long endX = edge._end.x;
            long endY = edge._end.y;
            long areaSize = startX * endY - endX * startY;
            sum += areaSize;
        }
        sum /= 2;

        long totalArea = Math.Abs(sum) - (path.BorderLength() / 2) + path.BorderLength() + 1;
        return totalArea;
    }
}
