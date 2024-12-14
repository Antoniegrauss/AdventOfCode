namespace AoC23.Day15;

public class Lens : IEquatable<Lens>
{
    public string id;
    public int strength;

    public bool Equals(Lens? other)
    {
        return id == other.id;
    }

    public int Hash()
    {
        int current = 0;
        foreach (char c in id)
        {
            current += (int)c;
            current *= 17;
            current %= 256;
        }
        return current;
    }
}

public class LensBox
{
    List<Lens> lenses = new();

    public int LensPower()
    {
        return lenses.Index().Sum(tup => tup.Item.strength * (tup.Index + 1));
    }

    public void Add(Lens lens)
    {
        if (lenses.Contains(lens))
        {
            lenses.Where(s => s.id == lens.id).ToList().ForEach(s => s.strength = lens.strength);
            return;
        }
        lenses.Add(lens);
    }

    public void Remove(Lens lens)
    {
        lenses.Remove(lens);
    }
}

public class LensBoxSeries
{
    List<LensBox> boxes = [];

    public LensBoxSeries()
    {
        for (int i = 0; i < 256; i++)
        {
            boxes.Add(new LensBox());
        }
    }

    public int FocusingPower()
    {
        return boxes.Select(lens => lens.LensPower()).Index().Sum(tup => tup.Item * (tup.Index + 1));
    }

    public void AddLens(string newLens)
    {
        // Add new lens
        Lens lens = new();
        lens.id = newLens.Split(new Char[] { '-', '=' })[0];

        int boxIndex = lens.Hash();

        if (newLens.Contains('='))
        {
            lens.strength = newLens[^1] - '0';
            boxes[boxIndex].Add(lens);
        }
        else if (newLens.Contains('-'))
        {
            // Remove this lens if present
            boxes[boxIndex].Remove(lens);
        }
        else
        {
            throw new Exception("Lens should contain = or -");
        }
    }
}

public class Day15
{
    public static long HashFunction(string input)
    {
        long current = 0;
        foreach (char c in input)
        {
            current += (int)c;
            current *= 17;
            current %= 256;
        }
        return current;
    }
    public static long Part1(string input)
    {
        long total = input.Split(
            ",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Sum(s => HashFunction(s));
        return total;
    }

    public static long Part2(string input)
    {
        LensBoxSeries boxes = new();
        foreach (string lens in input.Split(
            ",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            boxes.AddLens(lens);
        }
        return boxes.FocusingPower();
    }
}