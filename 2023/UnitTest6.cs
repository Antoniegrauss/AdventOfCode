namespace AoC23.Day6;

using System.IO;

public class UnitTest6
{
    [Fact]
    public void Day6Test1()
    {
        string test_input =
@"Time:      7  15   30
Distance:  9  40  200";

        long result = Day6.Part1(test_input);

        long test_answer = 288;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day6.txt");
        long result = Day6.Part1(test_input);

        Assert.Equal(2756160, result);
    }

    [Fact]
    public void Day6Test2()
    {
        string test_input =
@"Time:      7  15   30
Distance:  9  40  200";

        long result = Day6.Part2(test_input);

        long test_answer = 71503;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day6.txt");
        long result = Day6.Part2(test_input);

        Assert.Equal(34788142, result);
    }
}
