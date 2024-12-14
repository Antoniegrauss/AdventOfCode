using System.IO.Pipelines;

namespace AoC23.Day10;

public enum PipeCell
{
    Empty = '.',
    Start = 'S',
    LeftUp = 'J',
    RightUp = 'L',
    RightDown = 'F',
    LeftDown = '7',
    Horizontal = '-',
    Vertical = '|'
}


public class PipeGrid(List<string> rows)
{
    private static char START = 'S';
    readonly List<string> _rows = rows;

    public static PipeGrid Parse(string input)
    {
        List<string> rows = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        return new PipeGrid(rows);
    }

    public int Height() => _rows.Count;
    public int Width() => _rows[0].Length;

    public char GetAtCoordinate(Coordinate coord)
    {
        if (!IsInsideGrid(coord))
        {
            return (char)PipeCell.Empty;
        }
        return rows[coord.y][coord.x];
    }

    public bool IsInsideGrid(Coordinate coord)
    {
        return coord.x >= 0 && coord.x < Width() && coord.y >= 0 && coord.y < Height();
    }

    public bool IsAtBorder(Coordinate coord)
    {
        return coord.x == 0 || coord.x == Width() - 1 || coord.y == 0 || coord.y == Height() - 1;
    }

    public Coordinate FindStart()
    {
        for (int y = 0; y < _rows.Count; y++)
        {
            for (int x = 0; x < _rows[y].Length; x++)
            {
                if (_rows[y][x] == START)
                {
                    return new Coordinate { x = x, y = y };
                }
            }
        }

        throw new Exception("Start not found");
    }

    // Check for each direction around start if any pipes connect to it
    public List<Direction2D> GetStartDirections(Coordinate start)
    {
        List<Direction2D> possibleStartDirections = new();

        List<Direction2D> neighbours = new([Direction2D.Up, Direction2D.Down, Direction2D.Left, Direction2D.Right]);
        foreach (Direction2D direction in neighbours)
        {
            Coordinate neighbourCoord = start.Move(direction);
            PipeCell neighbourCell = (PipeCell)GetAtCoordinate(neighbourCoord);
            if (neighbourCell == PipeCell.Empty)
            {
                continue;
            }
            List<Direction2D> neighbourConnections = connections(neighbourCell);
            if (neighbourConnections.Contains(Direction2DExtensions.Opposite(direction)))
            {
                possibleStartDirections.Add(direction);
            }
        }

        return possibleStartDirections;
    }

    private List<Direction2D> connections(PipeCell cell)
    {
        return cell switch
        {
            PipeCell.LeftUp => new List<Direction2D> { Direction2D.Left, Direction2D.Up },
            PipeCell.RightUp => new List<Direction2D> { Direction2D.Right, Direction2D.Up },
            PipeCell.RightDown => new List<Direction2D> { Direction2D.Right, Direction2D.Down },
            PipeCell.LeftDown => new List<Direction2D> { Direction2D.Left, Direction2D.Down },
            PipeCell.Horizontal => new List<Direction2D> { Direction2D.Left, Direction2D.Right },
            PipeCell.Vertical => new List<Direction2D> { Direction2D.Up, Direction2D.Down },
            _ => throw new Exception("Invalid pipe cell"),
        };
    }

    public int FindLoopLength()
    {
        List<Step> paths = GetStepsFromStart();

        int steps = 1;
        List<PipeCell> currentEnds = paths.Select(p => (PipeCell)GetAtCoordinate(p.currentPosition)).ToList();

        // If paths meet, we have found the loop
        while (paths.Select(p => p.currentPosition).Distinct().Count() == 2)
        {
            paths = paths.Select((p, index) => p.TakeStep(currentEnds[index])).ToList();
            currentEnds = paths.Select(p => (PipeCell)GetAtCoordinate(p.currentPosition)).ToList();
            steps++;
        }

        return steps;
    }

    private List<Step> GetStepsFromStart()
    {
        Coordinate start = FindStart();

        List<Direction2D> startDirections = GetStartDirections(start);
        // Should be 2 pipes connecting to start
        if (startDirections.Count != 2)
        {
            throw new Exception("Invalid number of start directions");
        }

        List<Step> paths = new([
            new Step
            {
                currentPosition = start.Move(startDirections[0]),
                comingFrom = Direction2DExtensions.Opposite(startDirections[0])
            },
            new Step
            {
                currentPosition = start.Move(startDirections[1]),
                comingFrom = Direction2DExtensions.Opposite(startDirections[1])
            },
        ]);
        return paths;
    }

    public PipePath CalculateLoopPath()
    {
        PipePath loopPath = new();

        List<Step> paths = GetStepsFromStart();
        List<PipeCell> currentEnds = paths.Select(p => (PipeCell)GetAtCoordinate(p.currentPosition)).ToList();

        // Add starting positions
        loopPath.path.Add(FindStart());

        List<HashSet<Coordinate>> individualPaths = new([
            new HashSet<Coordinate> { paths[0].currentPosition },
            new HashSet<Coordinate> { paths[1].currentPosition }
        ]);
        // If paths meet, we have found the loop
        while (paths.Select(p => p.currentPosition).Distinct().Count() == 2)
        {
            paths = paths.Select((p, index) => p.TakeStep(currentEnds[index])).ToList();
            currentEnds = [.. paths.Select(p => (PipeCell)GetAtCoordinate(p.currentPosition))];

            // Hardcoded way to add coordinates to list of paths
            individualPaths[0].Add(paths[0].currentPosition);
            individualPaths[1].Add(paths[1].currentPosition);
        }

        // Now we must make this one continuous path
        loopPath.path = [.. loopPath.path.Union(individualPaths[0])];
        loopPath.path = [.. loopPath.path.Union(individualPaths[1].Reverse())];

        return loopPath;
    }

    private struct Step
    {
        public Coordinate currentPosition;
        public Direction2D comingFrom;

        public Step TakeStep(PipeCell cell)
        {
            Step newStep = new();
            Direction2D goingTo = cell switch
            {
                PipeCell.LeftUp => comingFrom switch
                {
                    Direction2D.Left => Direction2D.Up,
                    Direction2D.Up => Direction2D.Left,
                    _ => throw new Exception("Invalid coming from direction for LeftUp pipe")
                },
                PipeCell.RightUp => comingFrom switch
                {
                    Direction2D.Right => Direction2D.Up,
                    Direction2D.Up => Direction2D.Right,
                    _ => throw new Exception("Invalid coming from direction for RightUp pipe")
                },
                PipeCell.RightDown => comingFrom switch
                {
                    Direction2D.Right => Direction2D.Down,
                    Direction2D.Down => Direction2D.Right,
                    _ => throw new Exception("Invalid coming from direction for RightDown pipe")
                },
                PipeCell.LeftDown => comingFrom switch
                {
                    Direction2D.Left => Direction2D.Down,
                    Direction2D.Down => Direction2D.Left,
                    _ => throw new Exception("Invalid coming from direction for LeftDown pipe")
                },
                PipeCell.Horizontal => comingFrom switch
                {
                    Direction2D.Left => Direction2D.Right,
                    Direction2D.Right => Direction2D.Left,
                    _ => throw new Exception("Invalid coming from direction for Horizontal pipe")
                },
                PipeCell.Vertical => comingFrom switch
                {
                    Direction2D.Up => Direction2D.Down,
                    Direction2D.Down => Direction2D.Up,
                    _ => throw new Exception("Invalid coming from direction for Vertical pipe")
                },
                _ => throw new Exception("Invalid pipe cell")
            };

            newStep.currentPosition = currentPosition.Move(goingTo);
            newStep.comingFrom = Direction2DExtensions.Opposite(goingTo);

            return newStep;
        }
    }
}

public class PipePath
{
    public HashSet<Coordinate> path = new();
}

public class Day10
{
    public static long Part1(string input)
    {
        PipeGrid grid = PipeGrid.Parse(input);
        return grid.FindLoopLength();
    }

    public static long Part2(string input)
    {
        PipeGrid grid = PipeGrid.Parse(input);
        PipePath loopPath = grid.CalculateLoopPath();

        // Add all coordinates from the grid
        HashSet<Coordinate> unassignedCoords = new();
        for (int y = 0; y < grid.Height(); y++)
        {
            for (int x = 0; x < grid.Width(); x++)
            {
                Coordinate coord = new() { x = x, y = y };
                unassignedCoords.Add(coord);
            }
        }

        // Debug to check total coords
        int totalCoords = unassignedCoords.Count;

        // Remove path coords from the total set
        unassignedCoords = [.. unassignedCoords.Except(loopPath.path)];

        // Find free coordinates at the edges
        HashSet<Coordinate> freeCoords = [.. unassignedCoords.Where(c => grid.IsAtBorder(c))];
        unassignedCoords = [.. unassignedCoords.Except(freeCoords)];

        (unassignedCoords, freeCoords) = ExpandFreeCoords(unassignedCoords, freeCoords);

        // Check whether the left or right side of the loop encloses the remaining coordinates
        HashSet<Coordinate> leftOfPath = new();
        HashSet<Coordinate> rightOfPath = new();
        foreach (var pathCoords in loopPath.path.Zip(loopPath.path.Skip(1)))
        {
            leftOfPath.Add(pathCoords.First.Move(
                pathCoords.First.WhichDirectionIsLeftOfLine(pathCoords.Second)));
            rightOfPath.Add(pathCoords.First.Move(
                pathCoords.First.WhichDirectionIsRightOfLine(pathCoords.Second)));
        }

        // Remove the path coords from left/right side sets
        leftOfPath = [.. leftOfPath.Except(loopPath.path)];
        rightOfPath = [.. rightOfPath.Except(loopPath.path)];

        // Either some coords on the left or right side of the path must be free coords
        if (!leftOfPath.Intersect(freeCoords).Any())
        {
            freeCoords = [.. rightOfPath.Union(freeCoords)];
        }
        else
        {
            freeCoords = [.. leftOfPath.Union(freeCoords)];
        }

        (unassignedCoords, freeCoords) = ExpandFreeCoords(unassignedCoords, freeCoords);

        return totalCoords - freeCoords.Count - loopPath.path.Count;
    }

    private static (HashSet<Coordinate>, HashSet<Coordinate>) ExpandFreeCoords(
        HashSet<Coordinate> unassignedCoords, HashSet<Coordinate> coordsToExpand)
    {
        // Expand free coordinates iteratively untill no more free neighbours exist
        int numFreeCoords;
        do
        {
            numFreeCoords = coordsToExpand.Count;
            HashSet<Coordinate> newFreeCoords = [
                .. unassignedCoords.Where(c => coordsToExpand.Any(f => f.IsNeighbour(c))).Except(coordsToExpand)];
            coordsToExpand.UnionWith(newFreeCoords);
        }
        while (coordsToExpand.Count > numFreeCoords);

        return ([.. unassignedCoords.Except(coordsToExpand)], coordsToExpand);
    }
}