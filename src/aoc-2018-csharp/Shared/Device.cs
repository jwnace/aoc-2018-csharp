using aoc_2018_csharp.Extensions;

namespace aoc_2018_csharp.Shared;

public class Device
{
    private static readonly Dictionary<int, int> _intMappings = new()
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

    private static readonly Dictionary<string, int> _stringMappings = new()
    {
        { "addr", 0 },
        { "addi", 1 },
        { "mulr", 2 },
        { "muli", 3 },
        { "banr", 4 },
        { "bani", 5 },
        { "borr", 6 },
        { "bori", 7 },
        { "setr", 8 },
        { "seti", 9 },
        { "gtir", 10 },
        { "gtri", 11 },
        { "gtrr", 12 },
        { "eqir", 13 },
        { "eqri", 14 },
        { "eqrr", 15 },
    };

    private int? _instructionPointer;

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

    public void RunProgram(string[] instructions)
    {
        if (instructions[0].StartsWith("#ip"))
        {
            _instructionPointer = int.Parse(instructions[0].Split(" ")[1]);
        }

        instructions = instructions[1..];

        for (var i = 0; i < instructions.Length; i++)
        {
            if (_instructionPointer.HasValue)
            {
                Registers[_instructionPointer.Value] = i;
            }

            var line = instructions[i];
            var parts = line.Split(" ").ToArray();
            var opcode = parts[0];
            var (a, b, c) = parts[1..].Select(int.Parse).ToArray();

            if (int.TryParse(opcode, out var o))
            {
                ExecuteInstruction(o, a, b, c);
            }
            else
            {
                ExecuteInstruction(opcode, a, b, c);
            }

            if (_instructionPointer.HasValue)
            {
                i = Registers[_instructionPointer.Value];
            }
        }
    }

    public void ExecuteInstruction(int opcode, int a, int b, int c) => Execute(_intMappings[opcode], a, b, c);

    private void ExecuteInstruction(string opcode, int a, int b, int c) => Execute(_stringMappings[opcode], a, b, c);

    private void Execute(int opcode, int a, int b, int c)
    {
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
            _ => throw new ArgumentOutOfRangeException(nameof(opcode), opcode, $"Unknown opcode: {opcode}")
        };
    }
}
