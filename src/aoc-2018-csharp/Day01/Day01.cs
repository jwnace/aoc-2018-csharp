namespace aoc_2018_csharp.Day01;

public static class Day01
{
    private static readonly string[] Input = File.ReadAllLines("Day01/day01.txt");

    public static int Part1()
    {
        var temp = Input.Select(int.Parse);

        return temp.Sum(x => x);
        
    }

    public static int Part2()
    {
        var temp = Input.Select(int.Parse);

        var knownValues = new List<int>();
        var frequency = 0;
        
        while(true)
        foreach(var t in temp)
        {
            frequency = frequency + t;

            if (knownValues.Contains(frequency))
            {
                return frequency;
            }

            knownValues.Add(frequency);
        }

        return 0;
    }
}
