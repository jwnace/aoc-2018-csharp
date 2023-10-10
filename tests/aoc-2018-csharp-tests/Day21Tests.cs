using aoc_2018_csharp.Day21;

namespace aoc_2018_csharp_tests;

public class Day21Tests
{
    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        Day21.Part1().Should().Be(12446070);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        Day21.Part2().Should().Be(0);
    }
}
