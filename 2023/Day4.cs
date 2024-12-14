namespace AoC23.Day4;

public class ScratchCardGame
{
    public List<int> winning_numbers = [];
    public List<int> our_numbers = [];
    public int num_winning = -1;

    public static ScratchCardGame ParseFromString(string line)
    {
        // Remove the 'Card x:' part
        line = line.Split(':')[1];

        string[] parts = line.Trim().Split('|');

        ScratchCardGame game = new();
        string[] winning = parts[0].Trim().Split(' ');
        string[] ours = parts[1].Trim().Split(' ');

        string interim;
        foreach (string win in winning)
        {
            interim = win.Trim();
            if (interim.Length > 0)
            {
                game.winning_numbers.Add(int.Parse(interim));
            }
        }
        foreach (string our in ours)
        {
            interim = our.Trim();
            if (interim.Length > 0)
            {
                game.our_numbers.Add(int.Parse(interim));
            }
        }

        return game;
    }

    public int CalculateScore()
    {
        CalculateNumWinning();
        return (int)Math.Pow(2, num_winning - 1);
    }

    public int CalculateNumWinning()
    {
        if (num_winning != -1)
        {
            return num_winning;
        }
        num_winning = winning_numbers.Intersect(our_numbers).Count();
        return num_winning;
    }
}

public class Day4
{

    public static List<ScratchCardGame> ParseGames(string input)
    {
        List<ScratchCardGame> games = [];
        input = input.Replace("\r", "");
        foreach (string line in input.Split("\n"))
        {
            games.Add(ScratchCardGame.ParseFromString(line));
        }

        return games;
    }

    public static int Part1(string input)
    {

        List<ScratchCardGame> games = ParseGames(input);
        int sum = 0;
        foreach (ScratchCardGame game in games)
        {
            sum += game.CalculateScore();
        }

        return sum;
    }

    public static List<int> WinNewCards(int id, int score)
    {
        List<int> won_cards = [];
        for (int i = 1; i <= score; i++)
        {
            won_cards.Add(id + i);
        }

        return won_cards;
    }

    public static int Part2(string input)
    {
        List<ScratchCardGame> games = ParseGames(input);
        var cards_to_score = new PriorityQueue<int, int>();
        int total_cards = games.Count;

        // Start with one copy of each card
        foreach (int starting_card_id in Enumerable.Range(1, games.Count))
        {
            cards_to_score.Enqueue(starting_card_id, starting_card_id);
        }

        int num_winning = -1;
        int new_id = -1;
        while (cards_to_score.Count != 0)
        {
            new_id = cards_to_score.Dequeue();
            num_winning = games[new_id - 1].CalculateNumWinning();
            total_cards += num_winning;

            foreach (int new_card in WinNewCards(id: new_id, score: num_winning))
            {
                cards_to_score.Enqueue(new_card, new_card);
            }
        }

        return total_cards;
    }
}