using aoc_2018_csharp.Day24;

namespace aoc_2018_csharp_tests;

public class Day24Tests
{
    [Test]
    public void Part1_Example_ReturnsCorrectAnswer()
    {
        var input =
            """
            Immune System:
            17 units each with 5390 hit points (weak to radiation, bludgeoning) with an attack that does 4507 fire damage at initiative 2
            989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3

            Infection:
            801 units each with 4706 hit points (weak to radiation) with an attack that does 116 bludgeoning damage at initiative 1
            4485 units each with 2961 hit points (immune to radiation; weak to fire, cold) with an attack that does 12 slashing damage at initiative 4
            """;

        Day24.Solve1(input).Should().Be(5216);
    }

    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        Day24.Part1().Should().Be(10723);
    }

    [Test]
    public void Part2_Example_ReturnsCorrectAnswer()
    {
        var input =
            """
            Immune System:
            17 units each with 5390 hit points (weak to radiation, bludgeoning) with an attack that does 4507 fire damage at initiative 2
            989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3

            Infection:
            801 units each with 4706 hit points (weak to radiation) with an attack that does 116 bludgeoning damage at initiative 1
            4485 units each with 2961 hit points (immune to radiation; weak to fire, cold) with an attack that does 12 slashing damage at initiative 4
            """;

        Day24.Solve2(input, 0, int.MaxValue).Should().Be(51);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        Day24.Part2().Should().Be(5120);
    }
}
