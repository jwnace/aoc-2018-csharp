using aoc_2018_csharp.Day08;

namespace aoc_2018_csharp_tests;

public class Day08Tests
{
    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        var expected = 49_602;
        var actual = Day08.Part1();
        actual.Should().Be(expected);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        var expected = 25_656;
        var actual = Day08.Part2();
        actual.Should().Be(expected);
    }
}
