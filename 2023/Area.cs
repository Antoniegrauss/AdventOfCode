using System.ComponentModel;
using System.Dynamic;

namespace AoC23;

public class Area(HashSet<Coordinate> coords)
{
    public HashSet<Coordinate> _coords { get; private set; } = coords;

    public void GrowWithinBorder(HashSet<Coordinate> border)
    {
        HashSet<Coordinate> visited = [.. border.Union(_coords)];
        var toCheck = new Queue<Coordinate>(_coords);

        while (toCheck.Count > 0)
        {
            HashSet<Coordinate> neighbours = [.. toCheck.Dequeue().GetNeighboursAdjacent()];
            neighbours = [.. neighbours.Except(visited)];
            foreach (Coordinate newNeighbour in neighbours)
            {
                toCheck.Enqueue(newNeighbour);
                visited.Add(newNeighbour);
                _coords.Add(newNeighbour);
            }
        }
    }

    public void Combine(Area other)
    {
        _coords = [.. _coords.Union(other._coords)];
    }
}

public class Rectangle(Coordinate min, Coordinate max)
{
    public Coordinate _min { get; private set; } = min;
    public Coordinate _max { get; private set; } = max;

    public bool FitsInside(Rectangle other)
    {
        return other._min.x >= _min.x &&
        other._min.y >= _min.y &&
        other._max.x <= _max.x &&
        other._max.y <= _max.y;
    }

    public bool DoOverlap(Rectangle other)
    {
        // If the rectangles do not miss each other, they overlap
        // Missing is when the max is smaller than the other min coord
        // for either x or y
        // For example if the max x is smaller than the other min x, then the
        // entire rectangle is to the left of the other one and they miss.
        return !
            (other._max.x < _min.x ||
            other._max.y < _min.y ||
            other._min.x > _max.x ||
            other._min.y > _max.y);
    }
}