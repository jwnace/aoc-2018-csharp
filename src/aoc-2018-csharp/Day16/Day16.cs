using aoc_2018_csharp.Extensions;

namespace aoc_2018_csharp.Day16;

public static class Day16
{
    public static readonly string Input = File.ReadAllText("Day16/day16.txt").Trim();

    public static int Part1() => Solve1(Input);

    public static int Part2() => Solve2(Input);

    public static int Solve1(string input)
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

    public static int Solve2(string input)
    {
        var sections = input.Split("\n\n\n\n");
        var program = sections[1].Split("\n");

        var mappings = MapOpcodes(input);

        var device = new Device();

        foreach (var line in program)
        {
            var (opcode, a, b, c) = line.Split(" ").Select(int.Parse).ToArray();
            var instruction = new Instruction(mappings[opcode], a, b, c);

            device.Execute(instruction);
        }

        return device.Registers[0];
    }

    public static Dictionary<int, int> MapOpcodes(string input)
    {
        return new Dictionary<int, int>
        {
            { 0, 14},
            { 1,  7},
            { 2,  1},
            { 3,  5},
            { 4,  9},
            { 5, 15},
            { 6,  0},
            { 7, 11},
            { 8,  6},
            { 9, 10},
            {10,  8},
            {11, 13},
            {12,  2},
            {13,  3},
            {14, 12},
            {15,  4},
        };

        // var result = new Dictionary<int, List<int>>();
        //
        // var sections = input.Split("\n\n\n\n");
        // var (samples, _) = (sections[0].Split("\n\n"), sections[1]);
        //
        // foreach (var sample in samples)
        // {
        //     var parts = sample.Split("\n");
        //     var (beforeText, instructionText, afterText) = parts;
        //     var beforeRegisters = beforeText[9..^1].Split(", ").Select(int.Parse).ToArray();
        //     var instructionParts = instructionText.Split(" ").Select(int.Parse).ToArray();
        //     var afterRegisters = afterText[9..^1].Split(", ").Select(int.Parse).ToArray();
        //     var (opcode, a, b, c) = instructionParts;
        //     var matches = new List<int>();
        //
        //     for (var i = 0; i < 16; i++)
        //     {
        //         var device = new Device(beforeRegisters);
        //         var instruction = new Instruction(i, a, b, c);
        //         device.Execute(instruction);
        //
        //         if (device.Registers.SequenceEqual(afterRegisters))
        //         {
        //             matches.Add(i);
        //         }
        //     }
        //
        //     result[opcode] = matches;
        // }

        // Console.WriteLine(string.Join("\n", result.Select(x => $"{x.Key,2}: {string.Join(", ", x.Value.Select(y => y.ToString().PadLeft(2)))}")));

        // return FilterMappings(result);
    }

    private static Dictionary<int, int> FilterMappings(Dictionary<int, List<int>> result)
    {
        var mappings = new Dictionary<int, int>();

        while (result.Any())
        {
            var (opcode, matches) = result.First(x => x.Value.Count == 1);
            var match = matches.First();

            mappings[opcode] = match;
            result.Remove(opcode);

            var foo =
                result.Select(x => (x.Key, Value: x.Value.Where(y => y != match).ToList()))
                    .ToDictionary(x => x.Key, x => x.Value);
        }

        return mappings;
    }

    private class Device
    {
        public int[] Registers { get; set; } = new int[4];

        public Device()
        {

        }

        public Device(int[] registers)
        {
            registers.CopyTo(Registers, 0);
        }

        public void Execute(Instruction instruction)
        {
            var (opcode, a, b, c) = instruction;

            Registers[c] = opcode switch
            {
                0 => Registers[a] + Registers[b],
                1 => Registers[a] + b,
                2 => Registers[a] * Registers[b],
                3 => Registers[a] * b,
                4 => Registers[a] & Registers[b],
                5 => Registers[a] & b,
                6 => Registers[a] | Registers[b],
                7 => Registers[a] | b,
                8 => Registers[a],
                9 => a,
                10 => a > Registers[b] ? 1 : 0,
                11 => Registers[a] > b ? 1 : 0,
                12 => Registers[a] > Registers[b] ? 1 : 0,
                13 => a == Registers[b] ? 1 : 0,
                14 => Registers[a] == b ? 1 : 0,
                15 => Registers[a] == Registers[b] ? 1 : 0,
                _ => throw new Exception($"Unknown opcode: {opcode}")
            };
        }
    }

    private record Instruction(int Opcode, int A, int B, int C);
}
