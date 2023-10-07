using aoc_2018_csharp.Extensions;
using aoc_2018_csharp.Shared;

namespace aoc_2018_csharp.Day16;

public static class Day16
{
    private static readonly string Input = File.ReadAllText("Day16/day16.txt").Trim();

    public static int Part1() => Solve1(Input);

    public static int Part2() => Solve2(Input);

    private static int Solve1(string input)
    {
        var result = new Dictionary<string, List<int>>();
        var sections = input.Split("\n\n\n\n");
        var samples = sections[0].Split("\n\n");

        foreach (var sample in samples)
        {
            var parts = sample.Split("\n");
            var (beforeText, instructionText, afterText) = parts;
            var beforeRegisters = beforeText[9..^1].Split(", ").Select(int.Parse).ToArray();
            var instructionParts = instructionText.Split(" ").Select(int.Parse).ToArray();
            var afterRegisters = afterText[9..^1].Split(", ").Select(int.Parse).ToArray();
            var (_, a, b, c) = instructionParts;
            var matches = new List<int>();

            for (var i = 0; i < 16; i++)
            {
                var device = new Device(beforeRegisters);
                var instruction = new Instruction(i, a, b, c);

                device.Execute(instruction);

                if (device.Registers.SequenceEqual(afterRegisters))
                {
                    matches.Add(i);
                }
            }

            result[sample] = matches;
        }

        return result.Count(x => x.Value.Count >= 3);
    }

    private static int Solve2(string input)
    {
        var sections = input.Split("\n\n\n\n");
        var program = sections[1].Split("\n");
        var device = new Device();

        device.RunProgram(program);

        return device.Registers[0];
    }
}
