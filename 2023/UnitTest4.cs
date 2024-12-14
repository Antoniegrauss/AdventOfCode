namespace AoC23.Day4;

using System.IO;

public class UnitTest4
{
    [Fact]
    public void Day4Test1()
    {
        string test_input =
@"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11";

        int result = Day4.Part1(test_input);

        int test_answer = 13;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day4.txt");
        int result = Day4.Part1(test_input);

        Assert.Equal(23673, result);
    }

    [Fact]
    public void Day4Test2()
    {
        string test_input =
@"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11";

        int result = Day4.Part2(test_input);

        int test_answer = 30;
        Assert.Equal(test_answer, result);
    }


    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day4.txt");
        int result = Day4.Part2(test_input);

        Assert.Equal(12263631, result);
    }
}
