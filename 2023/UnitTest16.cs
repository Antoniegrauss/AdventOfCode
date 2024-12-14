using Xunit;
using Xunit.Abstractions;
using System.IO;

namespace AoC23.Day16;

public class UnitTest16
{
    private readonly ITestOutputHelper output;

    public UnitTest16(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Day16Test1()
    {
        string test_input =
@".|...\....
|.-.\.....
.....|-...
........|.
..........
.........\
..../.\\..
.-.-/..|..
.|....-|.\
..//.|....";

        // Answer should be 1 + 4 + 1 + 1 + 4 + 10 = 21

        long result = Day16.Part1(test_input);

        long test_answer = 46;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day16.txt");
        long result = Day16.Part1(test_input);

        Assert.Equal(7632, result);
    }

    [Fact]
    public void Day16Test2()
    {
        string test_input =
@".|...\....
|.-.\.....
.....|-...
........|.
..........
.........\
..../.\\..
.-.-/..|..
.|....-|.\
..//.|....";

        long result = Day16.Part2(test_input);

        long test_answer = 51;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day16.txt");
        long result = Day16.Part2(test_input);


        Assert.Equal(8023, result);
    }
}
