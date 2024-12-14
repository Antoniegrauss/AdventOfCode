using Xunit;
using Xunit.Abstractions;
using System.IO;

namespace AoC23.Day9;

using System.IO;

public class UnitTest9
{
    private readonly ITestOutputHelper output;

    public UnitTest9(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Day9Test1()
    {
        string test_input =
@"0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45";

        long result = Day9.Part1(test_input);

        long test_answer = 114;
        Assert.Equal(test_answer, result);
    }


    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day9.txt");
        long result = Day9.Part1(test_input);

        Assert.Equal(1731106378, result);
    }

    [Fact]
    public void Day9Test2()
    {
        string test_input =
@"0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45";

        long result = Day9.Part2(test_input);

        long test_answer = 2;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day9.txt");
        long result = Day9.Part2(test_input);

        Assert.Equal(1087, result);
    }
}
