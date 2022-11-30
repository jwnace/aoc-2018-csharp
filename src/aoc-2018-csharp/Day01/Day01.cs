namespace aoc_2018_csharp.Day01;

public static class Day01
{
    private static readonly int[] Input = File.ReadAllLines("Day01/day01.txt").Select(int.Parse).ToArray();

    public static int Part1() => Input.Sum(x => x);

    public static int Part2()
    {
        var knownValues = new List<int>();
        var value = 0;

        while (true)
        {
            foreach (var change in Input)
            {
                value += change;

                if (knownValues.Contains(value))
                {
                    return value;
                }

                knownValues.Add(value);
            }
        }
    }
}
