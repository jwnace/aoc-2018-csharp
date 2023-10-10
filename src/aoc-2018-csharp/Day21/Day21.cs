namespace aoc_2018_csharp.Day21;

public static class Day21
{
    private static readonly string[] Input = File.ReadAllLines("Day21/day21.txt");

    public static int Part1() => Solve1();

    public static int Part2() => Solve2(Input);

    private static int Solve1()
    {
        var register5 = 0;

        while (true)
        {
            var register1 = register5 | 65536;
            register5 = 4591209;

            while (true)
            {
                var register3 = register1 & 255;
                register5 += register3;
                register5 &= 16777215;
                register5 *= 65899;
                register5 &= 16777215;

                if (256 > register1)
                {
                    break;
                }

                register3 = 0;

                while (true)
                {
                    var register2 = register3 + 1;
                    register2 *= 256;

                    if (register2 > register1)
                    {
                        break;
                    }

                    register3++;
                }

                register1 = register3;
            }

            return register5;
        }
    }

    private static int Solve2(string[] input) => throw new NotImplementedException();
}
