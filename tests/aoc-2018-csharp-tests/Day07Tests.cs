using aoc_2018_csharp.Day07;

namespace aoc_2018_csharp_tests;

public class Day07Tests
{
    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        var expected = "CHILFNMORYKGAQXUVBZPSJWDET";
        var actual = Day07.Part1();
        actual.Should().Be(expected);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        var expected = 891;
        var actual = Day07.Part2();
        actual.Should().Be(expected);
    }
}
