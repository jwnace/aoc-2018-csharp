namespace aoc_2018_csharp.Extensions;

public static class EnumerableExtensions
{
    public static void Deconstruct<T>(this T[] array, out T first, out T second)
    {
        first = array[0];
        second = array[1];
    }

    public static void Deconstruct<T>(this T[] array, out T first, out T second, out T third)
    {
        first = array[0];
        second = array[1];
        third = array[2];
    }

    public static void Deconstruct<T>(this T[] array, out T first, out T second, out T third, out T fourth)
    {
        first = array[0];
        second = array[1];
        third = array[2];
        fourth = array[3];
    }

    public static void Deconstruct<T>(this T[] array, out T first, out T second, out T third, out T fourth, out T fifth)
    {
        first = array[0];
        second = array[1];
        third = array[2];
        fourth = array[3];
        fifth = array[4];
    }
}
