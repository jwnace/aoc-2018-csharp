using aoc_2018_csharp.Day03;

namespace aoc_2018_csharp_tests;

public class Day03Tests
{
    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        var expected = 107_663;
        var actual = Day03.Part1();
        actual.Should().Be(expected);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        var expected = 1_166;
        var actual = Day03.Part2();
        actual.Should().Be(expected);
    }
}
