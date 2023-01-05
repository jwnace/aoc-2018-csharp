namespace aoc_2018_csharp.Day05;

public static class Day05
{
    private static readonly string Input = File.ReadAllText("Day05/day05.txt");

    public static int Part1()
    {
        var input = Input.ToList();
        return GetShortenedLength(input);
    }

    public static int Part2()
    {
        var best = int.MaxValue;
        
        for (var c = 'a'; c <= 'z'; c++)
        {
            var input = Input.ToList();
            input.RemoveAll(x => x == c || x == c - 32);
            best = Math.Min(best, GetShortenedLength(input));
        }
        
        return best;
    }

    private static int GetShortenedLength(List<char> input)
    {
        while (true)
        {
            var reactionHappened = false;

            for (var i = 0; i < input.Count - 1; i++)
            {
                var (left, right) = (input[i], input[i + 1]);
                var difference = Math.Abs(left - right);

                if (difference == 32)
                {
                    input.RemoveAt(i + 1);
                    input.RemoveAt(i);
                    reactionHappened = true;
                    break;
                }
            }

            if (reactionHappened == false)
            {
                break;
            }
        }

        return input.Count;
    }
}
