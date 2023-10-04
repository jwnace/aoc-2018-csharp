namespace aoc_2018_csharp.Extensions;

public static class EnumerableExtensions
{
    public static void Deconstruct<T>(this IEnumerable<T> enumerable, out T first)
    {
        first = enumerable.First();
    }

    public static void Deconstruct<T>(this IEnumerable<T> enumerable, out T first, out T second)
    {
        first = enumerable.First();
        second = enumerable.Skip(1).First();
    }

    public static void Deconstruct<T>(this IEnumerable<T> enumerable, out T first, out T second, out T third)
    {
        first = enumerable.First();
        second = enumerable.Skip(1).First();
        third = enumerable.Skip(2).First();
    }

    public static void Deconstruct<T>(this IEnumerable<T> enumerable, out T first, out T second, out T third, out T fourth)
    {
        first = enumerable.First();
        second = enumerable.Skip(1).First();
        third = enumerable.Skip(2).First();
        fourth = enumerable.Skip(3).First();
    }
}
