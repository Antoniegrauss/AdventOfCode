using System.Runtime.CompilerServices;

namespace AoC23.Day22;

public enum Axes3D
{
    X,
    Y,
    Z
}

public record Coordinate3D : IEquatable<Coordinate3D>
{
    public int x, y, z;

    public Axes3D WhichAxisDiffers(Coordinate3D other)
    {
        if (x != other.x) return Axes3D.X;
        if (y != other.y) return Axes3D.Y;
        if (z != other.z) return Axes3D.Z;
        // If we have a block of length 1 return z
        return Axes3D.Z;
    }

    public Coordinate3D DropBy(int steps)
    {
        return new Coordinate3D { x = x, y = y, z = z - steps };
    }
}

public record SnowBrick(Coordinate3D start, Coordinate3D end, Axes3D alignedAlongAxis)
{
    public static SnowBrick Parse(string input)
    {
        string[] bounds = StringUtilities.SplitWithTrim(input, "~");
        int[] startBoundsArray = [.. bounds[0].Split(",").Select(i => int.Parse(i))];
        int[] endBoundsArray = [.. bounds[1].Split(",").Select(i => int.Parse(i))];

        Coordinate3D startBounds = new Coordinate3D
        {
            x = startBoundsArray[0],
            y = startBoundsArray[1],
            z = startBoundsArray[2]
        };

        Coordinate3D endBounds = new Coordinate3D
        {
            x = endBoundsArray[0],
            y = endBoundsArray[1],
            z = endBoundsArray[2]
        };

        Axes3D alignAxis = startBounds.WhichAxisDiffers(endBounds);

        return new SnowBrick
        (
            startBounds,
            endBounds,
            alignAxis
        );
    }

    public void DropBrick(int steps)
    {
        start.z -= steps;
        end.z -= steps;
    }

    public int MinZ()
    {
        return start.z;
    }

    public List<Coordinate3D> CoordsBelow()
    {
        if (alignedAlongAxis == Axes3D.Z) return [start.DropBy(1)];
        return [.. AllCoords().Select(coord => coord.DropBy(1))];
    }

    public List<Coordinate3D> CoordsAbove()
    {
        if (alignedAlongAxis == Axes3D.Z) return [end.DropBy(-1)];
        return [.. AllCoords().Select(coord => coord.DropBy(-1))];
    }

    public List<Coordinate3D> AllCoords()
    {
        List<Coordinate3D> coords = new();
        switch (alignedAlongAxis)
        {
            case Axes3D.X:
                for (int i = start.x; i <= end.x; i++)
                {
                    coords.Add(new Coordinate3D
                    {
                        x = i,
                        y = start.y,
                        z = start.z,
                    });
                }
                return coords;
            case Axes3D.Y:
                for (int i = start.y; i <= end.y; i++)
                {
                    coords.Add(new Coordinate3D
                    {
                        x = start.x,
                        y = i,
                        z = start.z,
                    });
                }
                return coords;
            case Axes3D.Z:
                for (int i = start.z; i <= end.z; i++)
                {
                    coords.Add(new Coordinate3D
                    {
                        x = start.x,
                        y = start.y,
                        z = i,
                    });
                }
                return coords;
        }
        throw new Exception("Axes3d should be exhaustive");
    }
}

public class SnowField
{
    Dictionary<Coordinate3D, SnowBrick> coordLookUp = new();
    List<SnowBrick> allBricks = new();

    public SnowField(string input)
    {
        string[] bricks = StringUtilities.SplitLines(input);
        foreach (string brick in bricks)
        {
            SnowBrick snowBrick = SnowBrick.Parse(brick);
            allBricks.Add(snowBrick);
            foreach (Coordinate3D coord in snowBrick.AllCoords())
            {
                coordLookUp.Add(coord, snowBrick);
            }
        }
    }

    public long Part1()
    {
        DropBricks();
        return CountUnnecessaryBricks();
    }

    private long CountUnnecessaryBricks()
    {
        long sum = 0;
        foreach (SnowBrick brick in allBricks)
        {
            var supportedBricks = Supports(brick);
            // If this brick is the only supporter for any brick above,
            // it can not be removed
            if (supportedBricks.Any(brick => IsSupportedBy(brick).Count == 1)) continue;
            sum += 1;
        }

        return sum;
    }

    private void DropBricks()
    {
        // Drop bricks in order of ascending z value
        // Then we should only need 1 pass
        allBricks.Sort((first, second) => first.start.z.CompareTo(second.start.z));

        for (int i = 0; i < allBricks.Count; i++)
        {
            DropBrickToSupport(allBricks[i]);
        }
    }

    private void DropBrickToSupport(SnowBrick brick)
    {
        // Drop brick until we hit another brick or the floor
        const int MIN_BRICK_Z = 1;
        if (IsSupportedBy(brick).Count != 0 || brick.MinZ() <= MIN_BRICK_Z) return;

        // Remove coord of this brick from the lookup
        foreach (Coordinate3D coord in brick.AllCoords())
        {
            coordLookUp.Remove(coord);
        }

        while (IsSupportedBy(brick).Count == 0 && brick.MinZ() > MIN_BRICK_Z)
        {
            brick.DropBrick(1);
        }

        // Add new coords of this brick back to lookup
        foreach (Coordinate3D coord in brick.AllCoords())
        {
            coordLookUp.Add(coord, brick);
        }
    }

    private HashSet<SnowBrick> IsSupportedBy(SnowBrick brick)
    {
        var coordsBelow = brick.CoordsBelow();
        return [.. coordsBelow
            .Where(coordLookUp.ContainsKey)
            .Select(below => coordLookUp[below])];
    }

    private HashSet<SnowBrick> Supports(SnowBrick brick)
    {
        var coordsBelow = brick.CoordsAbove();
        return [.. coordsBelow
            .Where(coordLookUp.ContainsKey)
            .Select(below => coordLookUp[below])];
    }

    private HashSet<SnowBrick> BrickWillFall(SnowBrick brick,
        Dictionary<Coordinate3D, SnowBrick> lookUpAfterExplosions)
    {
        var supportedBricks = SupportsWithDictionary(brick, lookUpAfterExplosions);
        // If this brick is the only supporter for any brick above,
        // it can not be removed
        return [.. supportedBricks.Where(
            brick => IsSupportedByWithDictionary(brick, lookUpAfterExplosions)
            .Count == 0)];
    }

    private static HashSet<SnowBrick> IsSupportedByWithDictionary(
        SnowBrick brick,
        Dictionary<Coordinate3D, SnowBrick> lookUpAfterExplosions)
    {
        var coordsBelow = brick.CoordsBelow();
        return [.. coordsBelow
            .Where(lookUpAfterExplosions.ContainsKey)
            .Select(below => lookUpAfterExplosions[below])];
    }

    private HashSet<SnowBrick> SupportsWithDictionary(SnowBrick brick,
        Dictionary<Coordinate3D, SnowBrick> lookUpAfterExplosions)
    {
        var coordsBelow = brick.CoordsAbove();
        return [.. coordsBelow
            .Where(lookUpAfterExplosions.ContainsKey)
            .Select(below => lookUpAfterExplosions[below])];
    }

    public long Part2()
    {
        // For each brick determine how many would fall if we were to zap
        // that brick
        // Then sum those numbers
        DropBricks();

        // Optimization possible: start at the bottom and keep track of bricks already exploded
        // Save their values
        return allBricks.Sum(brick => ChainReactionLengthByRemoving(brick));
    }

    public long ChainReactionLengthByRemoving(SnowBrick brick)
    {
        long counter = 0;

        // Breadth first search for bricks that are only supported by this brick
        HashSet<SnowBrick> exploded = [brick];

        // Copy the original dictionary to modify it
        Dictionary<Coordinate3D, SnowBrick> lookUpAfterExplosions = CopyLookUpDict();

        while (exploded.Count > 0)
        {
            RemoveBricksFromDict(ref lookUpAfterExplosions, exploded);
            exploded = PropagateExplosion(exploded, lookUpAfterExplosions);
            counter += exploded.Count;
        }

        return counter;
    }

    private HashSet<SnowBrick> PropagateExplosion(
        HashSet<SnowBrick> exploded,
        Dictionary<Coordinate3D, SnowBrick> lookUpAfterExplosions)
    {
        HashSet<SnowBrick> newExplosions = new();
        foreach (SnowBrick latestExplosion in exploded)
        {
            foreach (SnowBrick newBrick in BrickWillFall(latestExplosion, lookUpAfterExplosions))
            {
                newExplosions.Add(newBrick);
            }
        }

        return newExplosions;
    }

    private Dictionary<Coordinate3D, SnowBrick> CopyLookUpDict()
    {
        Dictionary<Coordinate3D, SnowBrick> lookUpAfterExplosions = new();
        foreach (var keyValuePair in coordLookUp)
        {
            lookUpAfterExplosions.Add(keyValuePair.Key, keyValuePair.Value);
        }

        return lookUpAfterExplosions;
    }

    private static void RemoveBricksFromDict(
        ref Dictionary<Coordinate3D, SnowBrick> lookUpAfterExplosions,
        HashSet<SnowBrick> newExplosions)
    {
        foreach (SnowBrick newlyExploded in newExplosions)
        {
            foreach (Coordinate3D explosionCoord in newlyExploded.AllCoords())
            {
                lookUpAfterExplosions.Remove(explosionCoord);
            }
        }
    }
}

public class Day22
{
    public static long Part1(string input)
    {
        SnowField field = new(input);
        return field.Part1();
    }

    public static long Part2(string input)
    {
        SnowField field = new(input);
        return field.Part2();
    }
}