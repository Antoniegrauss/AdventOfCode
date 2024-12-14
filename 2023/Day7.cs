using System.Text.RegularExpressions;

namespace AoC23.Day7;

public class CamelCard : IComparable<CamelCard>
{
    public int _bid;
    public List<int> _cards = [];
    public List<int> _cardsWithoutJokers = [];

    public CamelCard(string line)
    {
        string[] parts = line.Split(" ", StringSplitOptions.TrimEntries);
        _bid = int.Parse(parts[1]);
        List<char> char_cards = [.. parts[0].ToCharArray()];
        foreach (char char_card in char_cards)
        {
            if (Char.IsDigit(char_card))
            {
                _cards.Add(char_card - '0');
                continue;
            }
            switch (char_card)
            {
                case 'A':
                    _cards.Add(14);
                    break;
                case 'K':
                    _cards.Add(13);
                    break;
                case 'Q':
                    _cards.Add(12);
                    break;
                case 'J':
                    _cards.Add(11);
                    break;
                case 'T':
                    _cards.Add(10);
                    break;
                default:
                    throw new Exception("Hand can not contain different symbol");
            }
        }
        _cardsWithoutJokers = _cards;
    }

    public int CompareTo(CamelCard? other)
    {
        foreach (var combinations in HandCombinations().Zip(other.HandCombinations()))
        {
            if (combinations.Second > combinations.First)
            {
                return -1;
            }
            if (combinations.First > combinations.Second)
            {
                return 1;
            }
        }

        foreach (var both_cards in _cards.Zip(other._cards))
        {
            if (both_cards.Second > both_cards.First)
            {
                return -1;
            }
            if (both_cards.First > both_cards.Second)
            {
                return 1;
            }
        }

        return 0;
    }

    public List<int> HandCombinations()
    {
        var groups = _cardsWithoutJokers.GroupBy(card => card);
        List<int> combinations = [];
        foreach (var group in groups)
        {
            combinations.Add(group.Count());
        }
        combinations.Sort();
        combinations.Reverse();
        return combinations;
    }

    public void ChangeJokers()
    {
        int isJoker = 1;
        // If we have no jokers, return
        if (!_cards.Contains(isJoker)) return;

        List<int> nonJokers = [.. _cards.Where(x => x != isJoker)];
        if (nonJokers.Distinct().Count() == 1)
        {
            _cardsWithoutJokers = [.. _cards.Select(x => nonJokers[0])];
            return;
        }

        if (nonJokers.Count == 0)
        {
            _cardsWithoutJokers = [14, 14, 14, 14, 14];
            return;
        }
        // Jokers morph into the only other card
        if (nonJokers.Count == 1)
        {
            _cardsWithoutJokers = [nonJokers[0], nonJokers[0], nonJokers[0], nonJokers[0], nonJokers[0]];
            return;
        }
        // Jokers become the highest card
        if (nonJokers.Count == 2)
        {
            int minCard = nonJokers.Min();
            int maxCard = nonJokers.Max();
            _cardsWithoutJokers = [.. _cards.Select(x => x == isJoker ? maxCard : x)];
            return;
        }

        // Case three of a kind
        var combinations = nonJokers.GroupBy(card => card);
        if (combinations.Any(grouping => grouping.Count() == 3))
        {
            int threeOfAKind = combinations.Where(grouping => grouping.Count() >= 3).ElementAt(0).Key;
            _cardsWithoutJokers = [.. _cards.Select(x => x == isJoker ? threeOfAKind : x)];
            return;
        }

        // Case 2 pair or 1 pair
        if (combinations.Any(grouping => grouping.Count() == 2))
        {
            int pairCard = combinations
                .Where(grouping => grouping.Count() == 2)
                .Max(grouping => grouping.Key);
            _cardsWithoutJokers = [.. _cards.Select(x => x == isJoker ? pairCard : x)];
            return;
        }

        // Case high card
        if (combinations.Any(grouping => grouping.Count() > 1)) throw new Exception("Unhandled case other than high card");

        int highCard = combinations
            .Max(grouping => grouping.Key);
        _cardsWithoutJokers = [.. _cards.Select(x => x == isJoker ? highCard : x)];
        return;
    }

    // Jokers are the lowest card in part 2
    public void DowngradeJokers()
    {
        _cards = [.. _cards.Select(x => x == 11 ? 1 : x)];
    }
}

public class Day7
{
    public static long ScoreHands(List<CamelCard> hands)
    {
        long sum = 0;
        for (int i = 0; i < hands.Count; i++)
        {
            long new_score = hands[i]._bid * (i + 1);
            sum += new_score;
        }
        return sum;
    }
    public static long Part1(string input)
    {
        List<CamelCard> cards = [];
        foreach (string line in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            cards.Add(new CamelCard(line));
        }
        cards.Sort();

        return ScoreHands(cards);
    }
    public static long Part2(string input)
    {
        List<CamelCard> cards = [];
        foreach (string line in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            CamelCard newCard = new CamelCard(line);
            // J are 11, but they morph into the highest group
            // If multiple same groups exist (e.g. 2 pair) choose the highest number
            newCard.DowngradeJokers();
            newCard.ChangeJokers();

            cards.Add(newCard);
        }

        cards.Sort();

        return ScoreHands(cards);
    }
}