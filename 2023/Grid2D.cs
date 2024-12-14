namespace AoC23;

public class Grid2D(List<string> rows, Dictionary<Coordinate, char> gridLookUp)
{
    readonly List<string> _rows = rows;
    readonly Dictionary<Coordinate, char> _gridLookUp = gridLookUp;

    public static Grid2D Parse(string input)
    {
        List<string> rows = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

        Dictionary<Coordinate, char> gridLookUp = new();
        for (int y = 0; y < rows.Count; y++)
        {
            for (int x = 0; x < rows[0].Length; x++)
            {
                gridLookUp.Add(new Coordinate { x = x, y = y }, rows[y][x]);
            }
        }
        return new Grid2D(rows, gridLookUp);
    }

    public string GetRow(int rowId) => _rows[rowId];

    public int Height() => _rows.Count;
    public int Width() => _rows[0].Length;

    public List<Coordinate> AllCoords()
    {
        return [.. _gridLookUp.Keys];
    }

    public Coordinate BottomRight()
    {
        return new Coordinate
        {
            x = Width() - 1,
            y = Height() - 1
        };
    }

    public char GetAtCoordinate(Coordinate coord)
    {
        if (!IsInsideGrid(coord))
        {
            return '.';
        }
        return rows[coord.y][coord.x];
    }

    // Wrap the coordinate around when out of bound
    // a la Packman, when exiting right enter on the left
    public Coordinate WrapAround(Coordinate coord)
    {
        int x = coord.x;
        int y = coord.y;
        if (coord.x < 0) x = Width() - 1;
        if (coord.x >= Width()) x = 0;
        if (coord.y < 0) y = Height() - 1;
        if (coord.y >= Height()) y = 0;

        return new Coordinate { x = x, y = y };
    }

    public Coordinate? FindSymbol(char symbol)
    {
        foreach (var keyValuePair in _gridLookUp)
        {
            if (keyValuePair.Value == symbol) return keyValuePair.Key;
        }
        return null;
    }

    public List<Coordinate> FindAllOfSymbol(char symbol)
    {
        return [.. _gridLookUp
            .Where(keyValuePair => keyValuePair.Value == symbol)
            .Select(kvp => kvp.Key)];
    }

    public bool IsInsideGrid(Coordinate coord)
    {
        return coord.x >= 0 && coord.x < Width() && coord.y >= 0 && coord.y < Height();
    }

    public bool IsAtBorder(Coordinate coord)
    {
        return coord.x == 0 || coord.x == Width() - 1 || coord.y == 0 || coord.y == Height() - 1;
    }

    public List<char> GetNeighboursAdjacent(Coordinate coord)
    {
        List<char> neighbours = [];
        // Left
        if (coord.x > 0)
        {
            neighbours.Add(_rows[coord.y][coord.x - 1]);
        }
        // Up
        if (coord.y > 0)
        {
            neighbours.Add(_rows[coord.y - 1][coord.x]);
        }
        // Right
        if (coord.x < Height())
        {
            neighbours.Add(_rows[coord.y][coord.x + 1]);
        }
        // Down
        if (coord.y < Width())
        {
            neighbours.Add(_rows[coord.y + 1][coord.x]);
        }

        return neighbours;
    }

    public List<char> GetNeighboursAlsoDiagonal(Coordinate coord)
    {
        List<char> neighbours = [];
        // Left
        if (coord.x > 0)
        {
            neighbours.Add(_rows[coord.y][coord.x - 1]);
        }
        // Up Left
        if (coord.x > 0 && coord.y > 0)
        {
            neighbours.Add(_rows[coord.y - 1][coord.x - 1]);
        }
        // Up
        if (coord.y > 0)
        {
            neighbours.Add(_rows[coord.y - 1][coord.x]);
        }
        // Up Right
        if (coord.y > 0 && coord.x < Height())
        {
            neighbours.Add(_rows[coord.y - 1][coord.x + 1]);
        }
        // Right
        if (coord.x < Height())
        {
            neighbours.Add(_rows[coord.y][coord.x + 1]);
        }
        // Right Down
        if (coord.x < Height() && coord.y < Width())
        {
            neighbours.Add(_rows[coord.y + 1][coord.x + 1]);
        }
        // Down
        if (coord.y < Width())
        {
            neighbours.Add(_rows[coord.y + 1][coord.x]);
        }
        // Down Left
        if (coord.y < Width() && coord.x > 0)
        {
            neighbours.Add(_rows[coord.y + 1][coord.x - 1]);
        }

        return neighbours;
    }
}
