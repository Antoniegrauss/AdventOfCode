using System.Security;

namespace AoC23.Day23;

public class DAG
{
    public Dictionary<Coordinate, HashSet<DirectedEdge>> _connections = new();

    public void AddConnection(DirectedEdge edge)
    {
        // Add end node to dictionary if it does not exist yet
        if (!_connections.ContainsKey(edge._end.coord))
        {
            _connections.Add(edge._end.coord, []);
        }

        // Add starting node if it does not exist yet
        if (!_connections.ContainsKey(edge._start.coord))
        {
            _connections.Add(edge._start.coord, [edge]);
            return;
        }

        // Add the connection to the start node
        _connections[edge._start.coord].Add(edge);
    }

    public long LongestPathLength(Coordinate start, Coordinate end)
    {
        Dictionary<Coordinate, long> maxDistanceToNodes = new()
        {
            { start, 0 }
        };

        Queue<Coordinate> nodesToCheck = new();
        nodesToCheck.Enqueue(start);

        while (nodesToCheck.Count > 0) ProcessQueue(maxDistanceToNodes, nodesToCheck);

        // Subtract 1 for not counting the starting step
        return maxDistanceToNodes[end] - 1;
    }

    private void ProcessQueue(
        Dictionary<Coordinate, long> maxDistanceToNodes,
        Queue<Coordinate> nodesToCheck)
    {
        foreach (DirectedEdge edge in _connections[nodesToCheck.Dequeue()])
        {
            long costToEdgeEnd = maxDistanceToNodes[edge._start.coord] + edge._length;
            if (maxDistanceToNodes.ContainsKey(edge._end.coord))
            {
                // Store new value if higher
                maxDistanceToNodes[edge._end.coord] =
                    Math.Max(maxDistanceToNodes[edge._end.coord],
                        costToEdgeEnd);
                continue;
            }
            maxDistanceToNodes.Add(edge._end.coord, costToEdgeEnd);
            nodesToCheck.Enqueue(edge._end.coord);
        }
    }
}

public class UndirectedGraph
{
    // Edges contain start and end, so can also be treated as undirected
    Dictionary<Coordinate, HashSet<UndirectedEdge>> _connections = new();


    public void AddConnection(UndirectedEdge edge)
    {
        // Add end node to dictionary if it does not exist yet
        if (!_connections.ContainsKey(edge._end))
        {
            _connections.Add(edge._end, []);
        }

        // Add starting node if it does not exist yet
        if (!_connections.ContainsKey(edge._start))
        {
            _connections.Add(edge._start, [edge]);
            return;
        }

        // Add the connection to the start node
        _connections[edge._start].Add(edge);
        _connections[edge._end].Add(edge.Reverse());
    }

    public long LongestPathLength(Coordinate start, Coordinate end)
    {
        List<long> costsToEnd = [];

        Queue<UndirectedPath> nodesToCheck = new();
        nodesToCheck.Enqueue(new UndirectedPath(start));

        while (nodesToCheck.Count > 0) ProcessQueue(nodesToCheck, end, costsToEnd);

        // Subtract 1 for not counting the starting step
        return costsToEnd.Max() - 1;
    }

    private void ProcessQueue(
        Queue<UndirectedPath> nodesToCheck,
        Coordinate end,
        List<long> costsToEnd)
    {
        UndirectedPath pathToCheck = nodesToCheck.Dequeue();
        foreach (UndirectedEdge edge in _connections[pathToCheck.LastCoord()])
        {
            if (edge._end == end)
            {
                costsToEnd.Add(pathToCheck._cost + edge._length);
                continue;
            }
            if (pathToCheck.Contains(edge._end)) continue;
            nodesToCheck.Enqueue(new UndirectedPath(pathToCheck, edge));
        }
    }
}

public struct UndirectedEdge : IEquatable<UndirectedEdge>
{
    public Coordinate _start;
    public Coordinate _end;
    public long _length;

    public UndirectedEdge Reverse()
    {
        return new UndirectedEdge
        {
            _start = _end,
            _end = _start,
            _length = _length
        };
    }

    public bool Equals(UndirectedEdge other)
    {
        return _start.Equals(other._start) &&
            _end.Equals(other._end) &&
            _length == other._length;
    }

    // Do not use the directions to calculate the hash here
    public override int GetHashCode() => HashCode.Combine(_start, _end, _length);
}

public struct UndirectedPath
{
    public HashSet<Coordinate> _coords = new();
    public long _cost = 0;

    public UndirectedPath(Coordinate coord)
    {
        _coords = [coord];
    }

    public UndirectedPath(UndirectedPath previousPath, UndirectedEdge end)
    {
        _coords = [.. _coords.Union(previousPath._coords)];
        _coords.Add(end._end);
        _cost = previousPath._cost + end._length;
    }

    public bool Contains(Coordinate coord)
    {
        return _coords.Contains(coord);
    }

    public Coordinate LastCoord() => _coords.Last();
}

// Adding the start might be redundant for directed edge
// But it does give nice debug information
public struct DirectedEdge : IEquatable<DirectedEdge>
{
    public Step _start;
    public Step _end;
    public long _length;

    public bool Equals(DirectedEdge other)
    {
        return _start.coord.Equals(other._start.coord) &&
            _end.coord.Equals(other._end.coord) &&
            _length == other._length;
    }

    // Do not use the directions to calculate the hash here
    public override int GetHashCode() => HashCode.Combine(_start.coord, _end.coord, _length);
}

public class Day23
{
    public static long Part1(string input)
    {
        // Create a DAG of the input
        // Use the crossroads as nodes
        // Calculate the length between nodes
        Grid2D grid = Grid2D.Parse(input);
        DAG dag = CreateDAG(grid);
        return dag.LongestPathLength(
            start: new Coordinate { x = 1, y = 0 },
            end: new Coordinate { x = grid.Width() - 2, y = grid.Height() - 1 });
    }

    public static long Part2(string input)
    {
        Grid2D grid = Grid2D.Parse(input);
        UndirectedGraph dag = CreateUndirectedGraph(grid);
        return dag.LongestPathLength(
            start: new Coordinate { x = 1, y = 0 },
            end: new Coordinate { x = grid.Width() - 2, y = grid.Height() - 1 });
    }

    private static DAG CreateDAG(Grid2D grid)
    {
        // Hardcoded start position
        Step start = new Step
        {
            coord = new Coordinate { x = 1, y = 0 },
            direction = Direction2D.Down
        };

        DirectedEdge connectedToStart = WalkPath(grid, start);
        DAG dag = new DAG();
        dag.AddConnection(connectedToStart);

        Queue<Step> toConnect = new();
        toConnect.Enqueue(connectedToStart._end);

        while (toConnect.Count > 0)
        {
            ExpandDAG(grid, toConnect, dag);
        }

        return dag;
    }

    private static UndirectedGraph CreateUndirectedGraph(Grid2D grid)
    {
        // Hardcoded start position
        Coordinate start = new Coordinate { x = 1, y = 0 };

        UndirectedEdge connectedToStart = WalkPathUndirected(
            grid, start, new Coordinate { x = 0, y = 0 });
        UndirectedGraph graph = new();
        graph.AddConnection(connectedToStart);

        Queue<Coordinate> toConnect = new();
        toConnect.Enqueue(connectedToStart._end);

        HashSet<Coordinate> visited = [start];

        while (toConnect.Count > 0)
        {
            ExpandUndirectedGraph(grid, toConnect, graph, visited);
        }

        return graph;
    }

    private static void ExpandDAG(Grid2D grid, Queue<Step> toConnect, DAG dag)
    {
        Step current = toConnect.Dequeue();
        foreach (Step newPathStart in FindStartStepsOfNewPaths(current, grid))
        {
            DirectedEdge newEdge = WalkPath(
                grid, newPathStart);
            // Reset the start to the actual node
            newEdge._start = current;

            dag.AddConnection(newEdge);
            toConnect.Enqueue(newEdge._end);
        }
    }

    private static void ExpandUndirectedGraph(
        Grid2D grid,
        Queue<Coordinate> toConnect,
        UndirectedGraph dag,
        HashSet<Coordinate> visisted)
    {
        Coordinate current = toConnect.Dequeue();
        if (visisted.Contains(current)) return;
        visisted.Add(current);

        foreach (Coordinate newPathStart in FindStartStepsOfNewPathsPart2(current, grid))
        {
            UndirectedEdge newEdge = WalkPathUndirected(
                grid, newPathStart, current);
            // Reset the start to the actual node
            newEdge._start = current;

            dag.AddConnection(newEdge);

            if (!visisted.Contains(newEdge._end))
            {
                toConnect.Enqueue(newEdge._end);
            }
        }
    }

    private static List<Step> FindStartStepsOfNewPaths(Step current, Grid2D grid)
    {
        return [.. current.GetPossibleStepsForward()
            .Where(step => IsValid(step, grid))];
    }

    private static List<Coordinate> FindStartStepsOfNewPathsPart2(Coordinate current, Grid2D grid)
    {
        return [.. current.GetNeighboursAdjacent()
            .Where(coord => IsValidPart2(coord, grid))];
    }

    // Walks along a path until there is a split in the path
    private static DirectedEdge WalkPath(Grid2D grid, in Step start)
    {
        DirectedEdge edge = new();
        edge._start = start;
        edge._length = 1;
        edge._end = start;

        // Get steps in direction non-opposite
        // Filter walls out
        while (true)
        {
            List<Step> nextSteps = [.. edge._end.GetPossibleStepsForward()
                .Where(step => IsValid(step, grid))];
            if (nextSteps.Count != 1) break;

            edge._length += 1;
            edge._end = nextSteps[0];
        }

        return edge;
    }

    // Walks along a path until there is a split in the path
    private static UndirectedEdge WalkPathUndirected(
        Grid2D grid,
        in Coordinate start,
        Coordinate beforeStart)
    {
        UndirectedEdge edge = new();
        edge._start = start;
        edge._length = 1;
        edge._end = start;

        HashSet<Coordinate> visited = [beforeStart, start];

        // Get steps in direction non-opposite
        // Filter walls out
        while (true)
        {
            List<Coordinate> nextSteps = [.. edge._end.GetNeighboursAdjacent()
                .Where(step => IsValidPart2(step, grid))];
            nextSteps = [.. nextSteps.Except(visited)];
            foreach (Coordinate coord in nextSteps)
            {
                visited.Add(coord);
            }
            if (nextSteps.Count != 1) break;

            edge._length += 1;
            edge._end = nextSteps[0];
        }

        return edge;
    }

    private static bool IsValid(Step step, Grid2D grid)
    {
        return grid.IsInsideGrid(step.coord) &&
                DirectionValid(step.direction, grid.GetAtCoordinate(step.coord));
    }

    private static bool IsValidPart2(Coordinate coord, Grid2D grid)
    {
        return grid.IsInsideGrid(coord) &&
                !(grid.GetAtCoordinate(coord) == '#');
    }

    private static bool DirectionValid(Direction2D direction, char symbol)
    {
        if (symbol == '.') return true;
        if (symbol == '#') return false;
        if (symbol == '>') return direction == Direction2D.Right;
        if (symbol == '<') return direction == Direction2D.Left;
        if (symbol == 'v') return direction == Direction2D.Down;
        if (symbol == '^') return direction == Direction2D.Up;
        return false;
    }
}