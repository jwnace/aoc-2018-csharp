using aoc_2018_csharp.Day04;

namespace aoc_2018_csharp_tests;

public class Day04Tests
{
    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        var expected = 85_296;
        var actual = Day04.Part1();
        actual.Should().Be(expected);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        var expected = 58_559;
        var actual = Day04.Part2();
        actual.Should().Be(expected);
    }
}
