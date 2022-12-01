namespace advent;

public class One
{
    public void DoIt()
    {
        var (sample, problem) = Input.GetInput("01");

        Console.WriteLine(FindMaxCalories(sample!));
        Console.WriteLine(FindMaxCalories(problem!));

        Console.WriteLine(TopN(sample!, 3));
        Console.WriteLine(TopN(problem!, 3));
    }

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