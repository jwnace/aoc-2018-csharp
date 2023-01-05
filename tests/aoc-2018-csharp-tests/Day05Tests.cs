using aoc_2018_csharp.Day05;

namespace aoc_2018_csharp_tests;

public class Day05Tests
{
    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        var expected = 9_562;
        var actual = Day05.Part1();
        actual.Should().Be(expected);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        var expected = 4_934;
        var actual = Day05.Part2();
        actual.Should().Be(expected);
    }
}
