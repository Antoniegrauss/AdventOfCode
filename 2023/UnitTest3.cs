namespace AoC23.Day3;

using System.IO;

public class UnitTest3
{
    [Fact]
    public void Day3Test1()
    {
        string test_input =
@"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";

        int result = Day3.Part1(test_input);

        int test_answer = 4361;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day3Test1_2()
    {
        string test_input =
@"2.2
2*2
.2.";

        int result = Day3.Part1(test_input);

        int test_answer = 10;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day3.txt");
        int result = Day3.Part1(test_input);

        Assert.Equal(550064, result);
    }

    [Fact]
    public void Day3Test2()
    {
        string test_input =
@"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";

        long result = Day3.Part2(test_input);

        long test_answer = 467835;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day3Test2_2()
    {
        string test_input =
@"5*5
...
3*3";

        long result = Day3.Part2(test_input);

        long test_answer = 34;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day3Test2_3()
    {
        string test_input =
@"5*5
.1.
3*3";

        long result = Day3.Part2(test_input);

        long test_answer = 0;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day3Test2_4()
    {
        string test_input =
@"505
.*.
303";

        long result = Day3.Part2(test_input);

        long test_answer = 505 * 303;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day3Test2_5()
    {
        string test_input =
@".......................
.../..........*574.587.
..614..831..33.....*...";

        long result = Day3.Part2(test_input);

        long test_answer = 33 * 574;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day3Test2_6()
    {
        string test_input =
@".......................
.../..........*574*587.
..614..831..33.....*...";

        long result = Day3.Part2(test_input);

        long test_answer = 33 * 574 + 574 * 587;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day3Test2_7()
    {
        string test_input =
@"
.....603.......472................%...892..............=....314.684.......712............206......+.....657.%..........844.584.456...=......
......*..........*..#..........801...............738...524.................*........817.......+........*....57.............*..........749...
648..799........517.999...............#...........$..+.....................742..516*....939=..694...945..................863...480..........
..........700........................994....314......214....105.............................................#....137............*...522.....
...153.....*.........685..283................*...151........*....#....232......$.......99.92...863....*.....567.....*.285.....69.....*......
............205.........*..*..............275....*.........220...644...*....293.........$..%..*....337.91...............*.............963...
.......................844.32......449..........932....................869....................77......................288...................";

        long result = Day3.Part2(test_input);

        long test_answer =
            603 * 799 + 472 * 517 + 712 * 742 + 657 * 945 + 584 * 863 +
            516 * 817 +
            480 * 69 +
            700 * 205 + 314 * 275 + 105 * 220 + 337 * 91 + 522 * 963 +
            685 * 844 + 283 * 32 + 151 * 932 + 232 * 869 + 863 * 77 + 285 * 288;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day3.txt");
        long result = Day3.Part2(test_input);

        // Higher than 7758
        Assert.Equal(85010461, result);
    }
}
