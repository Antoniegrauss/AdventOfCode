namespace AoC23.Day6;

public class ToyBoatRace(long time, long record)
{
    public long _time = time;
    public long _record = record;

    public long WaysToWin()
    {
        long score = 0;
        for (long i = 0; i <= _time; i++)
        {
            long speed = i;
            long remaining_time = _time - i;
            long distance = speed * remaining_time;
            if (distance > _record)
            {
                score += 1;
            }
        }
        return score;
    }
}

public class Day6
{
    public static int Part1(string input)
    {
        string[] parts = input.Split(Environment.NewLine);
        List<int> times = parts[0].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Skip(1)
        .Select(s => int.Parse(s))
        .ToList();
        List<int> distances = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Skip(1)
        .Select(s => int.Parse(s))
        .ToList();

        var races = new List<ToyBoatRace>();
        foreach (var items in times.Zip(distances))
        {
            races.Add(new ToyBoatRace(time: items.First, record: items.Second));
        }

        long total_score = 1;
        foreach (ToyBoatRace race in races)
        {
            total_score *= race.WaysToWin();
        }
        return (int)total_score;
    }
    public static long Part2(string input)
    {
        string[] parts = input.Split(Environment.NewLine);
        long time = long.Parse(String.Join("", parts[0].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Skip(1)));
        long distance = long.Parse(String.Join("", parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Skip(1)));

        var race = new ToyBoatRace(time: time, record: distance);
        return race.WaysToWin();
    }
}