using aoc_2018_csharp.Day01;

namespace aoc_2018_csharp_tests;

public class Day01Tests
{
    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        var expected = 533;
        var actual = Day01.Part1();
        actual.Should().Be(expected);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        var expected = 73_272;
        var actual = Day01.Part2();
        actual.Should().Be(expected);
    }
}
