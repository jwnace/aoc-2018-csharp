using aoc_2018_csharp.Day19;

namespace aoc_2018_csharp_tests;

public class Day19Tests
{
    [Test]
    public void Part1_Example_ReturnsCorrectAnswer()
    {
        var input = new[]
        {
            "#ip 0",
            "seti 5 0 1",
            "seti 6 0 2",
            "addi 0 1 0",
            "addr 1 2 3",
            "setr 1 0 0",
            "seti 8 0 4",
            "seti 9 0 5",
        };

        Day19.Solve1(input).Should().Be(6);
    }

    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        Day19.Part1().Should().Be(2047);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        Day19.Part2().Should().Be(24033240);
    }
}
