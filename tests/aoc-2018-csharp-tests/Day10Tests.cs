using aoc_2018_csharp.Day10;

namespace aoc_2018_csharp_tests;

public class Day10Tests
{
    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        var expected = Environment.NewLine + "######  #####    ####   #    #  #         ##       ###  #     " +
                       Environment.NewLine + "#       #    #  #    #  #    #  #        #  #       #   #     " +
                       Environment.NewLine + "#       #    #  #        #  #   #       #    #      #   #     " +
                       Environment.NewLine + "#       #    #  #        #  #   #       #    #      #   #     " +
                       Environment.NewLine + "#####   #####   #         ##    #       #    #      #   #     " +
                       Environment.NewLine + "#       #  #    #         ##    #       ######      #   #     " +
                       Environment.NewLine + "#       #   #   #        #  #   #       #    #      #   #     " +
                       Environment.NewLine + "#       #   #   #        #  #   #       #    #  #   #   #     " +
                       Environment.NewLine + "#       #    #  #    #  #    #  #       #    #  #   #   #     " +
                       Environment.NewLine + "######  #    #   ####   #    #  ######  #    #   ###    ######";

        var actual = Day10.Part1();
        actual.Should().Be(expected);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        var expected = 10_813;
        var actual = Day10.Part2();
        actual.Should().Be(expected);
    }
}
