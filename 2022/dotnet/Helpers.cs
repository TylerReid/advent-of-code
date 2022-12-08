namespace Advent;

public static class Helpers
{
    public static string[] SplitLines(this string s) => s.Split("\n");
    public static (string a, string b) SplitOnce(this string s, string sep)
    {
        var a = s.Split(sep);
        return (a[0], a[1]);
    }

    public static IEnumerable<(int index, T item)> Enumerate<T>(this IEnumerable<T> source) => source.Select((item, index) => (index, item));
}