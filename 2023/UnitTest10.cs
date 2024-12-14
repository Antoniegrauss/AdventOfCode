using Xunit;
using Xunit.Abstractions;
using System.IO;

namespace AoC23.Day10;

using System.IO;

public class UnitTest10
{
    private readonly ITestOutputHelper output;

    public UnitTest10(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Day10Test1()
    {
        string test_input =
@"..F7.
.FJ|.
SJ.L7
|F--J
LJ...";

        long result = Day10.Part1(test_input);

        long test_answer = 8;
        Assert.Equal(test_answer, result);
    }


    [Fact]
    public void Day10Test1_2()
    {
        string test_input =
@"-L|F7
7S-7|
L|7||
-L-J|
L|-JF";

        long result = Day10.Part1(test_input);

        long test_answer = 4;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day10.txt");
        long result = Day10.Part1(test_input);

        Assert.Equal(6768, result);
    }

    [Fact]
    public void Day10CoordinateEqualsTest()
    {
        Coordinate coord1 = new Coordinate { x = 3, y = 5 };
        Coordinate coord2 = new Coordinate { x = 3, y = 5 };
        Coordinate coord3 = new Coordinate { x = 4, y = 5 };

        Assert.True(coord1.Equals(coord2));
        Assert.False(coord1.Equals(coord3));
    }

    [Fact]
    public void Day10Test2()
    {
        string test_input =
@"...........
.S-------7.
.|F-----7|.
.||.....||.
.||.....||.
.|L-7.F-J|.
.|..|.|..|.
.L--J.L--J.
...........";

        long result = Day10.Part2(test_input);

        long test_answer = 4;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day10Test2_2()
    {
        string test_input =
@"..........
.S------7.
.|F----7|.
.||....||.
.||....||.
.|L-7F-J|.
.|..||..|.
.L--JL--J.
..........";

        long result = Day10.Part2(test_input);

        long test_answer = 4;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day10.txt");
        long result = Day10.Part2(test_input);

        // Lower than 358
        // Higher than 350
        Assert.Equal(351, result);
    }
}
