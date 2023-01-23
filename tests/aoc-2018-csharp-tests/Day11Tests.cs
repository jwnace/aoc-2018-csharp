using aoc_2018_csharp.Day11;

namespace aoc_2018_csharp_tests;

public class Day11Tests
{
    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        var expected = "22,18";
        var actual = Day11.Part1();
        actual.Should().Be(expected);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        var expected = "234,197,14";
        var actual = Day11.Part2();
        actual.Should().Be(expected);
    }
}
