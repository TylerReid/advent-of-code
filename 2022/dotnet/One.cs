namespace Advent;

public class One : AdventDay
{
    public override int Day => 1;

    protected override void PartOne(string input) 
        => Console.WriteLine(FindMaxCalories(input));

    protected override void PartTwo(string input) 
        => Console.WriteLine(TopN(input, 3));

    private long FindMaxCalories(string input) => 
        input.Split("\n\n")
            .Select(x => x.Split("\n").Select(y => long.Parse(y)).Sum())
            .Max(); 

    private long TopN(string input, int n) =>
        input.Split("\n\n")
            .Select(x => x.Split("\n").Select(y => long.Parse(y)).Sum())
            .OrderByDescending(x => x)
            .Take(n)
            .Sum();
}