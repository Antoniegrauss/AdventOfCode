using Xunit;
using Xunit.Abstractions;
using System.IO;

namespace AoC23.Day22;

public class UnitTest22
{
    private readonly ITestOutputHelper output;

    public UnitTest22(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Day22Test1()
    {
        string test_input =
@"1,0,1~1,2,1
0,0,2~2,0,2
0,2,3~2,2,3
0,0,4~0,2,4
2,0,5~2,2,5
0,1,6~2,1,6
1,1,8~1,1,9";


        long result = Day22.Part1(test_input);
        Assert.Equal(5, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day22.txt");
        long result = Day22.Part1(test_input);

        Assert.Equal(454, result);
    }

    [Fact]
    public void Day22Test2()
    {
        string test_input =
@"1,0,1~1,2,1
0,0,2~2,0,2
0,2,3~2,2,3
0,0,4~0,2,4
2,0,5~2,2,5
0,1,6~2,1,6
1,1,8~1,1,9";

        long result = Day22.Part2(test_input);
        Assert.Equal(7, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day22.txt");
        long result = Day22.Part2(test_input);

        Assert.Equal(74287, result);
    }
}
