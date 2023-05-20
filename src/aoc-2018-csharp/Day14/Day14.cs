namespace aoc_2018_csharp.Day14;

public class Day14
{
    private static readonly string Input = File.ReadAllText("Day14/day14.txt").Trim();

    public static long Part1()
    {
        var iterations = int.Parse(Input);
        var recipes = new List<int> { 3, 7 };

        var elf1 = 0;
        var elf2 = 1;

        while (recipes.Count < iterations + 10)
        {
            var recipe1 = recipes[elf1];
            var recipe2 = recipes[elf2];

            var sum = recipe1 + recipe2;

            var newRecipes = sum.ToString()
                .Select(c => int.Parse(c.ToString()))
                .ToList();

            recipes.AddRange(newRecipes);

            elf1 += 1 + recipe1;
            elf2 += 1 + recipe2;

            elf1 %= recipes.Count;
            elf2 %= recipes.Count;
        }

        return long.Parse(string.Join("", recipes.TakeLast(10)));
    }

    public static int Part2()
    {
        var recipes = new List<int> { 3, 7 };

        var elf1 = 0;
        var elf2 = 1;

        while (true)
        {
            var s = new string(recipes.Skip(recipes.Count - Input.Length - 1).Select(x => x.ToString()[0]).ToArray());

            if (s.Contains(Input))
            {
                s = new string(recipes.Select(x => x.ToString()[0]).ToArray());
                return s.IndexOf(Input, StringComparison.Ordinal);
            }

            var sum = recipes[elf1] + recipes[elf2];

            if (sum >= 10)
            {
                recipes.Add(sum / 10);
            }

            recipes.Add(sum % 10);

            elf1 = (elf1 + recipes[elf1] + 1) % recipes.Count;
            elf2 = (elf2 + recipes[elf2] + 1) % recipes.Count;
        }
    }
}
