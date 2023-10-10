using aoc_2018_csharp.Day23;

namespace aoc_2018_csharp_tests;

public class Day23Tests
{
    [Test]
    public void Part1_Example_ReturnsCorrectAnswer()
    {
        var input = new[]
        {
            "pos=<0,0,0>, r=4",
            "pos=<1,0,0>, r=1",
            "pos=<4,0,0>, r=3",
            "pos=<0,2,0>, r=1",
            "pos=<0,5,0>, r=3",
            "pos=<0,0,3>, r=1",
            "pos=<1,1,1>, r=1",
            "pos=<1,1,2>, r=1",
            "pos=<1,3,1>, r=1",
        };

        Day23.Solve1(input).Should().Be(7);
    }

    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        Day23.Part1().Should().Be(0);
    }

    [Test]
    public void Part2_Example_ReturnsCorrectAnswer()
    {
        var input = new[]
        {
            "pos=<0,0,0>, r=4",
            "pos=<1,0,0>, r=1",
            "pos=<4,0,0>, r=3",
            "pos=<0,2,0>, r=1",
            "pos=<0,5,0>, r=3",
            "pos=<0,0,3>, r=1",
            "pos=<1,1,1>, r=1",
            "pos=<1,1,2>, r=1",
            "pos=<1,3,1>, r=1",
        };

        Day23.Solve2(input).Should().Be(0);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        Day23.Part2().Should().Be(0);
    }
}
