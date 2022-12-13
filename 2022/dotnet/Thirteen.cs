using Newtonsoft.Json.Linq;

namespace Advent;

public class Thirteen : AdventDay
{
    public int Day => 13;

    public void PartOne(string input)
    {
        var data = Parse(input);

        var sum = 0;
        foreach (var (i, (left, right)) in data.Enumerate())
        {
           DebugWrite($"== Pair {i + 1} ==");
            if (IsInOrder(left, right) == true)
            {
                DebugWrite($"{i} + 1");
                sum += i + 1;
            }
            DebugWrite("");
        }

        Console.WriteLine(sum);
    }

    public void PartTwo(string input)
    {
        var data = ParsePartTwo(input);
        var dividerOne = new JArray(new JArray(new JValue(2L)));
        var dividerTwo = new JArray(new JArray(new JValue(6L)));
        data.Add(dividerOne);
        data.Add(dividerTwo);
        var sorted = data.OrderBy(x => x, new PacketComparer()).ToList();
        Console.WriteLine((sorted.IndexOf(dividerOne) + 1) * (sorted.IndexOf(dividerTwo) + 1));
    }

    class PacketComparer : Comparer<dynamic>
    {
        public override int Compare(dynamic? x, dynamic? y)
        {
            var inOrder = Thirteen.IsInOrder(x, y);
            return inOrder ? -1 : 1;
        }
    }

    static bool? IsInOrder(dynamic left, dynamic right)
    {
        DebugWrite($"Compare {JsonConvert.SerializeObject(left)} {JsonConvert.SerializeObject(right)}");
        if (left is JValue leftValue && leftValue.Value is long leftInt
            && right is JValue rightValue && rightValue.Value is long rightInt)
        {
            DebugWrite($"{(leftInt == rightInt ? null : leftInt < rightInt)}");
            return leftInt == rightInt ? null : leftInt < rightInt;
        }

        if (left is JArray leftArray && right is JArray rightArray)
        {
            foreach (var (i, lv) in leftArray.Enumerate())
            {
                if (i == rightArray.Count)
                {
                    DebugWrite("right ran out first");
                    return false;
                }
                var rv = rightArray[i];
                var inOrder = IsInOrder(lv, rv);
                if (inOrder.HasValue)
                {
                    DebugWrite($"{JsonConvert.SerializeObject(lv)} {JsonConvert.SerializeObject(rv)} is in order? {inOrder}");
                    return inOrder;
                }
            }
            return leftArray.Count == rightArray.Count ? null : true;
        }

        if (left is JValue)
        {
            DebugWrite($"convert left {left.Value} to array");
            return IsInOrder(new JArray(left), right);
        }

        if (right is JValue)
        {
            DebugWrite($"convert right {right.Value} to array");
            return IsInOrder(left, new JArray(right));
        }

        throw new Exception($"did not expect to get here left {left} right {right}");
    }

    List<(dynamic left, dynamic right)> Parse(string input)
    {
        var list = new List<(dynamic, dynamic)>();
        foreach (var pair in input.Split("\n\n"))
        {
            var (a, b) = pair.SplitOnce("\n");
            var first = JsonConvert.DeserializeObject(a)!;
            var second = JsonConvert.DeserializeObject(b)!;

            list.Add((first!, second!));
        }
        return list;
    }

    List<dynamic> ParsePartTwo(string input) => input.SplitLines()
        .Where(x => !string.IsNullOrEmpty(x))
        .Select(x => JsonConvert.DeserializeObject(x)!)
        .ToList();

    static bool Debug = false;
    static void DebugWrite(string s)
    {
        if (Debug)
        {
            Console.WriteLine(s);
        }
    }
}