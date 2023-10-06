using aoc_2018_csharp.Day19;

namespace aoc_2018_csharp_tests;

public class Day19Tests
{
    [Test]
    public void Part1_Example_ReturnsCorrectAnswer()
    {
        var input = new[]
        {
            "",
        };

        Day19.Solve1(input).Should().Be(0);
    }

    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        Day19.Part1().Should().Be(0);
    }

    [Test]
    public void Part2_Example_ReturnsCorrectAnswer()
    {
        var input = new[]
        {
            "",
        };

        Day19.Solve1(input).Should().Be(0);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        Day19.Part2().Should().Be(0);
    }
}
