using Xunit;
using Xunit.Abstractions;
using System.IO;

namespace AoC23.Day14;

public class UnitTest14
{
    private readonly ITestOutputHelper output;

    public UnitTest14(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Day14Test1()
    {
        string test_input =
@"O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....";

        // Answer should be 1 + 4 + 1 + 1 + 4 + 10 = 21

        long result = Day14.Part1(test_input);

        long test_answer = 136;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day14.txt");
        long result = Day14.Part1(test_input);

        Assert.Equal(105784, result);
    }

    [Fact]
    public void Day14Test2()
    {
        string test_input =
@"O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....";

        long result = Day14.Part2(test_input);

        long test_answer = 64;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day14.txt");
        long result = Day14.Part2(test_input);

        Assert.Equal(91286, result);
    }
}
