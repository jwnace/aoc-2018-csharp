namespace aoc_2018_csharp.Day17;

public static class Day17
{
    private static readonly string[] Input = File.ReadAllLines("Day17/day17.txt");

    public static int Part1() => Solve1(Input);

    public static int Part2() => Solve2(Input);

    public static int Solve1(string[] input) => new Solver().Solve(input).Count(x => x.Value is '|' or '~');

    public static int Solve2(string[] input) => new Solver().Solve(input).Count(x => x.Value is '~');
}
