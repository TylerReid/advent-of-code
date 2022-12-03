namespace Advent;

public interface AdventDay
{
    int Day { get; }
    string Example { get; }

    public void DoIt() 
    {
        var baseTextColor = Console.ForegroundColor;
        void Green() => Console.ForegroundColor = ConsoleColor.Green;
        void Yellow() => Console.ForegroundColor = ConsoleColor.Yellow;
        void Normal() => Console.ForegroundColor = baseTextColor;

        var problem = GetInput($"{Day:D2}");
        
        Green();
        Console.WriteLine($"Day {Day} Part 1");
        Yellow();
        Console.WriteLine("Example:");
        Normal();
        PartOne(Example);
        Yellow();
        Console.WriteLine("Problem:");
        Normal();
        PartOne(problem);

        Green();
        Console.WriteLine($"Day {Day} Part 2");
        Yellow();
        Console.WriteLine("Example:");
        Normal();
        PartTwo(Example);
        Yellow();
        Console.WriteLine("Problem:");
        Normal();
        PartTwo(problem);
    }

    void PartOne(string input);
    void PartTwo(string input){}

    private string GetInput(string day)
    {
        try
        {
            return File.ReadAllText(Path.Combine("input", $"{day}.problem"));
        }
        catch (FileNotFoundException) 
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri($"https://adventofcode.com/2022/day/{Day}/input"),
            };
            client.DefaultRequestHeaders.Add("Cookie", $"session={File.ReadAllText(".aoctoken")}");
            var result = client.GetAsync("").Result;
            result.EnsureSuccessStatusCode();
            var input = result.Content.ReadAsStringAsync().Result;
            File.WriteAllText(Path.Combine("input", $"{day}.problem"), input);
            return input;
        }
    }
}