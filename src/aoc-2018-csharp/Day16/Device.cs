namespace aoc_2018_csharp.Day16;

public class Device
{
    public int[] Registers { get; } = new int[4];

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
