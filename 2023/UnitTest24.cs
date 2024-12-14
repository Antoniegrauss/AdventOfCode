using System.IO;
using Xunit;
using Xunit.Abstractions;
using System.IO;

namespace AoC23.Day24;

public class UnitTest24
{
    private readonly ITestOutputHelper output;

    public UnitTest24(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Day24Test1_1()
    {
        string test_input =
@"19, 13, 30 @ -2,  1, -2
18, 19, 22 @ -1, -1, -2
";

        long result = Day24.Part1(test_input, new Bounds { min = 7, max = 27 });
        Assert.Equal(1, result);
    }

    [Fact]
    public void Day24Test1_2()
    {
        string test_input =
@"19, 13, 30 @ -2,  1, -2
20, 19, 15 @  1, -5, -3
";

        long result = Day24.Part1(test_input, new Bounds { min = 7, max = 27 });
        Assert.Equal(0, result);
    }

    [Fact]
    public void Day24Test1_3()
    {
        string test_input =
@"18, 19, 22 @ -1, -1, -2
20, 25, 34 @ -2, -2, -4
12, 31, 28 @ -1, -2, -1
20, 19, 15 @  1, -5, -3
";

        long result = Day24.Part1(test_input, new Bounds { min = 7, max = 27 });
        Assert.Equal(0, result);
    }

    [Fact]
    public void Day24Test1_4()
    {
        string test_input =
@"19, 13, 30 @ -2,  1, -2
18, 19, 22 @ -1, -1, -2
20, 25, 34 @ -2, -2, -4
12, 31, 28 @ -1, -2, -1
20, 19, 15 @  1, -5, -3
";

        long result = Day24.Part1(test_input, new Bounds { min = 7, max = 27 });
        Assert.Equal(2, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day24.txt");
        long result = Day24.Part1(
            test_input,
            new Bounds { min = 200000000000000, max = 400000000000000 });

        Assert.Equal(0, result);
    }

    // Used https://pastebin.com/pnbxaCVu for the solution instead
    // [Fact]
    // public void FullTestPart2()
    // {
    //     string PATH_TO_INPUT = "C:\\Users\\AntonieGrauss\\CSharpProjects\\AoC24\\AoC24\\inputs\\";
    //     string test_input = File.ReadAllText(PATH_TO_INPUT + "Day24.txt");
    //     long result = Day24.Part2(test_input,
    //         new Bounds { min = 200000000000000, max = 400000000000000 });

    //     Assert.Equal(0, result);
    // }
}
