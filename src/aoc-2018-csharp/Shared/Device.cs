using aoc_2018_csharp.Extensions;

namespace aoc_2018_csharp.Shared;

public class Device
{
    private static readonly Dictionary<int, int> _mappings = new()
    {
        { 0, 14 },
        { 1, 7 },
        { 2, 1 },
        { 3, 5 },
        { 4, 9 },
        { 5, 15 },
        { 6, 0 },
        { 7, 11 },
        { 8, 6 },
        { 9, 10 },
        { 10, 8 },
        { 11, 13 },
        { 12, 2 },
        { 13, 3 },
        { 14, 12 },
        { 15, 4 },
    };

    public int[] Registers { get; }

    public Device(int numRegisters = 4)
    {
        Registers = new int[numRegisters];
    }

    public Device(int[] registers)
    {
        Registers = new int[registers.Length];
        registers.CopyTo(Registers, 0);
    }

    public void Execute(Instruction instruction)
    {
        var (opcode, a, b, c) = instruction;

        Registers[c] = _mappings[opcode] switch
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

    public void RunProgram(IEnumerable<string> instructions)
    {
        foreach (var line in instructions)
        {
            var (opcode, a, b, c) = line.Split(" ").Select(int.Parse).ToArray();
            var instruction = new Instruction(opcode, a, b, c);

            Execute(instruction);
        }
    }
}
