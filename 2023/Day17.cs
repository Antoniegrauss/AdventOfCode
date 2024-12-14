namespace AoC23.Day17;

public struct PathNode : IComparable<PathNode>, IEquatable<PathNode>
{
    private AoC23.PathNode _lastNode;
    public int _turnedXMovesAgo { get; } = 0;

    public PathNode(PathNode other)
    {
        _lastNode = other._lastNode;
        _turnedXMovesAgo = other._turnedXMovesAgo;
    }

    public override int GetHashCode() => HashCode.Combine(_lastNode, _turnedXMovesAgo);

    public PathNode(AoC23.PathNode path, int turnedXMovesAgo)
    {
        _lastNode = path;
        _turnedXMovesAgo = turnedXMovesAgo;
    }

    public (Coordinate, Direction2D, int) GetVisitedTuple()
    {
        return (_lastNode.step.coord, _lastNode.step.direction, _turnedXMovesAgo);
    }

    public PathNode()
    { }

    public PathNode ExpandPath(AoC23.PathNode path)
    {
        int turnedXMovesAgo = 0;
        // If we continue in the same direction add 1 to turnedXMovesAgo
        // Otherwise set it to 0
        if (path.step.direction == _lastNode.step.direction) turnedXMovesAgo = _turnedXMovesAgo + 1;

        return new PathNode(path, turnedXMovesAgo);
    }

    public readonly int Cost()
    {
        return _lastNode.cost;
    }

    public Coordinate LastCoord() => _lastNode.step.coord;

    public Direction2D GetLastDirection() => _lastNode.step.direction;

    public readonly AoC23.PathNode GetLastNode()
    {
        return _lastNode;
    }

    public List<Step> GetNextSteps(Coordinate maxCoord)
    {
        Coordinate coord = _lastNode.step.coord;
        List<Step> neighbours = [];
        // Left
        if (coord.x > 0)
        {
            neighbours.Add(new Step
            {
                coord = new Coordinate { y = coord.y, x = coord.x - 1 },
                direction = Direction2D.Left
            });
        }
        // Up
        if (coord.y > 0)
        {
            neighbours.Add(new Step
            {
                coord = new Coordinate { y = coord.y - 1, x = coord.x },
                direction = Direction2D.Up
            });
        }
        // Right
        if (coord.x < maxCoord.x)
        {
            neighbours.Add(new Step
            {
                coord = new Coordinate { y = coord.y, x = coord.x + 1 },
                direction = Direction2D.Right
            });

        }
        // Down
        if (coord.y < maxCoord.y)
        {
            neighbours.Add(new Step
            {
                coord = new Coordinate { y = coord.y + 1, x = coord.x },
                direction = Direction2D.Down
            });
        }

        return neighbours;
    }

    public int CompareTo(PathNode other)
    {
        if (other.Cost() < Cost()) return 1;
        if (other.Cost() == Cost()) return 0;
        return -1;
    }

    public bool Equals(PathNode other)
    {
        return _lastNode.Equals(other._lastNode) &&
            _turnedXMovesAgo == other._turnedXMovesAgo;
    }
}

public static class Day17
{
    // Return the numerial value of the char in the grid (are digits)
    public static int CostFunction(Grid2D grid, Coordinate coord)
    {
        return grid.GetAtCoordinate(coord) - '0';
    }

    private static List<PathNode> ExpandPathPart1(
        PathNode path,
        Grid2D grid)
    {
        return ExpandPathDay17(path, grid, DirectionLegalDay17Part1);
    }

    private static List<PathNode> ExpandPathPart2(
        PathNode path,
        Grid2D grid)
    {
        return ExpandPathDay17(path, grid, DirectionLegalDay17Part2);
    }

    // Adds all possible steps to one path and returns all the new paths
    private static List<PathNode> ExpandPathDay17(
        PathNode path,
        Grid2D grid,
        Func<Step, PathNode, Coordinate, bool> isDirectionLegalFunction)
    {
        List<PathNode> newPaths = new();
        // Max coord is end here
        Coordinate maxCoord = new Coordinate { x = grid.Width() - 1, y = grid.Height() - 1 };

        foreach (Step newStep in path.GetNextSteps(maxCoord))
        {
            if (!isDirectionLegalFunction(newStep, path, maxCoord)) continue;

            newPaths.Add(path.ExpandPath(path:
                new AoC23.PathNode
                {
                    step = newStep,
                    cost = CostFunction(grid, newStep.coord) + path.Cost()
                }
            ));
        }
        return newPaths;
    }

    private static bool DirectionLegalDay17Part1(Step newStep, PathNode path, Coordinate end)
    {
        // Cannot reverse
        if (Direction2DExtensions.Opposite(
            path.GetLastNode().step.direction) == newStep.direction) return false;

        // Can not take more than 3 steps in the same direction
        if (path._turnedXMovesAgo < 2)
        {
            return true;
        }
        // If taken 3 steps in the same direction, we must turn now
        return path.GetLastDirection() != newStep.direction;
    }

    private static bool DirectionLegalDay17Part2(Step newStep, PathNode path, Coordinate end)
    {
        if (newStep.coord.Equals(end)) return path._turnedXMovesAgo >= 2;

        // Can only turn after 4 moves straight
        if (path._turnedXMovesAgo < 3) return newStep.direction == path.GetLastDirection();

        // Cannot reverse
        if (Direction2DExtensions.Opposite(path.GetLastNode().step.direction) == newStep.direction) return false;

        // Can not take more than 10 steps in the same direction
        if (path._turnedXMovesAgo >= 9)
        {
            return path.GetLastDirection() != newStep.direction;
        }

        return true;
    }

    public static long Part1(string input)
    {
        Grid2D grid = Grid2D.Parse(input);
        PathFinder pathFinder = new(grid);
        return pathFinder.PathFindDijkstra(
            start: new Coordinate { x = 0, y = 0 },
            end: grid.BottomRight(),
            pathExpansionFunction: ExpandPathPart1);
    }

    public static long Part2(string input)
    {
        Grid2D grid = Grid2D.Parse(input);
        PathFinder pathFinder = new(grid);
        return pathFinder.PathFindDijkstra(
            start: new Coordinate { x = 0, y = 0 },
            end: grid.BottomRight(),
            pathExpansionFunction: ExpandPathPart2);
    }
}