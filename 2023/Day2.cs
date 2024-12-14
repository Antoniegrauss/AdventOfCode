namespace AoC23.Day2;

public struct Pull
{
    public Dictionary<string, int> _color_amounts;

    public Pull()
    {
        _color_amounts = [];
    }

    public static List<Pull> ParsePulls(string input)
    {
        string[] pulls = input.Split(';');
        List<Pull> parsed_pulls = [];

        foreach (string pull in pulls)
        {
            parsed_pulls.Add(ParseSinglePull(pull));
        }

        return parsed_pulls;
    }

    public static Pull ParseSinglePull(string input)
    {
        string[] colors = input.Split(',');
        string[] single_color;

        Pull pull = new();
        foreach (string color in colors)
        {
            single_color = color.Trim().Split(' ');
            pull._color_amounts.Add(single_color[1], Int32.Parse(single_color[0]));
        }

        return pull;
    }
}


public class Game(Dictionary<string, int> balls, List<Pull> pulls, int id)
{
    public int _id = id;
    public Dictionary<string, int> _avaible_balls = balls;
    public List<Pull> _pulls = pulls;

    public bool CheckPullPossible(Pull pull)
    {
        foreach (var color_pull in pull._color_amounts)
        {
            if (!_avaible_balls.ContainsKey(color_pull.Key))
            {
                return false;
            }
            if (_avaible_balls[color_pull.Key] < color_pull.Value)
            {
                return false;
            }
        }
        return true;
    }

    public bool AllPullsPossible()
    {
        foreach (Pull pull in _pulls)
        {
            if (!CheckPullPossible(pull))
            {
                return false;
            }
        }
        return true;
    }

    public int CalculatePowerScore()
    {
        Dictionary<string, int> max_color_values = [];
        foreach (Pull pull in _pulls)
        {
            foreach (var item in pull._color_amounts)
            {
                if (!max_color_values.ContainsKey(item.Key))
                {
                    max_color_values.Add(item.Key, item.Value);
                    continue;
                }

                if (max_color_values[item.Key] < item.Value)
                {
                    max_color_values[item.Key] = item.Value;
                }
            }
        }

        return PowerScore(max_color_values);
    }

    public static Game ParseGame(string line)
    {
        string[] game_and_pulls = line.Split(":");
        List<Pull> pulls = Pull.ParsePulls(game_and_pulls[1]);

        Dictionary<string, int> available_balls = new Dictionary<string, int>();
        available_balls.Add("red", 12);
        available_balls.Add("green", 13);
        available_balls.Add("blue", 14);

        string[] split = game_and_pulls[0].Split(' ');
        int id = Int32.Parse(split[1]);

        return new Game(available_balls, pulls, id);
    }

    public static int PowerScore(Dictionary<string, int> balls)
    {
        int sum = 1;
        foreach (var Value in balls.Values)
        {
            sum *= Value;
        }
        return sum;
    }
}

public class Day2
{

    public static int Part1(string input)
    {
        List<Game> games = [];
        input = input.Replace('\r', ' ');
        foreach (string line in input.Split('\n'))
        {
            games.Add(Game.ParseGame(line));
        }

        int sum = 0;
        foreach (Game game in games)
        {
            if (game.AllPullsPossible())
            {
                sum += game._id;
            }
        }

        return sum;
    }

    public static int Part2(string input)
    {
        List<Game> games = [];
        input = input.Replace('\r', ' ');
        foreach (string line in input.Split('\n'))
        {
            games.Add(Game.ParseGame(line));
        }

        int sum = 0;
        foreach (Game game in games)
        {
            // Power for each game given the pulls
            sum += game.CalculatePowerScore();
        }

        return sum;
    }
}