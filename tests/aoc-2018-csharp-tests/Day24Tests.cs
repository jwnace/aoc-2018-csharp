using aoc_2018_csharp.Day24;

namespace aoc_2018_csharp_tests;

public class Day24Tests
{
    [Test]
    public void Part1_Example_ReturnsCorrectAnswer()
    {
        var input = "";

        Day24.Solve1(input).Should().Be(0);
    }

    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        Day24.Part1().Should().Be(0);
    }

    [Test]
    public void Part2_Example_ReturnsCorrectAnswer()
    {
        var input = "";

        Day24.Solve2(input).Should().Be(0);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        Day24.Part2().Should().Be(0);
    }
}