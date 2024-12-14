namespace AoC23.Day7;

using System.IO;

public class UnitTest7
{
    [Fact]
    public void Day7Test1()
    {
        string test_input =
@"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483";

        long result = Day7.Part1(test_input);

        long test_answer = 6440;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day7Test1_2()
    {
        string test_input =
@"25555 40
23555 30
23455 20
23457 10
55555 50
23456 5";

        long result = Day7.Part1(test_input);

        long test_answer = 5 + 2 * 10 + 3 * 20 + 4 * 30
            + 5 * 40 + 6 * 50;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day7Test1_3()
    {
        string test_input =
@"55789 10
12366 5";

        long result = Day7.Part1(test_input);

        long test_answer = 1 * 5 + 2 * 10;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day7Test1_4()
    {
        string test_input =
@"55522 10
55598 5";

        long result = Day7.Part1(test_input);

        long test_answer = 1 * 5 + 2 * 10;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void CompareCardsTest()
    {
        CamelCard fiveOfAKind = new("TTTTT 684");
        CamelCard fourOfAKind = new("TTTT3 1");

        Assert.Equal(1, fiveOfAKind.CompareTo(fourOfAKind));
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day7.txt");
        long result = Day7.Part1(test_input);

        Assert.Equal(250453939, result);
    }

    [Fact]
    public void Day7Test2()
    {
        string test_input =
@"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483";

        long result = Day7.Part2(test_input);

        long test_answer = 5905;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day7Test2_2()
    {
        string test_input =
@"J2345 5
22345 10";

        long result = Day7.Part2(test_input);

        long test_answer = 5 + 20;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day7Test2_3()
    {
        string test_input =
@"J234J 5
22345 10";

        long result = Day7.Part2(test_input);

        long test_answer = 10 + 10;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day7.txt");
        long result = Day7.Part2(test_input);

        Assert.Equal(248652697, result);
    }
}
