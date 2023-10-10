namespace aoc_2018_csharp.Day22;

public static class Day22
{
    private static readonly string[] Input = File.ReadAllLines("Day22/day22.txt");

    public static int Part1() => Solve1(Input);

    public static int Part2() => Solve2(Input);

    public static int Solve1(string[] input) => new Solver(input).Solve1();

    public static int Solve2(string[] input) => new Solver(input).Solve2();
}
