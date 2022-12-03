namespace Advent;

public class One : AdventDay
{
    public int Day => 1;
    public string Example => """
    1000
    2000
    3000

    4000

    5000
    6000

    7000
    8000
    9000

    10000
    """;

    public void PartOne(string input) 
        => Console.WriteLine(FindMaxCalories(input));

    public void PartTwo(string input) 
        => Console.WriteLine(TopN(input, 3));

    private long FindMaxCalories(string input) => 
        input.Trim().Split("\n\n")
            .Select(x => x.Split("\n").Select(y => long.Parse(y)).Sum())
            .Max(); 

    private long TopN(string input, int n) =>
        input.Trim().Split("\n\n")
            .Select(x => x.Split("\n").Select(y => long.Parse(y)).Sum())
            .OrderByDescending(x => x)
            .Take(n)
            .Sum();
}