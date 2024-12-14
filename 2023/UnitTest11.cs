using Xunit;
using Xunit.Abstractions;
using System.IO;

namespace AoC23.Day11;

public class UnitTest11
{
    private readonly ITestOutputHelper output;

    public UnitTest11(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Day11Test1()
    {
        string test_input =
@"...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....";

        long result = Day11.Part1(test_input);

        long test_answer = 374;
        Assert.Equal(test_answer, result);
    }


    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day11.txt");
        long result = Day11.Part1(test_input);

        Assert.Equal(9565386, result);
    }

    [Fact]
    public void Day11Test2()
    {
        string test_input =
@"...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....";

        long result = Day11.Part2(test_input, 10);

        long test_answer = 1030;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day11.txt");
        long result = Day11.Part2(test_input, 1_000_000);

        // Lower than 358
        // Higher than 350
        Assert.Equal(857986849428, result);
    }
}
