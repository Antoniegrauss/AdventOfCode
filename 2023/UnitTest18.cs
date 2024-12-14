using Xunit;
using Xunit.Abstractions;
using System.IO;

namespace AoC23.Day18;

public class UnitTest18
{
    private readonly ITestOutputHelper output;

    public UnitTest18(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Day18Test1()
    {
        string test_input =
@"R 6 (#70c710)
D 5 (#0dc571)
L 2 (#5713f0)
D 2 (#d2c081)
R 2 (#59c680)
D 2 (#411b91)
L 5 (#8ceee2)
U 2 (#caa173)
L 1 (#1b58a2)
U 2 (#caa171)
R 2 (#7807d2)
U 3 (#a77fa3)
L 2 (#015232)
U 2 (#7a21e3)
";

        long result = Day18.Part1(test_input);

        long test_answer = 62;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day18.txt");
        long result = Day18.Part1(test_input);

        Assert.Equal(66993, result);
    }

    [Fact]
    public void Day18Test2_1()
    {
        string test_input =
@"R 6 (#70c710)
D 5 (#0dc571)
L 2 (#5713f0)
D 2 (#d2c081)
R 2 (#59c680)
D 2 (#411b91)
L 5 (#8ceee2)
U 2 (#caa173)
L 1 (#1b58a2)
U 2 (#caa171)
R 2 (#7807d2)
U 3 (#a77fa3)
L 2 (#015232)
U 2 (#7a21e3)
";

        long result = Day18.Part2(test_input);

        long test_answer = 952408144115;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day18.txt");
        long result = Day18.Part2(test_input);


        Assert.Equal(177243763226648, result);
    }
}
