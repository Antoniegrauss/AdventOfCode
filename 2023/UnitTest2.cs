namespace AoC23.Day2;

using System.IO;

public class UnitTest2
{

    [Fact]
    public void D2P1Test()
    {
        string test_input =
@"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green";


        int result = Day2.Part1(test_input);

        int test_answer = 8;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void D2P1Full()
    {
        
        string full_input = File.ReadAllText(PATH_TO_INPUT + "Day2.txt");
        int result = Day2.Part1(full_input);

        Assert.Equal(2879, result);
    }

    [Fact]
    public void D2P2Test()
    {
        string test_input =
@"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green";


        int result = Day2.Part2(test_input);

        int test_answer = 2286;
        Assert.Equal(test_answer, result);
    }


    [Fact]
    public void D2P2Full()
    {
        
        string full_input = File.ReadAllText(PATH_TO_INPUT + "Day2.txt");
        int result = Day2.Part2(full_input);

        Assert.Equal(65122, result);
    }
}
