using Xunit;
using Xunit.Abstractions;
using System.IO;

namespace AoC23.Day21;

public class UnitTest21
{
    private readonly ITestOutputHelper output;

    public UnitTest21(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Day21Test1()
    {
        string test_input =
@"...........
.....###.#.
.###.##..#.
..#.#...#..
....#.#....
.##..S####.
.##..#...#.
.......##..
.##.#.####.
.##..##.##.
...........";


        long result = Day21.Part1(test_input, steps: 6);
        Assert.Equal(16, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day21.txt");
        long result = Day21.Part1(test_input, 64);

        Assert.Equal(3762, result);
    }

    // Used python solution of https://github.com/CalSimmon/advent-of-code/blob/main/2023/day_21/solution.py instead
    // [Fact]
    // public void FullTestPart2()
    // {
    //     
    //     string test_input = File.ReadAllText(PATH_TO_INPUT + "Day21.txt");
    //     long result = Day21.Part2(test_input, 26501365);

    //     // 621944684638461 too low
    //     // 621944737236721 is too high
    //     Assert.Equal(0, result);
    // }
}
