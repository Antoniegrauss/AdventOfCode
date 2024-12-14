using Xunit;
using Xunit.Abstractions;
using System.IO;

namespace AoC23.Day15;

public class UnitTest15
{
    private readonly ITestOutputHelper output;

    public UnitTest15(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Day15Test1()
    {
        string test_input =
@"rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7";

        // Answer should be 1 + 4 + 1 + 1 + 4 + 10 = 21

        long result = Day15.Part1(test_input);

        long test_answer = 1320;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day15.txt");
        long result = Day15.Part1(test_input);

        Assert.Equal(503487, result);
    }

    [Fact]
    public void Day15Test2()
    {
        string test_input =
@"rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7";


        long result = Day15.Part2(test_input);

        long test_answer = 145;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day15.txt");
        long result = Day15.Part2(test_input);


        Assert.Equal(261505, result);
    }
}
