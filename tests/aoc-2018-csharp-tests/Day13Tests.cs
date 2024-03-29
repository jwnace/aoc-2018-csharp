﻿using aoc_2018_csharp.Day13;

namespace aoc_2018_csharp_tests;

public class Day13Tests
{
    [Test]
    public void Part1_ReturnsCorrectAnswer()
    {
        var expected = "118,112";
        var actual = Day13.Part1();
        actual.Should().Be(expected);
    }

    [Test]
    public void Part2_ReturnsCorrectAnswer()
    {
        var expected = "50,21";
        var actual = Day13.Part2();
        actual.Should().Be(expected);
    }
}
