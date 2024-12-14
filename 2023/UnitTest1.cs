namespace AoC23.Day1;

using System.IO;

public class UnitTest1
{
    [Fact]
    public void Day1Test1()
    {
        string test_input =
@"1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet";

        int result = Day1.Part1(test_input);

        int test_answer = 142;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day1.txt");
        int result = Day1.Part1(test_input);

        Assert.Equal(54634, result);
    }

    [Fact]
    public void Day1Test2()
    {
        string test_input =
@"two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen";

        int result = Day1.Part2(test_input);

        int test_answer = 281;
        Assert.Equal(test_answer, result);
    }


    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day1.txt");
        int result = Day1.Part2(test_input);

        Assert.Equal(53855, result);
    }
}
