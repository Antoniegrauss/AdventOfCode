using Xunit;
using Xunit.Abstractions;
using System.IO;

namespace AoC23.Day17;

public class UnitTest17
{
    private readonly ITestOutputHelper output;

    public UnitTest17(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Day17Test1()
    {
        string test_input =
@"2413432311323
3215453535623
3255245654254
3446585845452
4546657867536
1438598798454
4457876987766
3637877979653
4654967986887
4564679986453
1224686865563
2546548887735
4322674655533";

        long result = Day17.Part1(test_input);

        long test_answer = 102;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day17Test1_2()
    {
        string test_input =
@"111
199
199";

        long result = Day17.Part1(test_input);

        long test_answer = 20;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day17Test1_3()
    {
        string test_input =
@"11111
99999";

        long result = Day17.Part1(test_input);

        long test_answer = 21;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day17Test1_5()
    {
        string test_input =
@"
129911
111111";

        long result = Day17.Part1(test_input);

        long test_answer = 9;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day17Test1_6()
    {
        string test_input =
@"
119911
111111";

        long result = Day17.Part1(test_input);

        long test_answer = 8;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day17TestPerformance25x25()
    {
        string test_input = "";
        int gridSize = 25;
        for (int i = 0; i < gridSize; i++)
        {
            test_input += new string('9', gridSize) + Environment.NewLine;
        }

        long result = Day17.Part1(test_input);

        long test_answer = 432;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day17TestPerformance50x50()
    {
        string test_input = "";
        int gridSize = 50;
        for (int i = 0; i < gridSize; i++)
        {
            test_input += new string('9', gridSize) + Environment.NewLine;
        }

        long result = Day17.Part1(test_input);

        long test_answer = 882;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day17TestPerformance55x55()
    {
        string test_input = "";
        int gridSize = 55;
        for (int i = 0; i < gridSize; i++)
        {
            test_input += new string('5', gridSize) + Environment.NewLine;
        }

        long result = Day17.Part1(test_input);

        long test_answer = 540;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day17TestPerformance60x60()
    {
        string test_input = "";
        int gridSize = 60;
        for (int i = 0; i < gridSize; i++)
        {
            test_input += new string('1', gridSize) + Environment.NewLine;
        }

        long result = Day17.Part1(test_input);

        long test_answer = 118;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day17TestPerformance75x75()
    {
        string test_input = "";
        int gridSize = 75;
        for (int i = 0; i < gridSize; i++)
        {
            test_input += new string('1', gridSize) + Environment.NewLine;
        }

        long result = Day17.Part1(test_input);

        long test_answer = 148;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day17TestPerformance100x100()
    {
        string test_input = "";
        int gridSize = 100;
        for (int i = 0; i < gridSize; i++)
        {
            test_input += new string('9', gridSize) + Environment.NewLine;
        }

        long result = Day17.Part1(test_input);

        long test_answer = 1782;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day17TestPerformance200x200()
    {
        string test_input = "";
        int gridSize = 200;
        for (int i = 0; i < gridSize; i++)
        {
            test_input += new string('1', gridSize) + Environment.NewLine;
        }

        long result = Day17.Part1(test_input);

        long test_answer = 398;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day17.txt");
        long result = Day17.Part1(test_input);

        Assert.Equal(1244, result);
    }

    [Fact]
    public void Day17Test2_1()
    {
        string test_input =
@"2413432311323
3215453535623
3255245654254
3446585845452
4546657867536
1438598798454
4457876987766
3637877979653
4654967986887
4564679986453
1224686865563
2546548887735
4322674655533";

        long result = Day17.Part2(test_input);

        long test_answer = 94;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day17Test2_2()
    {
        string test_input =
@"111111111111
999999999991
999999999991
999999999991
999999999991";

        long result = Day17.Part2(test_input);

        long test_answer = 71;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day17.txt");
        long result = Day17.Part2(test_input);


        Assert.Equal(1367, result);
    }
}
