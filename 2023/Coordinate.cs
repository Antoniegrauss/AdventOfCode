using System.Reflection.Metadata.Ecma335;

namespace AoC23;

public record Coordinate : IEquatable<Coordinate>
{
    public int x;
    public int y;

    public override int GetHashCode() => HashCode.Combine(x, y);

    public Coordinate Move(Direction2D direction)
    {
        return direction switch
        {
            Direction2D.Up => new Coordinate { x = x, y = y - 1 },
            Direction2D.Down => new Coordinate { x = x, y = y + 1 },
            Direction2D.Left => new Coordinate { x = x - 1, y = y },
            Direction2D.Right => new Coordinate { x = x + 1, y = y },
            _ => throw new Exception("Invalid direction"),
        };
    }

    public Coordinate Move(Direction2D direction, int amount)
    {
        return direction switch
        {
            Direction2D.Up => new Coordinate { x = x, y = y - amount },
            Direction2D.Down => new Coordinate { x = x, y = y + amount },
            Direction2D.Left => new Coordinate { x = x - amount, y = y },
            Direction2D.Right => new Coordinate { x = x + amount, y = y },
            _ => throw new Exception("Invalid direction"),
        };
    }

    public Direction2D GetDirectionTo(Coordinate other)
    {
        if (other.x == x)
        {
            if (other.y == y - 1)
            {
                return Direction2D.Up;
            }
            else if (other.y == y + 1)
            {
                return Direction2D.Down;
            }
        }
        else if (other.y == y)
        {
            if (other.x == x - 1)
            {
                return Direction2D.Left;
            }
            else if (other.x == x + 1)
            {
                return Direction2D.Right;
            }
        }

        throw new Exception("Coordinates are not adjacent");
    }

    public int ManhattanDistance(Coordinate other)
    {
        return Math.Abs(x - other.x) + Math.Abs(y - other.y);
    }

    public bool IsNeighbour(Coordinate other)
    {
        return (Math.Abs(other.x - x) == 1 && other.y == y) || (Math.Abs(other.y - y) == 1 && other.x == x);
    }

    public Direction2D WhichDirectionIsLeftOfLine(Coordinate previous)
    {
        Direction2D direction = previous.GetDirectionTo(this);
        return direction switch
        {
            Direction2D.Up => Direction2D.Left,
            Direction2D.Down => Direction2D.Right,
            Direction2D.Left => Direction2D.Down,
            Direction2D.Right => Direction2D.Up,
            _ => throw new Exception("Invalid direction"),
        };
    }

    public Direction2D WhichDirectionIsRightOfLine(Coordinate previous)
    {
        return Direction2DExtensions.Opposite(WhichDirectionIsLeftOfLine(previous));
    }

    public List<Coordinate> GetNeighboursAdjacent()
    {
        return
            [new Coordinate{x=x, y=y+1},
            new Coordinate{x=x+1, y=y},
            new Coordinate{x=x-1, y=y},
            new Coordinate{x=x, y=y-1}];
    }

    public List<Coordinate> GetNeighboursDiagonal()
    {
        return
            [new Coordinate{x=x+1, y=y+1},
            new Coordinate{x=x+1, y=y-1},
            new Coordinate{x=x-1, y=y-1},
            new Coordinate{x=x-1, y=y+1}];
    }

    public List<Coordinate> GetAllNeighbours()
    {
        var adjacent = GetNeighboursAdjacent();
        adjacent.AddRange(GetNeighboursDiagonal());
        return adjacent;
    }
}
