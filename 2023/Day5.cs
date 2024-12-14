using System.ComponentModel.DataAnnotations;

namespace AoC23.Day5;

public struct SourceDestinationRange
{
    public Int64 source_start;
    public Int64 destination_start;
    public Int64 range_length;

    public Int64 SourceEnd() {
        return source_start + range_length - 1;
    }

    public Int64 DestinationEnd() {
        return destination_start + range_length - 1;
    }

    public SourceDestinationRange(string input)
    {
        string[] numbers = input.Split(" ");
        destination_start = Int64.Parse(numbers[0]);
        source_start = Int64.Parse(numbers[1]);
        range_length = Int64.Parse(numbers[2]);
    }

    public bool IsInRange(Int64 number)
    {
        return number >= source_start && number <= SourceEnd();
    }

    public Int64 MapNumber(Int64 number)
    {
        if (!IsInRange(number))
        {
            throw new Exception();
        }

        return number - source_start + destination_start;
    }

    public bool DoIntersect(SeedRange range)
    {
        // Entire seed range is below transformation range
        if (range.start < source_start && range.End() < source_start)
        {
            return false;
        }
        // Seed range starts above transformation range
        if (range.start > SourceEnd())
        {
            return false;
        }
        return true;
    }

    public SplitSeedRange SplitSeedRange(SeedRange range)
    {
        SplitSeedRange split_ranges = new();
        split_ranges.transformed = new(0, range.length);

        // If source start if in the seed range, split off the start
        if (range.start < source_start)
        {
            split_ranges.transformed.start = source_start;
            split_ranges.non_transformed.Add(new SeedRange(start_: range.start, length_: source_start - range.start));

            // Start of the transformed segment is start of the destiantion range
            split_ranges.transformed.start = destination_start;

            // Shorten length of original segment
            split_ranges.transformed.length -= split_ranges.non_transformed.Last().length;
        }
        // If source start is before the range, do not split before
        else
        {
            split_ranges.transformed.start = MapNumber(range.start);
        }

        // If the source end (start + length) is in the range, split of the end
        if (SourceEnd() < range.End())
        {
            split_ranges.non_transformed.Add(new SeedRange(start_: SourceEnd() + 1, length_: range.End() - SourceEnd()));

            // Shorten length of transformed segment
            split_ranges.transformed.length -= split_ranges.non_transformed.Last().length;
        }

        return split_ranges;
    }
}

public struct SplitSeedRange {
    public SeedRange transformed = new();
    public List<SeedRange> non_transformed = [];

    public SplitSeedRange()
    {}
}

// All maps for one transformation
public class SeedMapCollection
{
    public List<SourceDestinationRange> transformations = [];

    // Parse from string
    public SeedMapCollection(string input)
    {
        string[] lines = input.Split("\r\n");
        foreach (string line in lines.Skip(1))
        {
            transformations.Add(new SourceDestinationRange(line));
        }
    }

    public Int64 TransformNumber(Int64 number)
    {
        foreach (SourceDestinationRange transform in transformations)
        {
            if (transform.IsInRange(number))
            {
                return transform.MapNumber(number);
            }
        }
        // If none of the ranges contain number, return original number
        return number;
    }

    // Transforms a range (range might be split into parts)
    public List<SeedRange> TransformRange(SeedRange range)
    {
        List<SeedRange> seed_ranges_to_check = [range];
        List<SeedRange> transformed_ranges = [];

        List<SeedRange> split_ranges = [];
        foreach (SourceDestinationRange transform in transformations)
        {
            // Keep track of all the new ranges
            split_ranges = [];
            foreach (SeedRange range_to_check in seed_ranges_to_check)
            {
                if (transform.DoIntersect(range_to_check))
                {
                    // Break up into parts (before, transformed, after)
                    SplitSeedRange new_ranges = transform.SplitSeedRange(range_to_check);
                    // Add transformed range
                    transformed_ranges.Add(new_ranges.transformed);
                    split_ranges.AddRange(new_ranges.non_transformed);

                    // Add parts before and after to new lists
                    seed_ranges_to_check = split_ranges;
                }
            }
        }
        // Add all non_transformed ranged to the list
        foreach (SeedRange non_transformed in seed_ranges_to_check)
        {
            transformed_ranges.Add(non_transformed);
        }

        return transformed_ranges;
    }
}

public class SeedRange
{
    public SeedRange()
    {
        start = 0;
        length = 0;
    }
    public SeedRange(Int64 start_, Int64 length_)
    {
        start = start_;
        length = length_;
    }
    public SeedRange(string[] input)
    {
        start = Int64.Parse(input[0]);
        length = Int64.Parse(input[1]);

    }
    public Int64 start;
    public Int64 length;

    public Int64 End()
    {
        return start + length - 1;
    }
}


public class AllSeedMaps
{
    public List<Int64> seeds = [];
    public List<SeedMapCollection> transformation_steps = [];

    public AllSeedMaps(string input)
    {
        // Split on empty lines
        string[] parts = input.Split("\r\n\r\n");
        seeds = ParseSeeds(parts[0]);

        foreach (string part in parts.Skip(1))
        {
            transformation_steps.Add(new SeedMapCollection(part));
        }
    }

    public static List<Int64> ParseSeeds(string input)
    {
        List<Int64> seeds = [];

        string[] numbers = input.Split(" ");
        foreach (string number in numbers.Skip(1))
        {
            seeds.Add(Int64.Parse(number));
        }

        return seeds;
    }

    public Int64 FindSeedPlantLocation(Int64 seed)
    {
        Int64 location = seed;
        foreach (SeedMapCollection map in transformation_steps)
        {
            location = map.TransformNumber(location);
        }
        return location;
    }

    public Int64 FindLowestSeedPlantLocation()
    {
        List<Int64> locations = [];
        foreach (Int64 seed in seeds)
        {
            locations.Add(FindSeedPlantLocation(seed));
        }
        return locations.Min();
    }
}

public class AllSeedMapsPart2
{
    List<SeedRange> seed_ranges = [];
    public List<SeedMapCollection> transformation_steps = [];

    public AllSeedMapsPart2(string input)
    {
        // Split on empty lines
        string[] parts = input.Split("\r\n\r\n");
        ParseSeeds(parts[0]);

        foreach (string part in parts.Skip(1))
        {
            transformation_steps.Add(new SeedMapCollection(part));
        }
    }

    public void ParseSeeds(string input)
    {
        string[] numbers = input.Split(" ");
        foreach (string[] number in numbers.Skip(1).Chunk(2))
        {
            seed_ranges.Add(new SeedRange(number));
        }

    }

    public List<SeedRange> FindSeedPlantLocation(SeedRange seed)
    {
        List<SeedRange> ranges = [seed];
        foreach (SeedMapCollection map in transformation_steps)
        {
            ranges = OneTransformationStep(ranges, map);
        }
        return ranges;
    }

    public static List<SeedRange> OneTransformationStep(List<SeedRange> ranges, SeedMapCollection map)
    {
        List<SeedRange> new_ranges = [];
        foreach (SeedRange range in ranges)
        {
            new_ranges.AddRange(map.TransformRange(range));
        }
        return new_ranges;
        
    }

    public Int64 FindLowestSeedPlantLocation()
    {
        List<SeedRange> ranges = [];
        foreach (SeedRange seed_range in seed_ranges)
        {
            ranges.AddRange(FindSeedPlantLocation(seed_range));
        }

        // Store the starts of each range, return the min of those
        List<Int64> range_start = [];
        foreach (SeedRange range in ranges)
        {
            range_start.Add(range.start);
        }
        return range_start.Min();
    }
}

public class Day5
{

    public static Int64 Part1(string input)
    {
        AllSeedMaps seed_maps = new(input);
        // TODO: transform seed numbers through transformation lists
        // Pick the lowest number
        return seed_maps.FindLowestSeedPlantLocation();
    }

    public static Int64 Part2(string input)
    {
        AllSeedMapsPart2 seed_maps = new(input);
        return seed_maps.FindLowestSeedPlantLocation();
    }
}