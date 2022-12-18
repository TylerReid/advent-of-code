namespace Advent;

public static class Helpers
{
    public static string[] SplitLines(this string s) => s.Split("\n");
    public static (string a, string b) SplitOnce(this string s, string sep)
    {
        var a = s.Split(sep);
        return (a[0], a[1]);
    }

    public static (T2 a, T2 b) Convert<T, T2>(this (T a, T b) p, Func<T, T2> convert) => (convert(p.a), convert(p.b));

    public static IEnumerable<(int index, T item)> Enumerate<T>(this IEnumerable<T> source) => source.Select((item, index) => (index, item));

    public static IEnumerable<T> RepeatForever<T>(this IEnumerable<T> source)
    {
        while (true)
        {
            foreach (var i in source)
            {
                yield return i;
            }
        }
    }
}