using aoc_2018_csharp.Day09;

namespace aoc_2018_csharp_tests;

public class Day09Tests
{
    [TestCase(9, 25, 32)]
    [TestCase(10, 1_618, 8_317)]
    [TestCase(13, 7_999, 146_373)]
    [TestCase(17, 1_104, 2_764)]
    [TestCase(21, 6_111, 54_718)]
    [TestCase(30, 5_807, 37_305)]
    public void GetMaxScore_ReturnsCorrectAnswer(int playerCount, int lastMarbleValue, int expected)
    {
        var actual = Day09.GetMaxScore(playerCount, lastMarbleValue);
        actual.Should().Be(expected);
    }
    
    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        var expected = 439_089;
        var actual = Day09.Part1();
        actual.Should().Be(expected);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        var expected = 3_668_541_094;
        var actual = Day09.Part2();
        actual.Should().Be(expected);
    }
}
