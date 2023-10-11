using aoc_2018_csharp.Day25;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace aoc_2018_csharp_tests;

public class Day25Tests
{
    [TestCase(new[]
    {
        "0,0,0,0",
        "3,0,0,0",
        "0,3,0,0",
        "0,0,3,0",
        "0,0,0,3",
        "0,0,0,6",
        "9,0,0,0",
        "12,0,0,0",
    }, 2)]
    [TestCase(new[]
    {
        "0,0,0,0",
        "3,0,0,0",
        "0,3,0,0",
        "0,0,3,0",
        "0,0,0,3",
        "0,0,0,6",
        "9,0,0,0",
        "12,0,0,0",
        "6,0,0,0",
    }, 1)]
    [TestCase(new[]
    {
        "-1,2,2,0",
        "0,0,2,-2",
        "0,0,0,-2",
        "-1,2,0,0",
        "-2,-2,-2,2",
        "3,0,2,-1",
        "-1,3,2,2",
        "-1,0,-1,0",
        "0,2,1,-2",
        "3,0,0,0",
    }, 4)]
    [TestCase(new[]
    {
        "1,-1,0,1",
        "2,0,-1,0",
        "3,2,-1,0",
        "0,0,3,1",
        "0,0,-1,-1",
        "2,3,-2,0",
        "-2,2,0,0",
        "2,-2,0,-1",
        "1,-1,0,-1",
        "3,2,0,2",
    }, 3)]
    [TestCase(new[]
    {
        "1,-1,-1,-2",
        "-2,-2,0,1",
        "0,2,1,3",
        "-2,3,-2,1",
        "0,2,3,-2",
        "-1,-1,1,-2",
        "0,-2,-1,0",
        "-2,2,3,-1",
        "1,2,2,0",
        "-1,-2,0,-2",
    }, 8)]
    public void Part1_Example_ReturnsCorrectAnswer(string[] input, int expected)
    {
        Day25.Solve1(input).Should().Be(expected);
    }

    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        Day25.Part1().Should().Be(370);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        Day25.Part2().Should().Be("Merry Christmas!");
    }
}
