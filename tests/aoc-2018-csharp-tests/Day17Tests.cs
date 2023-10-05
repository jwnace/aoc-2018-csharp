using aoc_2018_csharp.Day17;

namespace aoc_2018_csharp_tests;

public class Day17Tests
{
    [Test]
    public void Part1_Example_ReturnsCorrectAnswer()
    {
        var input = new[]
        {
            "x=495, y=2..7",
            "y=7, x=495..501",
            "x=501, y=3..7",
            "x=498, y=2..4",
            "x=506, y=1..2",
            "x=498, y=10..13",
            "x=504, y=10..13",
            "y=13, x=498..504",
        };

        Day17.Solve1(input).Should().Be(57);
    }

    [Test]
    public void Part1_CustomExample_ReturnsCorrectAnswer()
    {
        var input = new[]
        {
            "x=507, y=1..2",
            "x=498, y=5..7",
            "x=501, y=5..7",
            "y=7, x=498..501",
            "x=494, y=9..13",
            "x=506, y=9..13",
            "y=13, x=494..506",
        };

        Day17.Solve1(input).Should().Be(88);
    }

    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        Day17.Part1().Should().Be(27331);
    }

    [Test]
    public void Part2_Example_ReturnsCorrectAnswer()
    {
        var input = new[]
        {
            "x=495, y=2..7",
            "y=7, x=495..501",
            "x=501, y=3..7",
            "x=498, y=2..4",
            "x=506, y=1..2",
            "x=498, y=10..13",
            "x=504, y=10..13",
            "y=13, x=498..504",
        };

        Day17.Solve2(input).Should().Be(29);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        Day17.Part2().Should().Be(22245);
    }
}
