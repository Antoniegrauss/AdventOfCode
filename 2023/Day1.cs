using Microsoft.VisualStudio.TestPlatform.Common.Utilities;

namespace AoC23.Day1;

public struct SearchItem
{
    public string _search_term;
    public int _value;

    public SearchItem(string search_term, int value)
    {
        _search_term = search_term;
        _value = value;
    }
}

public class Day1
{

    public static List<SearchItem> SEARCH_ITEMS = [
        new SearchItem("one", 1),
         new SearchItem("two", 2),
         new SearchItem("three", 3),
         new SearchItem("four", 4),
         new SearchItem("five", 5),
         new SearchItem("six", 6),
         new SearchItem("seven", 7),
         new SearchItem("eight", 8),
         new SearchItem("nine", 9),
         new SearchItem("1", 1),
         new SearchItem("2", 2),
         new SearchItem("3", 3),
         new SearchItem("4", 4),
         new SearchItem("5", 5),
         new SearchItem("6", 6),
         new SearchItem("7", 7),
         new SearchItem("8", 8),
         new SearchItem("9", 9),
    ];

    public static int Part1(string input)
    {
        string[] lines = input.Split("\n");
        int sum = 0;
        foreach (string line in lines)
        {
            char first_digit = FindFirstDigit(line);
            char last_digit = FindFirstDigit(Reverse(line));
            string result_str = first_digit.ToString() + last_digit.ToString();
            sum += Int32.Parse(result_str);
        }
        return sum;
    }

    public static int Part2(string input)
    {
        string[] lines = input.Split("\n");
        int sum = 0;
        foreach (string line in lines)
        {
            char first_digit = FindFirstDigitOrSpelledOut(line);
            char last_digit = FindLastDigitOrSpelledOut(line);
            string result_str = ((int)first_digit).ToString() + ((int)last_digit).ToString();
            sum += Int32.Parse(result_str);
        }
        return sum;

    }

    public static char FindFirstDigit(string str)
    {
        foreach (char ch in str)
        {
            if (Char.IsDigit(ch))
            {
                return ch;
            }
        }
        return 'x';
    }

    // Use with IndexOf or LastIndexOff
    public static char FindFirstDigitOrSpelledOut(string str)
    {
        int lowest_index = str.Length + 1;
        int value = -1;
        int current_index;
        foreach (SearchItem search in SEARCH_ITEMS)
        {
            current_index = str.IndexOf(search._search_term);
            if (current_index == -1)
            {
                continue;
            }

            if (current_index < lowest_index)
            {
                lowest_index = current_index;
                value = search._value;
            }
        }

        return (char)value;
    }

    public static char FindLastDigitOrSpelledOut(string str)
    {
        int highest_index = -1;
        int value = -1;
        int current_index;
        foreach (SearchItem search in SEARCH_ITEMS)
        {
            current_index = str.LastIndexOf(search._search_term);
            if (current_index == -1)
            {
                continue;
            }

            if (current_index > highest_index)
            {
                highest_index = current_index;
                value = search._value;
            }
        }

        return (char)value;
    }

    public static string Reverse(string str)
    {
        var chars = str.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }
}