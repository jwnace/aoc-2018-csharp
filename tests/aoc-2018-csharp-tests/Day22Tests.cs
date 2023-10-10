using aoc_2018_csharp.Day22;

namespace aoc_2018_csharp_tests;

public class Day22Tests
{
    [Test]
    public void Part1_Example_ReturnsCorrectAnswer()
    {
        var input = new[]
        {
            "depth: 510",
            "target: 10,10"
        };

        Day22.Solve1(input).Should().Be(114);
    }

    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        Day22.Part1().Should().Be(5400);
    }

    [Test]
    public void Part2_Example_ReturnsCorrectAnswer()
    {
        var input = new[]
        {
            "depth: 510",
            "target: 10,10"
        };

        Day22.Solve2(input).Should().Be(45);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        Day22.Part2().Should().Be(1048);
    }
}
