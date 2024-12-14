namespace AoC23;

using VisitedDict = Dictionary<Coordinate, int>;

public class PathFinder(Grid2D grid)
{
    Grid2D _grid = grid;

    // Dijkstra that takes into account directions as well
    public int PathFindDijkstra(
        Coordinate start,
        Coordinate end,
        Func<
            Day17.PathNode,
            Grid2D,
            List<Day17.PathNode>
            > pathExpansionFunction)
    {
        Direction2D[] startDirections = [Direction2D.Right, Direction2D.Down];

        List<Day17.PathNode> startingNodes = new();
        foreach (Direction2D direction in startDirections)
        {
            startingNodes.Add(new(
                new PathNode
                {
                    step = new Step
                    {
                        coord = start,
                        direction = direction
                    },
                    cost = 0
                },

                turnedXMovesAgo: 0
            ));
        }

        // Note: We assume going right or down on the first step does not matter
        VisitedDict visited = new();

        var queue = new PriorityQueue<Day17.PathNode, int>();
        HashSet<Day17.PathNode> alreadyChecked = new();

        foreach (Day17.PathNode node in startingNodes)
        {
            queue.Enqueue(node, 0);
            alreadyChecked.Add(node);
        }

        while (true)
        {
            // Check done:
            // If end is in visited and
            // queue[0] has higher cost than end
            Day17.PathNode lowestCostPath = queue.Dequeue();

            if (visited.ContainsKey(end))
            {
                return visited[end];
            }

            // Add new paths
            List<Day17.PathNode> newPaths = pathExpansionFunction(lowestCostPath, _grid);

            // Prune depending on already visited and cost
            newPaths = PrunePaths(visited, newPaths, alreadyChecked, end);

            // Add new paths to the queue
            foreach (Day17.PathNode newPath in newPaths)
            {
                queue.Enqueue(newPath, newPath.Cost() + end.ManhattanDistance(newPath.LastCoord()));
                alreadyChecked.Add(newPath);
            }
        }
    }

    private static List<Day17.PathNode> PrunePaths(
        VisitedDict visited,
        List<Day17.PathNode> paths,
        HashSet<Day17.PathNode> alreadyChecked,
        Coordinate end)
    {
        List<Day17.PathNode> pruned = new();
        foreach (Day17.PathNode path in paths)
        {
            if (alreadyChecked.Contains(path)) continue;

            // If new add to dictionary
            int pathTotalCost = path.Cost() + path.LastCoord().ManhattanDistance(end);
            if (!visited.ContainsKey(path.LastCoord()))
            {
                visited[path.LastCoord()] = pathTotalCost;
                pruned.Add(path);
                continue;
            }

            // If better path, add to dictionary
            int currentCost = visited[path.LastCoord()];
            if (currentCost > pathTotalCost)
            {
                visited[path.LastCoord()] = pathTotalCost;
                pruned.Add(path);
                continue;
            }

            // Need to account for directions and last turn
            const int TURNING_MARGIN = 30;
            if (currentCost + TURNING_MARGIN >= pathTotalCost)
            {
                pruned.Add(path);
            }
        }
        return pruned;
    }
}

public struct PathNode : IEquatable<PathNode>
{
    public Step step;
    public int cost;

    public bool Equals(PathNode other)
    {
        return step.Equals(other.step) &&
            cost == other.cost;
    }

    public override int GetHashCode() => HashCode.Combine(step, cost);
}

public struct Step : IEquatable<Step>
{
    public Coordinate coord;
    public Direction2D direction;

    public Step()
    { }

    public Step(Coordinate coordX, Direction2D directionX)
    {
        coord = coordX;
        direction = directionX;
    }

    public bool Equals(Step other)
    {
        return coord.Equals(other.coord) &&
            direction == other.direction;
    }

    public override int GetHashCode() => HashCode.Combine(coord, direction);

    public readonly bool SameDirection(Step other)
    {
        return direction == other.direction;
    }

    public readonly bool SameDirection(Direction2D other)
    {
        return direction == other;
    }

    // Move in this direction
    // Note: the direction is relative to the 
    public Step RelativeMove(Direction2D relativeDirection)
    {
        return relativeDirection switch
        {
            Direction2D.Up => new Step
            {
                coord = coord.Move(direction),
                direction = direction
            },
            Direction2D.Down => new Step
            {
                coord = coord.Move(Direction2DExtensions.Opposite(direction)),
                direction = direction
            },
            Direction2D.Left => new Step
            {
                coord = coord.Move(Direction2DExtensions.RotateLeft(direction)),
                direction = direction
            },
            Direction2D.Right => new Step
            {
                coord = coord.Move(Direction2DExtensions.RotateRight(direction)),
                direction = direction
            },
        };
    }

    // All coords that do not go backward
    public readonly List<Step> GetPossibleStepsForward()
    {
        Direction2D currentDir = direction;
        Coordinate currentCoord = coord;

        // Filter coords by getting the direction backwards,
        // which should not be the same as forwards
        List<Coordinate> newCoords = [.. coord.GetNeighboursAdjacent().Where(
            n => n.GetDirectionTo(currentCoord) != currentDir)];
        return [.. newCoords.Select(newCoord => new Step
        {
            coord = newCoord,
            direction = currentCoord.GetDirectionTo(newCoord)
        })];
    }
}
