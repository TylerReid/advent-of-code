namespace Advent;

public static class Helpers
{
    public static string[] SplitLines(this string s) => s.Split("\n");
    public static (string a, string b) SplitOnce(this string s, string sep)
    {
        var a = s.Split(sep);
        return (a[0], a[1]);
    }
}