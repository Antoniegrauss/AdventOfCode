namespace AoC23.Day11
{

    public class Universe
    {
        List<List<char>> cells = new();
        List<Coordinate> galaxies = new();

        private static char EMPTY_SPACE = '.';

        public static Universe Parse(string input)
        {
            Universe grid = new Universe();

            string[] lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                List<char> row = new List<char>();
                foreach (char c in line)
                {
                    row.Add(c);
                }
                grid.cells.Add(row);
            }

            return grid;
        }

        public void ExpandEmptySpace()
        {
            ExpandRows();
            ExpandCols();
        }

        public void ExpandRows()
        {
            List<List<char>> newRows = new();
            foreach (List<char> row in cells)
            {
                // Expand empty rows by duplicating them
                if (row.All(c => c == EMPTY_SPACE))
                {
                    newRows.Add(row);
                }
                newRows.Add(row);
            }
            cells = newRows;
        }

        public void ExpandCols()
        {
            List<List<char>> newCols = new();
            // Have to transpose rows to get cols
            for (int i = 0; i < cells[0].Count; i++)
            {
                List<char> col = [.. cells.Select(row => row[i])];
                if (col.All(c => c == EMPTY_SPACE))
                {
                    newCols.Add(col);
                }
                newCols.Add(col);
            }

            List<List<char>> newCells = new();
            for (int i = 0; i < newCols[0].Count; i++)
            {
                newCells.Add([.. newCols.Select(row => row[i])]);
            }
            cells = newCells;
        }

        public void ExpandEmptySpaceByFactor(int expansion)
        {
            ExpandRowsByFactor(expansion);
            ExpandColsByFactor(expansion);
        }

        private void ExpandRowsByFactor(int expansion)
        {
            for (int i = cells.Count - 1; i > 0; i--)
            {
                List<char> row = cells[i];
                // Expand empty rows by duplicating them
                if (row.All(c => c == EMPTY_SPACE))
                {
                    foreach (Coordinate galaxy in galaxies)
                    {
                        if (galaxy.y > i)
                        {
                            galaxy.y += expansion;
                        }
                    }
                }
            }
        }

        private void ExpandColsByFactor(int factor)
        {
            for (int i = cells[0].Count - 1; i > 0; i--)
            {
                List<char> col = [.. cells.Select(row => row[i])];
                if (col.All(c => c == EMPTY_SPACE))
                {
                    foreach (Coordinate galaxy in galaxies)
                    {
                        if (galaxy.x > i)
                        {
                            galaxy.x += factor;
                        }
                    }
                }
            }
        }

        public void FindGalaxies()
        {
            for (int y = 0; y < cells.Count; y++)
            {
                for (int x = 0; x < cells[0].Count; x++)
                {
                    char c = cells[y][x];
                    if (c != EMPTY_SPACE)
                    {
                        galaxies.Add(new Coordinate { x = x, y = y });
                    }
                }
            }
        }

        public List<(Coordinate, Coordinate)> GetGalaxyPairs()
        {
            List<(Coordinate, Coordinate)> pairs = new();
            for (int i = 0; i < galaxies.Count; i++)
            {
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    pairs.Add((galaxies[i], galaxies[j]));
                }
            }
            return pairs;
        }

        public long CalculateDistancesBetweenAllGalaxyPairs()
        {
            long totalDistance = 0;
            var pairs = GetGalaxyPairs();
            foreach (var (galaxy1, galaxy2) in pairs)
            {
                totalDistance += galaxy1.ManhattanDistance(galaxy2);
            }
            return totalDistance;
        }

        public record Coordinate
        {
            public int x;
            public int y;

            public int ManhattanDistance(Coordinate other)
            {
                return Math.Abs(x - other.x) + Math.Abs(y - other.y);
            }
        }
    }

    public class Day11
    {
        public static long Part1(string input)
        {
            Universe universe = Universe.Parse(input);
            universe.ExpandEmptySpace();
            universe.FindGalaxies();
            return universe.CalculateDistancesBetweenAllGalaxyPairs();
        }

        public static long Part2(string input, int factor)
        {
            // Expand empty space with 1000000 instead of 1
            Universe universe = Universe.Parse(input);
            universe.FindGalaxies();

            universe.ExpandEmptySpaceByFactor(factor - 1);
            return universe.CalculateDistancesBetweenAllGalaxyPairs();
        }
    }
}