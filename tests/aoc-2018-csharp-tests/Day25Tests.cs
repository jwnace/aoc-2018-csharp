using aoc_2018_csharp.Day25;

namespace aoc_2018_csharp_tests;

public class Day25Tests
{
    [Test]
    public void Part1_Example_ReturnsCorrectAnswer()
    {
        var input = "";

        Day25.Solve1(input).Should().Be(0);
    }

    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        Day25.Part1().Should().Be(0);
    }

    [Test]
    public void Part2_Example_ReturnsCorrectAnswer()
    {
        var input = "";

        Day25.Solve2(input).Should().Be(0);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        Day25.Part2().Should().Be(0);
    }
}
