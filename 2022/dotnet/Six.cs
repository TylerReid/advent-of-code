namespace Advent;

public class Six : AdventDay
{
    public int Day => 6;

    public void PartOne(string input) => Console.WriteLine(StartLength(input, 4));

    public void PartTwo(string input) => Console.WriteLine(StartLength(input, 14));

    int StartLength(string input, int len)
    {
        var chars = input.ToCharArray();
        for (int i = 0; i + len-1 < chars.Length; i++)
        {
            if (chars[i..(i+len)].Distinct().Count() == len)
            {
                return i+len;
            }
        }
        throw new Exception($"did not find packet for {input} with length {len}");
    }
}