using aoc_2018_csharp.Day14;

namespace aoc_2018_csharp_tests;

public class Day14Tests
{
    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        // arrange
        var expected = 9_315_164_154;

        // act
        var actual = Day14.Part1();

        // assert
        actual.Should().Be(expected);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        // arrange
        var expected = 20_231_866;

        // act
        var actual = Day14.Part2();

        // assert
        actual.Should().Be(expected);
    }
}
