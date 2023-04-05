using aoc_2018_csharp.Day12;

namespace aoc_2018_csharp_tests;

public class Day12Tests
{
    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        var expected = 1_987;
        var actual = Day12.Part1();
        actual.Should().Be(expected);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        var expected = 1_150_000_000_358;
        var actual = Day12.Part2();
        actual.Should().Be(expected);
    }
}
