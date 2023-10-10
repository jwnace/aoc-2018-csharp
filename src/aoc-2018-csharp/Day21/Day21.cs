namespace aoc_2018_csharp.Day21;

public static class Day21
{
    public static int Part1() => Solve(1);

    public static int Part2() => Solve(int.MaxValue);

    private static int Solve(int maxIterations)
    {
        var seen = new HashSet<int>();
        var register5 = 0;

        for (var i = 0; i < maxIterations; i++)
        {
            var register1 = register5 | 65536;
            register5 = 4591209;

            while (true)
            {
                register5 += register1 & 255;
                register5 &= 16777215;
                register5 *= 65899;
                register5 &= 16777215;

                if (256 > register1)
                {
                    break;
                }

                var register3 = 0;

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

            if (seen.Contains(register5))
            {
                break;
            }

            seen.Add(register5);
        }

        return seen.Last();
    }
}
