using System.ComponentModel;

namespace AoC23.Day16;

public class Beam
{

    public Beam(Coordinate coordinate, Direction2D direction)
    {
        _location = coordinate;
        _direction = direction;
    }

    public Coordinate _location { get; set; }
    public Direction2D _direction { get; set; }

    public Coordinate LocationAfterMove()
    {
        return _location.Move(_direction);
    }

    public void TakeStep()
    {
        _location = _location.Move(_direction);
    }
}

public enum CellType
{
    Empty = '.',
    SplitterVertical = '|',
    SplitterHorizontal = '-',
    MirrorLeft = '\\', // Mirror bouncing left-up and right-down
    MirrorRight = '/', // Mirror bouncing left-down and right-up
}

public static class CellTypeExtensions
{
    public static Direction2D BounceOffMirror(Direction2D direction, CellType mirror)
    {
        if (mirror == CellType.MirrorLeft)
        {
            return direction switch
            {
                Direction2D.Up => Direction2D.Left,
                Direction2D.Left => Direction2D.Up,
                Direction2D.Right => Direction2D.Down,
                Direction2D.Down => Direction2D.Right,
            };
        }

        if (mirror != CellType.MirrorRight)
        {
            throw new Exception("Should be MirrorLeft or MirrorRight");
        }

        return direction switch
        {
            Direction2D.Up => Direction2D.Right,
            Direction2D.Right => Direction2D.Up,
            Direction2D.Left => Direction2D.Down,
            Direction2D.Down => Direction2D.Left,
        };
    }

    public static bool IsSplitter(CellType cell)
    {
        return cell == CellType.SplitterHorizontal ||
            cell == CellType.SplitterVertical;
    }

    public static bool IsMirror(CellType cell)
    {
        return cell == CellType.MirrorLeft ||
            cell == CellType.MirrorRight;
    }
}

public static class Day16
{
    public static long EnergizedByStartPosition(Grid2D grid, Beam start)
    {
        HashSet<(Coordinate, Direction2D)> visitedCache = new();
        HashSet<Coordinate> energizedTiles = new();

        List<Beam> beams = new([start]);

        List<Beam> newBeams = new();
        while (beams.Count > 0)
        {
            newBeams.Clear();
            foreach (Beam beam in beams)
            {
                // Try to move as far as possible through free cells
                (List<Coordinate> newCoords, Beam beamAfterMove) = MoveBeamUntilHit(beam, grid);
                energizedTiles.UnionWith(newCoords);

                // Take a step to a non-free tile
                newBeams.AddRange(StepBeam(beamAfterMove, ref grid));
                // If beam got out of grid, skip
                if (newBeams.Count == 0) continue;

                // Only 1 new location to Add
                // Even if splitting, the location of the beams are the same
                // Only the directions are opposite
                energizedTiles.Add(newBeams.Last()._location);
            }
            beams.Clear();

            // Only add beams that we did not find before yet
            foreach (Beam beam in newBeams)
            {
                if (visitedCache.Contains((beam._location, beam._direction))) continue;
                visitedCache.Add((beam._location, beam._direction));
                beams.Add(beam);
            }
        }

        return energizedTiles.Count;
    }

    public static long Part1(string input)
    {
        Grid2D grid = Grid2D.Parse(input);
        Beam startPosition = new(new Coordinate { x = -1, y = 0 }, Direction2D.Right);
        return EnergizedByStartPosition(grid, startPosition);
    }

    private static (List<Coordinate>, Beam) MoveBeamUntilHit(Beam beam, Grid2D grid)
    {
        List<Coordinate> energizedCoords = new();
        while (true)
        {
            Coordinate newLocation = beam.LocationAfterMove();
            if (!grid.IsInsideGrid(newLocation)) break;
            if ((CellType)grid.GetAtCoordinate(newLocation) != CellType.Empty) break;

            beam.TakeStep();
            energizedCoords.Add(beam._location);
        }

        return (energizedCoords, beam);
    }

    private static List<Beam> StepBeam(Beam beam, ref readonly Grid2D grid)
    {
        Coordinate newPosition = beam.LocationAfterMove();

        // If outside grid now, destroy beam
        if (!grid.IsInsideGrid(newPosition))
        {
            return new List<Beam>();
        }

        // Return the 2 split beams if we hit a splitter correctly
        if (CanSplitBeam(beam, grid.GetAtCoordinate(newPosition)))
        {
            beam.TakeStep();
            return DoSplitBeam(beam);
        }

        // Change direction if we hit a mirror
        if (CellTypeExtensions.IsMirror((CellType)grid.GetAtCoordinate(newPosition)))
        {
            RotateBeamWithMirror(ref beam, (CellType)grid.GetAtCoordinate(newPosition));
            beam._location = newPosition;
            return new List<Beam>([beam]);
        }

        beam.TakeStep();
        return new List<Beam>([beam]);
    }

    private static bool CanSplitBeam(Beam beam, char gridCell)
    {
        CellType cell = (CellType)gridCell;
        if (!CellTypeExtensions.IsSplitter(cell)) return false;

        if ((CellType)gridCell == CellType.SplitterHorizontal)
        {
            return Direction2DExtensions.IsVertical(beam._direction);
        }

        // Sanity check, if it is a splitter and not vertical,
        // it should be a horizontal splitter
        if ((CellType)gridCell != CellType.SplitterVertical)
        {
            throw new Exception("Celltype is splitter but not horizontal or vertical splitter?");
        }

        return Direction2DExtensions.IsHorizontal(beam._direction);
    }

    private static List<Beam> DoSplitBeam(Beam beam)
    {
        Direction2D rotateLeft = Direction2DExtensions.RotateLeft(beam._direction);
        Direction2D rotateRight = Direction2DExtensions.RotateRight(beam._direction);

        Beam opposite = new Beam(beam._location, rotateLeft);
        beam._direction = rotateRight;

        return new List<Beam>([beam, opposite]);
    }

    private static void RotateBeamWithMirror(ref Beam beam, CellType mirrorType)
    {
        beam._direction = CellTypeExtensions.BounceOffMirror(beam._direction, mirrorType);
    }


    public static long Part2(string input)
    {
        Grid2D grid = Grid2D.Parse(input);

        long maxEnergizedCount = 0;
        // Create a list of all border positions
        // Run the algorithm for each one
        List<Beam> startPositions = BorderAroundGrid(grid);

        foreach (Beam startPosition in startPositions)
        {
            long score = EnergizedByStartPosition(grid, startPosition);
            maxEnergizedCount = Math.Max(score, maxEnergizedCount);
        }

        return maxEnergizedCount;
    }

    private static List<Beam> BorderAroundGrid(Grid2D grid)
    {
        List<Beam> beams = new();
        for (int y = 0; y < grid.Height(); y++)
        {
            beams.Add(new Beam(new Coordinate { x = -1, y = y }, Direction2D.Right));
            beams.Add(new Beam(new Coordinate { x = grid.Width() - 1, y = y }, Direction2D.Left));
        }
        for (int x = 0; x < grid.Width(); x++)
        {
            beams.Add(new Beam(new Coordinate { x = x, y = -1 }, Direction2D.Down));
            beams.Add(new Beam(new Coordinate { x = x, y = grid.Height() - 1 }, Direction2D.Up));
        }
        return beams;
    }
    
}