using System.Windows.Controls;

namespace AoC23.Day3;

public struct NumberInGrid
{
    public NumberInGrid()
    {
        _value = -1;
        _connected_to_symbol = false;
        _coordinates = [];
    }

    public int _value;
    public bool _connected_to_symbol;
    public List<Tuple<int, int>> _coordinates;

    // Any non . and non digit counts as a connecting symbol
    public static bool IsConnector(char ch)
    {
        if (ch == '\\')
        {
            throw new Exception("Should not contain backslashes");
        }
        return ch != '.' && (!Char.IsDigit(ch));
    }

    public static int SumValues(List<NumberInGrid> numbers)
    {
        int sum = 0;
        foreach (NumberInGrid number in numbers)
        {
            if (number._connected_to_symbol)
            {
                sum += number._value;
            }
        }
        return sum;
    }
}

public class NumberGrid
{
    public List<List<char>> _grid;
    public int _max_row = 0;
    public int _max_col = 0;
    public NumberGrid(string input)
    {
        input = input.Replace("\r", "");
        List<string> lines = [.. input.Split('\n')];

        _grid = [];
        foreach (string line in lines)
        {
            _grid.Add([.. line.ToCharArray()]);
        }

        _max_row = _grid[0].Count() - 1;
        _max_col = _grid.Count() - 1;
    }

    public int FindAndSumNumbers()
    {
        bool checking_number = false;
        List<NumberInGrid> numbers = [];

        // i is row
        for (int i = 0; i < _max_row + 1; i += 1)
        {
            checking_number = false;
            // j is column
            for (int j = 0; j < _max_col + 1; j += 1)
            {
                if (!Char.IsDigit(_grid[i][j]))
                {
                    checking_number = false;
                    continue;
                }

                // Make sure we find any other symbol in between numbers
                if (!checking_number)
                {
                    checking_number = true;
                    numbers.Add(CheckNumber(row_id_start: i, col_id_start: j));
                }
            }
        }
        return NumberInGrid.SumValues(numbers);
    }

    public NumberInGrid CheckNumber(int row_id_start, int col_id_start)
    {
        NumberInGrid number = new();

        int col_id = col_id_start - 1;
        while (col_id < _max_col)
        {
            col_id += 1;

            char cell = _grid[row_id_start][col_id];
            if (Char.IsDigit(cell))
            {
                number._coordinates.Add(new Tuple<int, int>(row_id_start, col_id));
                if (number._value < 0)
                {
                    number._value = cell - '0';
                }
                else
                {
                    number._value *= 10;
                    number._value += cell - '0';
                }

                // Only bother checking the neighbours for symbols until we find one
                if (number._connected_to_symbol)
                {
                    continue;
                }
                List<char> neighbours = GetNeighbours(row_id_start, col_id);
                foreach (char neighbour in neighbours)
                {
                    if (NumberInGrid.IsConnector(neighbour))
                    {
                        number._connected_to_symbol = true;
                        break;
                    }
                }
            }
            else
            {
                return number;
            }
        }

        return number;
    }

    public List<char> GetNeighbours(int row_id, int col_id)
    {
        List<char> neighbours = [];
        // Left
        if (col_id > 0)
        {
            neighbours.Add(_grid[row_id][col_id - 1]);
        }
        // Up Left
        if (col_id > 0 && row_id > 0)
        {
            neighbours.Add(_grid[row_id - 1][col_id - 1]);
        }
        // Up
        if (row_id > 0)
        {
            neighbours.Add(_grid[row_id - 1][col_id]);
        }
        // Up Right
        if (row_id > 0 && col_id < _max_col)
        {
            neighbours.Add(_grid[row_id - 1][col_id + 1]);
        }
        // Right
        if (col_id < _max_col)
        {
            neighbours.Add(_grid[row_id][col_id + 1]);
        }
        // Right Down
        if (col_id < _max_col && row_id < _max_row)
        {
            neighbours.Add(_grid[row_id + 1][col_id + 1]);
        }
        // Down
        if (row_id < _max_row)
        {
            neighbours.Add(_grid[row_id + 1][col_id]);
        }
        // Down Left
        if (row_id < _max_row && col_id > 0)
        {
            neighbours.Add(_grid[row_id + 1][col_id - 1]);
        }

        return neighbours;
    }

}

public class Gear
{
    Coordinate gearCoord;

    List<NumberConnectedToGear> connectedNumbers = new();

    public long Score()
    {
        if (connectedNumbers.Count == 2) return connectedNumbers[0].value * connectedNumbers[1].value;
        return 0;
    }

    public Gear(Grid2D grid, Coordinate coord)
    {
        gearCoord = coord;

        // Find for these neighbours all numbers
        // Make sure to connect the digits of the numbers

        HashSet<Coordinate> visited = new();
        foreach (Coordinate neighbour in gearCoord.GetAllNeighbours())
        {
            if (visited.Contains(neighbour)) continue;
            char neighbourChar = grid.GetAtCoordinate(neighbour);
            if (!char.IsDigit(neighbourChar)) continue;
            connectedNumbers.Add(FindWholeNumber(grid, neighbour));
            foreach (Coordinate inNumber in connectedNumbers.Last().coords) visited.Add(inNumber);
        }
    }

    private static NumberConnectedToGear FindWholeNumber(Grid2D grid, Coordinate start)
    {
        NumberConnectedToGear gearNumber = new();

        string row = grid.GetRow(rowId: start.y);

        int startId = start.x;
        int endId = start.x;
        while (startId > 0)
        {
            if (!char.IsDigit(row[startId - 1])) break;
            startId -= 1;
        }
        while (endId < row.Length - 1)
        {
            if (!char.IsDigit(row[endId + 1])) break;
            endId += 1;
        }

        gearNumber.value = long.Parse(row.Substring(startId, endId - startId + 1));

        for (int i = startId; i <= endId; i++)
        {
            gearNumber.coords.Add(new Coordinate { x = i, y = start.y });
        }

        return gearNumber;
    }
}

public class NumberConnectedToGear
{
    public HashSet<Coordinate> coords = new();
    public long value;
}

public class Day3
{
    public static int Part1(string input)
    {
        NumberGrid grid = new(input);
        return grid.FindAndSumNumbers();
    }

    public static long Part2(string input)
    {
        Grid2D grid = Grid2D.Parse(input);
        List<Coordinate> gearCoords = grid.FindAllOfSymbol('*');
        List<Gear> gears = [.. gearCoords.Select(coord => new Gear(grid, coord))];

        return gears.Sum(gear => gear.Score());
    }

}