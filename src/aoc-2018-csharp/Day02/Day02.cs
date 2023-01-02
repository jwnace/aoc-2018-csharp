using System.Text;

namespace aoc_2018_csharp.Day02;

public static class Day02
{
    private static readonly string[] Input = File.ReadAllLines("Day02/day02.txt");

    public static int Part1()
    {
        var count1 = Input.Count(LineHasExactlyNOccurrencesOfAnyLetter(2));
        var count2 = Input.Count(LineHasExactlyNOccurrencesOfAnyLetter(3));

        return count1 * count2;
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

    private static Func<string, bool> LineHasExactlyNOccurrencesOfAnyLetter(int count) =>
        line => line.GroupBy(c => c).Select(g => new { Letter = g.Key, Count = g.Count() }).Any(g => g.Count == count);

    private static string GetCommonLetters(string a, string b)
    {
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
