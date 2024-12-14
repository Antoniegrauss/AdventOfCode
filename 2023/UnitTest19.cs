using Xunit;
using Xunit.Abstractions;
using System.IO;

namespace AoC23.Day19;

public class UnitTest19
{
    private readonly ITestOutputHelper output;

    public UnitTest19(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Day19Test1()
    {
        string test_input =
@"px{a<2006:qkq,m>2090:A,rfg}
pv{a>1716:R,A}
lnx{m>1548:A,A}
rfg{s<537:gd,x>2440:R,A}
qs{s>3448:A,lnx}
qkq{x<1416:A,crn}
crn{x>2662:A,R}
in{s<1351:px,qqz}
qqz{s>2770:qs,m<1801:hdj,R}
gd{a>3333:R,R}
hdj{m>838:A,pv}

{x=787,m=2655,a=1222,s=2876}
{x=1679,m=44,a=2067,s=496}
{x=2036,m=264,a=79,s=2244}
{x=2461,m=1339,a=466,s=291}
{x=2127,m=1623,a=2188,s=1013}
";

        long result = Day19.Part1(test_input);

        long test_answer = 19114;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day19.txt");
        long result = Day19.Part1(test_input);

        Assert.Equal(509597, result);
    }

    [Fact]
    public void Day19Test2_1()
    {
        string test_input =
@"px{a<2006:qkq,m>2090:A,rfg}
pv{a>1716:R,A}
lnx{m>1548:A,A}
rfg{s<537:gd,x>2440:R,A}
qs{s>3448:A,lnx}
qkq{x<1416:A,crn}
crn{x>2662:A,R}
in{s<1351:px,qqz}
qqz{s>2770:qs,m<1801:hdj,R}
gd{a>3333:R,R}
hdj{m>838:A,pv}

{x=787,m=2655,a=1222,s=2876}
{x=1679,m=44,a=2067,s=496}
{x=2036,m=264,a=79,s=2244}
{x=2461,m=1339,a=466,s=291}
{x=2127,m=1623,a=2188,s=1013}
";

        long result = Day19.Part2(test_input);

        long test_answer = 167_409_079_868_000;
        // 96725156237500
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day19Test2_2()
    {
        string test_input =
@"in{a<2:A,R}

{x=787,m=2655,a=1222,s=2876}
{x=1679,m=44,a=2067,s=496}
{x=2036,m=264,a=79,s=2244}
{x=2461,m=1339,a=466,s=291}
{x=2127,m=1623,a=2188,s=1013}
";

        long result = Day19.Part2(test_input);

        long test_answer = (long)4000 * (long)4000 * (long)4000;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day19Test2_3()
    {
        string test_input =
@"in{a<2:henk,R}
henk{x>3999:piet,R}
piet{m<3:A,R}

{x=787,m=2655,a=1222,s=2876}
{x=1679,m=44,a=2067,s=496}
{x=2036,m=264,a=79,s=2244}
{x=2461,m=1339,a=466,s=291}
{x=2127,m=1623,a=2188,s=1013}
";

        long result = Day19.Part2(test_input);

        long test_answer = (long)2 * (long)4000;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void Day19Test2_4()
    {
        string test_input =
@"in{a<2:henk,piet}
henk{x>3999:piet,R}
piet{m<3:A,R}

{x=787,m=2655,a=1222,s=2876}
{x=1679,m=44,a=2067,s=496}
{x=2036,m=264,a=79,s=2244}
{x=2461,m=1339,a=466,s=291}
{x=2127,m=1623,a=2188,s=1013}
";

        long result = Day19.Part2(test_input);

        long test_answer =
            (long)2 * (long)4000 +
            (long)2 * (long)3999 * (long)4000 * (long)4000;
        Assert.Equal(test_answer, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day19.txt");
        long result = Day19.Part2(test_input);

        Assert.Equal(143219569011526, result);
    }
}
