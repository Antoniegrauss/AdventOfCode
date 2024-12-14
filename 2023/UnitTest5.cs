namespace AoC23.Day5;

using System.IO;

public class UnitTest5
{
    [Fact]
    public void Day5Test1()
    {
        string test_input =
@"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4";

        Int64 result = Day5.Part1(test_input);

        Int64 test_answer = 35;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day5.txt");
        Int64 result = Day5.Part1(test_input);

        Assert.Equal(836040384, result);
    }

    [Fact]
    public void Day4Test2()
    {
        string test_input =
@"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4";

        Int64 result = Day5.Part2(test_input);

        Int64 test_answer = 46;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day4SimpleTest2()
    {
        string test_input =
@"seeds: 81 2

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4";

        Int64 result = Day5.Part2(test_input);

        Int64 test_answer = 46;
        Assert.Equal(test_answer, result);
    }


    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day5.txt");
        Int64 result = Day5.Part2(test_input);

        Assert.Equal(10834440, result);
    }
}
