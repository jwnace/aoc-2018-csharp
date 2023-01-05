namespace aoc_2018_csharp.Day04;

public static class Day04
{
    private static readonly string[] Input = File.ReadAllLines("Day04/day04.txt");

    public static int Part1()
    {
        var guards = GetGuards();

        var guard = guards.MaxBy(g => g.SleepMinutes.Sum(m => m.Value));
        var minute = guard!.SleepMinutes.MaxBy(m => m.Value).Key;

        return guard.Id * minute;
    }
    
    public static int Part2()
    {
        var guards = GetGuards();

        var guard = guards.MaxBy(g => g.SleepMinutes.Max(m => m.Value));
        var minute = guard!.SleepMinutes.MaxBy(m => m.Value).Key;

        return guard.Id * minute;
    }

    private static List<Guard> GetGuards()
    {
        var guards = new List<Guard>();
        var guardId = 0;
        var sleepTime = 0;

        foreach (var line in Input)
        {
            if (line.Contains("begins shift"))
            {
                guardId = int.Parse(line.Split(' ')[3][1..]);
            }
            else if (line.Contains("falls asleep"))
            {
                sleepTime = int.Parse(line.Split(':')[1][..2]);
            }
            else if (line.Contains("wakes up"))
            {
                var wakeTime = int.Parse(line.Split(':')[1][..2]);

                var guard = guards.FirstOrDefault(g => g.Id == guardId);

                if (guard is null)
                {
                    guard = new Guard(guardId);
                    guards.Add(guard);
                }

                for (var i = sleepTime; i < wakeTime; i++)
                {
                    var value = guard.SleepMinutes.TryGetValue(i, out var v) ? v : 0;
                    guard.SleepMinutes[i] = value + 1;
                }
            }
        }

        return guards;
    }

    private class Guard
    {
        public Guard(int id)
        {
            Id = id;
        }

        public int Id { get; }
        public Dictionary<int, int> SleepMinutes { get; } = new();
    }
}
