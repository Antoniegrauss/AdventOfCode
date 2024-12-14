using Xunit;
using Xunit.Abstractions;
using System.IO;

namespace AoC23.Day8;

public class UnitTest8
{
    private readonly ITestOutputHelper output;

    public UnitTest8(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Day8Test1()
    {
        string test_input =
@"RL

AAA = (BBB, CCC)
BBB = (DDD, EEE)
CCC = (ZZZ, GGG)
DDD = (DDD, DDD)
EEE = (EEE, EEE)
GGG = (GGG, GGG)
ZZZ = (ZZZ, ZZZ)
";

        long result = Day8.Part1(test_input);

        long test_answer = 2;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day8TestSample1_2()
    {
        string test_input =
@"LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)
";

        long result = Day8.Part1(test_input);

        long test_answer = 6;
        Assert.Equal(test_answer, result);
    }


    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day8.txt");
        long result = Day8.Part1(test_input);

        Assert.Equal(13019, result);
    }

    [Fact]
    public void Day8Test2()
    {
        string test_input =
@"LR

11A = (11B, XXX)
11B = (XXX, 11Z)
11Z = (11B, XXX)
22A = (22B, XXX)
22B = (22C, 22C)
22C = (22Z, 22Z)
22Z = (22B, 22B)
XXX = (XXX, XXX)";

        long result = Day8.Part2(test_input);

        long test_answer = 6;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day8.txt");
        long result = Day8.Part2(test_input);

        Assert.Equal(13524038372771, result);
    }
}
