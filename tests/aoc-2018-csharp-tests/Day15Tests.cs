using aoc_2018_csharp.Day15;

namespace aoc_2018_csharp_tests;

public class Day15Tests
{
    [Test]
    public void Part1_Example()
    {
        var input = """
                    #######
                    #.G...#
                    #...EG#
                    #.#.#G#
                    #..G#E#
                    #.....#
                    #######
                    """.Split(Environment.NewLine);

        Day15.Solve(input, 1).Should().Be(27730);
    }

    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        Day15.Part1().Should().Be(190012);
    }

    [TestCase(new[]
    {
        "#######",
        "#.G...#",
        "#...EG#",
        "#.#.#G#",
        "#..G#E#",
        "#.....#",
        "#######",
    }, 4988)]
    [TestCase(new[]
    {
        "#######",
        "#E..EG#",
        "#.#G.E#",
        "#E.##E#",
        "#G..#.#",
        "#..E#.#",
        "#######",
    }, 31284)]
    [TestCase(new[]
    {
        "#######",
        "#E.G#.#",
        "#.#G..#",
        "#G.#.G#",
        "#G..#.#",
        "#...E.#",
        "#######",
    }, 3478)]
    [TestCase(new[]
    {
        "#######",
        "#.E...#",
        "#.#..G#",
        "#.###.#",
        "#E#G#G#",
        "#...#G#",
        "#######",
    }, 6474)]
    [TestCase(new[]
    {
        "#########",
        "#G......#",
        "#.E.#...#",
        "#..##..G#",
        "#...##..#",
        "#...#...#",
        "#.G...G.#",
        "#.....G.#",
        "#########",
    }, 1140)]
    public void Part2_Example(string[] input, int expected)
    {
        Day15.Solve(input, 2).Should().Be(expected);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        Day15.Part2().Should().Be(34364);
    }
}
