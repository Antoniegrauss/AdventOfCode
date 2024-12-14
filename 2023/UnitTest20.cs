using Xunit;
using Xunit.Abstractions;
using System.IO;

namespace AoC23.Day20;

public class UnitTest20
{
    private readonly ITestOutputHelper output;

    public UnitTest20(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Day20Test1()
    {
        string test_input =
@"broadcaster -> a, b, c
%a -> b
%b -> c
%c -> inv
&inv -> a";

        long result = Day20.Part1(test_input);

        long test_answer = 32000000;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day20Test1_2()
    {
        string test_input =
@"broadcaster -> a
%a -> inv, con
&inv -> b
%b -> con
&con
";

        long result = Day20.Part1(test_input);

        long test_answer = 6562500;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day20Test1_3()
    {
        string test_input =
@"broadcaster -> a
%a -> inv, con
&inv -> b
%b -> con
&con -> output
";

        long result = Day20.Part1(test_input);

        long test_answer = 11687500;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day20.txt");
        long result = Day20.Part1(test_input);

        Assert.Equal(825167435, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day20.txt");

        List<string> conjunctionModules = ["pl", "mz", "lz", "zm"];
        // List<string> conjunctionModules = ["qt", "dq", "vt", "nl"];
        long result = Day20.Part2(test_input, conjunctionModules);

        Assert.Equal(225514321828633, result);
    }
}
