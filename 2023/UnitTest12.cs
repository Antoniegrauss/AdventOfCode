using Xunit;
using Xunit.Abstractions;
using System.IO;

namespace AoC23.Day12;

public class UnitTest12
{
    private readonly ITestOutputHelper output;

    public UnitTest12(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Day12Test1()
    {
        string test_input =
@"???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1";

        // Answer should be 1 + 4 + 1 + 1 + 4 + 10 = 21

        long result = Day12.Part1(test_input);

        long test_answer = 21;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day12Test1_2()
    {
        string test_input =
@"?###???????? 3,2,1";

        long result = Day12.Part1(test_input);

        long test_answer = 10;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day12Test1_3()
    {
        string test_input =
@"??????? 2,1";

        long result = Day12.Part1(test_input);

        long test_answer = 10;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day12Test1_4()
    {
        string test_input =
@"???? 2,1";

        long result = Day12.Part1(test_input);

        long test_answer = 1;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day12Test1_5()
    {
        string test_input =
@"????? 2,1";

        long result = Day12.Part1(test_input);

        long test_answer = 3;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day12Test1_6()
    {
        string test_input =
@".????#?.??? 1,3,3
?#??#?##??.??? 7,1,1
.???.??.?? 1,1,1
##?.#????.???? 2,2,1,1
?#?#.??.??. 4,2";

        long result = Day12.Part1(test_input);

        long test_answer = 36;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day12Test1_7()
    {
        string test_input =
@"??????????.??? 7,1,1";

        long result = Day12.Part1(test_input);

        long test_answer = 7 + 4 + 2;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day12Test1_8()
    {
        string test_input =
@"?#??#?##??.??? 7,1,1";

        long result = Day12.Part1(test_input);

        long test_answer = 4;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day12Test1_9()
    {
        string test_input =
@"?#??#?##?? 7";

        long result = Day12.Part1(test_input);

        long test_answer = 1;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day12Test1_10()
    {
        string test_input =
@"?????????? 7";

        long result = Day12.Part1(test_input);

        long test_answer = 4;
        Assert.Equal(test_answer, result);
    }


    [Fact]
    public void Day12Test1_11()
    {
        string test_input =
@".????#?.??? 1,3,3
?#??#?##??.??? 7,1,1
.???.??.?? 1,1,1
##?.#????.???? 2,2,1,1
?#?#.??.??. 4,2
.????#?.??? 1,3,3
?#??#?##??.??? 7,1,1
.???.??.?? 1,1,1
##?.#????.???? 2,2,1,1
?#?#.??.??. 4,2
.????#?.??? 1,3,3
?#??#?##??.??? 7,1,1
.???.??.?? 1,1,1
##?.#????.???? 2,2,1,1
?#?#.??.??. 4,2
.????#?.??? 1,3,3
?#??#?##??.??? 7,1,1
.???.??.?? 1,1,1
##?.#????.???? 2,2,1,1
?#?#.??.??. 4,2";

        long result = Day12.Part1(test_input);

        long test_answer = 36 * 4;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day12Test1_12()
    {
        string test_input =
@"??????????????????? 1,1,1,1,1,1,1,1,1,1";

        long result = Day12.Part1(test_input);

        long test_answer = 1;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day12Test1_13()
    {
        string test_input =
@"??????????????????# 1";

        long result = Day12.Part1(test_input);

        long test_answer = 1;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day12.txt");
        long result = Day12.Part1(test_input);

        Assert.Equal(7173, result);
    }

    [Fact]
    public void Day12Test2()
    {
        string test_input =
@"???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1";

        long result = Day12.Part2(test_input);

        long test_answer = 525152;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day12Test2_2()
    {
        string test_input =
@"?###???????? 3,2,1";

        long result = Day12.Part2(test_input);

        long test_answer = 506250;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day12.txt");
        long result = Day12.Part2(test_input);

        // 31453760541172 is not right
        Assert.Equal(29826669191291, result);
    }
}
