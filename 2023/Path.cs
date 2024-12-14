namespace AoC23;

public struct Path
{
    private List<Step> steps = [];

    public Path()
    { }

    public void AddStep(Step step)
    {
        steps.Add(step);
    }

    public readonly List<Coordinate> AllCoords() => [.. steps.Select(s => s.coord)];

    public Coordinate LastCoord()
    {
        if (steps.Count > 0) return steps[^1].coord;
        return new Coordinate { x = 0, y = 0 };
    }

    public HashSet<Coordinate> FindCoordinatesNextToPath(Direction2D leftOrRightOfPath)
    {
        HashSet<Coordinate> alongPath = [.. steps.Select(step => step.RelativeMove(leftOrRightOfPath).coord)];
        return [.. alongPath.Except(AllCoords())];
    }

}

public struct Edge
{
    public Coordinate _start { get; set; }
    public Coordinate _end { get; set; }
}

public struct Polygon
{
    public List<Edge> _edges { get; private set; } = new();

    public Polygon()
    { }

    public void AddEdge(Coordinate end)
    {
        _edges.Add(new Edge { _start = LastCoord(), _end = end });
    }

    public Coordinate LastCoord()
    {
        if (_edges.Count > 0) return _edges[^1]._end;
        return new Coordinate { x = 0, y = 0 };
    }

    public long BorderLength()
    {
        return _edges.Sum(edge => edge._start.ManhattanDistance(edge._end));
    }
}
