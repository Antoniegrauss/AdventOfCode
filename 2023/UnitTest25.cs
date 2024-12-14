using System.IO;
using Xunit;
using Xunit.Abstractions;
using System.IO;

namespace AoC23.Day25;

public class UnitTest25
{
    private readonly ITestOutputHelper output;

    public UnitTest25(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Day25Test1_1()
    {
        string test_input =
@"jqt: rhn xhk nvd
rsh: frs pzl lsr
xhk: hfx
cmg: qnr nvd lhk bvb
rhn: xhk bvb hfx
bvb: xhk hfx
pzl: lsr hfx nvd
qnr: nvd
ntq: jqt hfx bvb xhk
nvd: lhk
lsr: lhk
rzs: qnr cmg lsr rsh
frs: qnr lhk lsr
";

        List<(string, string)> cutWires = [
            ("hfx", "pzl"),
            ("bvb", "cmg"),
            ("nvd", "jqt")
        ];

        long result = Day25.Part1(test_input, cutWires);
        Assert.Equal(54, result);
    }

    [Fact]
    public void FullTestPart1()
    {
        
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day25.txt");

        List<(string, string)> cutWires = [
            ("xbl", "qqh"),
            ("tbq", "qfj"),
            ("xzn", "dsr")];
        long result = Day25.Part1(test_input, cutWires);

        Assert.Equal(0, result);
    }

    [Fact]
    public void FullTestPart2()
    {
        string PATH_TO_INPUT = "C:\\Users\\AntonieGrauss\\CSharpProjects\\AoC25\\AoC25\\inputs\\";
        string test_input = File.ReadAllText(PATH_TO_INPUT + "Day25.txt");
        long result = Day25.Part2(test_input);

        Assert.Equal(0, result);
    }
}
