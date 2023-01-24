using System.Text;

namespace aoc_2018_csharp.Day12;

public static class Day12
{
    private static readonly string[] Input = File.ReadAllLines("Day12/day12.txt");

    public static int Part1()
    {
        var initialState = Input.First().Split(": ")[1];
        var state = new Dictionary<int, char>();
        var rules = new Dictionary<string, char>();
        
        // TODO: try using ToDictionary() instead of looping
        for (var i = 0; i < initialState.Length; i++)
        {
            state[i] = initialState[i];
        }
        
        foreach (var line in Input.Skip(2))
        {
            var values = line.Split(" => ");
            var left = values[0];
            var right = values[1].Single();

            rules[left] = right;
        }

        for (var t = 1; t <= 20; t++)
        {
            var newState = new Dictionary<int, char>();

            var min = state.First().Key;
            var max = state.Last().Key;

            for (var i = min - 2; i <= max + 2; i++)
            {
                var builder = new StringBuilder();

                for (var j = -2; j <= 2; j++)
                {
                    var value = state.TryGetValue(i + j, out var v1) ? v1 : '.';
                    builder.Append(value);
                }

                var sample = builder.ToString();
                
                var newValue = rules.TryGetValue(sample, out var v2) ? v2 : '.';

                if (newValue == '#' || state.ContainsKey(i))
                {
                    newState[i] = newValue;
                }
            }
            
            state = newState;
        }
        
        return state.Where(x => x.Value == '#').Sum(x => x.Key);
    }

    public static int Part2() => 2;
}
