using aoc_2018_csharp.Day06;

namespace aoc_2018_csharp_tests;

public class Day06Tests
{
    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        var expected = 3_276;
        var actual = Day06.Part1();
        actual.Should().Be(expected);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        var expected = 38_380;
        var actual = Day06.Part2();
        actual.Should().Be(expected);
    }
}
