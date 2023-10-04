using aoc_2018_csharp.Day16;

namespace aoc_2018_csharp_tests;

public class Day16Tests
{
    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        Day16.Part1().Should().Be(590);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        Day16.Part2().Should().Be(475);
    }
}
