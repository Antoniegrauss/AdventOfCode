namespace AoC23.Day21;

public class ReachabilityChecker(Grid2D grid, Coordinate start)
{
    private readonly Grid2D _grid = grid;
    private readonly Coordinate _start = start;

    public Dictionary<Coordinate, int> CalculateFirstVisits()
    {
        HashSet<Coordinate> visited = [_start];
        Dictionary<Coordinate, int> firstVisit = new()
        {
            { _start, 0 }
        };

        // Start with only toCheck, then expand
        List<Coordinate> toCheck = [_start];
        foreach (int step in Enumerable.Range(0, 500))
        {
            toCheck = ExpandAStepNoDoubles(ref visited, toCheck);
            foreach (Coordinate newlyReached in toCheck)
            {
                firstVisit.Add(newlyReached, step);
            }
        }
        return firstVisit;
    }

    private List<Coordinate> ExpandAStepNoDoubles(ref HashSet<Coordinate> visited, List<Coordinate> toCheck)
    {
        HashSet<Coordinate> newlyReached = new();

        foreach (Coordinate toCheckCoord in toCheck)
        {
            var neighbours = toCheckCoord.GetNeighboursAdjacent();
            foreach (Coordinate neighbour in neighbours)
            {
                if (visited.Contains(neighbour)) continue;
                if (!_grid.IsInsideGrid(neighbour)) continue;
                if (_grid.GetAtCoordinate(neighbour) == '#') continue;
                newlyReached.Add(neighbour);
                visited.Add(neighbour);
            }
        }
        toCheck = newlyReached.ToList();
        return toCheck;
    }

    public long HowManyReachableInXSteps(int steps)
    {
        // Start with only toCheck, then expand
        List<Coordinate> toCheck = [_start];
        foreach (int _ in Enumerable.Range(0, steps))
        {
            toCheck = ExpandAStep(toCheck);
        }
        return toCheck.Count;
    }

    private List<Coordinate> ExpandAStep(List<Coordinate> toCheck)
    {
        HashSet<Coordinate> newlyReached = new();

        foreach (Coordinate toCheckCoord in toCheck)
        {
            var neighbours = toCheckCoord.GetNeighboursAdjacent();
            foreach (Coordinate neighbour in neighbours)
            {
                if (!_grid.IsInsideGrid(neighbour)) continue;
                if (_grid.GetAtCoordinate(neighbour) == '#') continue;
                newlyReached.Add(neighbour);
            }
        }
        toCheck = newlyReached.ToList();
        return toCheck;
    }
}

public class Day21
{
    public static long Part1(string input, int steps)
    {
        Grid2D grid = Grid2D.Parse(input);
        Coordinate? start = grid.FindSymbol('S');

        return new ReachabilityChecker(grid, start).HowManyReachableInXSteps(steps);
    }

    // Implemented geometric solution from:
    //  https://github.com/villuna/aoc23/wiki/A-Geometric-solution-to-advent-of-code-2023,-day-21
    public static long Part2(string input, int steps)
    {
        Grid2D grid = Grid2D.Parse(input);
        var obstacles = grid.FindAllOfSymbol('#');
        Coordinate? start = grid.FindSymbol('S');

        Dictionary<Coordinate, int> firstVisits = new ReachabilityChecker(grid, start).CalculateFirstVisits();

        long evenCorners = firstVisits.Values.Where(x => x % 2 == 0 && x > 65).Count();
        long oddCorners = firstVisits.Values.Where(x => x % 2 == 1 && x > 65).Count();

        long evenFull = firstVisits.Values.Where(x => x % 2 == 0).Count();
        long oddFull = firstVisits.Values.Where(x => x % 2 == 1).Count();

        if (firstVisits.Count != evenFull + oddFull) throw new Exception("Missing visited squares");

        long numGridsVisited = (steps - (grid.Height() / 2)) / grid.Width();
        if (numGridsVisited != 202300) throw new Exception("Should visit exactly 202300 grids");

        return (numGridsVisited + 1) * (numGridsVisited + 1) * oddFull +
            (numGridsVisited * numGridsVisited) * evenFull -
            (numGridsVisited + 1) * oddCorners +
            (numGridsVisited) * evenCorners;
    }
}