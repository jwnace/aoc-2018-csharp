using aoc_2018_csharp.Day20;

namespace aoc_2018_csharp_tests;

public class Day20Tests
{
    [TestCase("^WNE$", 3)]
    [TestCase("^ENWWW(NEEE|SSE(EE|N))$", 10)]
    [TestCase("^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$", 18)]
    [TestCase("^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$", 23)]
    [TestCase("^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$", 31)]
    public void Part1_Example_ReturnsCorrectAnswer(string input, int expected)
    {
        Day20.Solve1(input).Should().Be(expected);
    }

    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        Day20.Part1().Should().Be(3046);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        Day20.Part2().Should().Be(8545);
    }
}
