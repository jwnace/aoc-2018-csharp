using System.Text;

namespace aoc_2018_csharp.Day02;

public static class Day02
{
    private static readonly string[] Input = File.ReadAllLines("Day02/day02.txt");

    public static int Part1()
    {
        var query1 = Input.Select(line =>
                line.GroupBy(c => c)
                    .Select(g => new { Letter = g.Key, Count = g.Count() })
                    .Where(g => g.Count == 2)
                    .ToList())
            .Count(x => x.Count > 0);

        var query2 = Input.Select(line =>
                line.GroupBy(c => c)
                    .Select(g => new { Letter = g.Key, Count = g.Count() })
                    .Where(g => g.Count == 3)
                    .ToList())
            .Count(x => x.Count > 0);

        return query1 * query2;
    }

    public static string Part2()
    {
        for (var i = 0; i < Input.Length - 1; i++)
        {
            for (var j = i + 1; j < Input.Length; j++)
            {
                var commonLetters = GetCommonLetters(Input[i], Input[j]);

                if (commonLetters.Length == Input[i].Length - 1)
                {
                    return commonLetters;
                }
            }
        }

        return "couldn't find a solution";
    }

    private static string GetCommonLetters(string a, string b)
    {
        if (a.Length != b.Length)
        {
            throw new InvalidOperationException("the strings must be the same length");
        }

        var builder = new StringBuilder();

        for (var i = 0; i < a.Length; i++)
        {
            if (a[i] == b[i])
            {
                builder.Append(a[i]);
            }
        }

        return builder.ToString();
    }
}
