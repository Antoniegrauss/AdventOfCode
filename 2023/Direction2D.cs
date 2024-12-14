namespace AoC23;

public enum Direction2D
{
    Up = 1,
    Down = 2,
    Left = 4,
    Right = 8,
}

public class Direction2DExtensions
{
    public static Direction2D Opposite(Direction2D direction)
    {
        return direction switch
        {
            Direction2D.Up => Direction2D.Down,
            Direction2D.Down => Direction2D.Up,
            Direction2D.Left => Direction2D.Right,
            Direction2D.Right => Direction2D.Left,
            _ => throw new Exception("Invalid direction"),
        };
    }

    public static bool IsHorizontal(Direction2D direction)
    {
        return direction == Direction2D.Left ||
            direction == Direction2D.Right;
    }

    public static bool IsVertical(Direction2D direction)
    {
        return direction == Direction2D.Up ||
            direction == Direction2D.Down;
    }

    public static Direction2D RotateLeft(Direction2D direction)
    {
        return direction switch
        {
            Direction2D.Up => Direction2D.Left,
            Direction2D.Left => Direction2D.Down,
            Direction2D.Down => Direction2D.Right,
            Direction2D.Right => Direction2D.Up,
            _ => throw new Exception("Invalid direction"),
        };
    }

    public static Direction2D RotateRight(Direction2D direction)
    {
        return direction switch
        {
            Direction2D.Up => Direction2D.Right,
            Direction2D.Right => Direction2D.Down,
            Direction2D.Down => Direction2D.Left,
            Direction2D.Left => Direction2D.Up,
            _ => throw new Exception("Invalid direction"),
        };
    }

    public static Direction2D RelativeDirection(Direction2D frame, Direction2D turn)
    {
        if (frame == turn) return Direction2D.Up;
        if (Opposite(frame) == turn) return Direction2D.Down;
        if (RotateLeft(frame) == turn) return Direction2D.Left;
        // Must be right if other options are false
        return Direction2D.Right;
    }
}
