using aoc_2018_csharp.Day18;

namespace aoc_2018_csharp_tests;

public class Day18Tests
{
    [Test]
    public void Part1_Example_ReturnsCorrectAnswer()
    {
        var input = new[]
        {
            ".#.#...|#.",
            ".....#|##|",
            ".|..|...#.",
            "..|#.....#",
            "#.#|||#|#|",
            "...#.||...",
            ".|....|...",
            "||...#|.#|",
            "|.||||..|.",
            "...#.|..|.",
        };

        Day18.Solve(input).Should().Be(1147);
    }

    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        Day18.Part1().Should().Be(621205);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        Day18.Part2().Should().Be(228490);
    }
}
