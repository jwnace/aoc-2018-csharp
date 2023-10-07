using aoc_2018_csharp.Shared;

namespace aoc_2018_csharp.Day19;

public static class Day19
{
    private static readonly string[] Input = File.ReadAllLines("Day19/day19.txt");

    public static int Part1() => Solve1(Input);

    public static int Part2() => Solve2();

    public static int Solve1(string[] input)
    {
        var device = new Device(6);

        device.RunProgram(input);

        return device.Registers[0];
    }

    private static int Solve2()
    {
        const int bigNumber = 10551424;

        var result = 0;

        for (var i = 1; i <= bigNumber; i++)
        {
            if (bigNumber % i == 0)
            {
                result += i;
            }
        }

        return result;
    }
}
