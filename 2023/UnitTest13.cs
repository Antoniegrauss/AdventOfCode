using Xunit;
using Xunit.Abstractions;
using System.IO;

namespace AoC23.Day13;

public class UnitTest13
{
    private readonly ITestOutputHelper output;

    public UnitTest13(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Day13Test1()
    {
        string test_input =
@"#.##..##.
..#.##.#.
##......#
##......#
..#.##.#.
..##..##.
#.#.##.#.

#...##..#
#....#..#
..##..###
#####.##.
#####.##.
..##..###
#....#..#";

        // Answer should be 1 + 4 + 1 + 1 + 4 + 10 = 21

        long result = Day13.Part1(test_input);

        long test_answer = 405;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day13.txt");
        long result = Day13.Part1(test_input);

        Assert.Equal(29130, result);
    }

    [Fact]
    public void Day13Test2()
    {
        string test_input =
@"#.##..##.
..#.##.#.
##......#
##......#
..#.##.#.
..##..##.
#.#.##.#.

#...##..#
#....#..#
..##..###
#####.##.
#####.##.
..##..###
#....#..#";

        long result = Day13.Part2(test_input);

        long test_answer = 400;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day13.txt");
        long result = Day13.Part2(test_input);

        Assert.Equal(33438, result);
    }
}
