namespace Advent;

public interface AdventDay
{
    int Day { get; }

    public void DoIt() 
    {
        var baseTextColor = Console.ForegroundColor;
        void Green() => Console.ForegroundColor = ConsoleColor.Green;
        void Yellow() => Console.ForegroundColor = ConsoleColor.Yellow;
        void Normal() => Console.ForegroundColor = baseTextColor;

        var example = GetExample();
        var problem = GetInput();
        
        Green();
        Console.WriteLine($"Day {Day} Part 1");
        Yellow();
        Console.WriteLine("Example:");
        Normal();
        PartOne(example);
        Yellow();
        Console.WriteLine("Problem:");
        Normal();
        PartOne(problem);

        Green();
        Console.WriteLine($"Day {Day} Part 2");
        Yellow();
        Console.WriteLine("Example:");
        Normal();
        PartTwo(example);
        Yellow();
        Console.WriteLine("Problem:");
        Normal();
        PartTwo(problem);
    }

    void PartOne(string input);
    void PartTwo(string input){}

    private string GetExample()
    {
        var path = Path.Combine("input", $"{Day:D2}.example");
        try
        {
            var example = File.ReadAllText(path);
            if (string.IsNullOrEmpty(example))
            {
                Console.WriteLine($"example data is empty at {path}");
                Environment.Exit(-1);
            }
            return example;
        }
        catch (FileNotFoundException) 
        {
            File.Create(Path.Combine(path));
            Console.WriteLine($"No input data found at {path}");
            Environment.Exit(-1);
            return "derp";
        }
    }

    private string GetInput()
    {
        try
        {
            return File.ReadAllText(Path.Combine("input", $"{Day:D2}.problem"));
        }
        catch (FileNotFoundException) 
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Cookie", $"session={File.ReadAllText(".aoctoken")}");
            var result = client.GetAsync($"https://adventofcode.com/2022/day/{Day}/input").Result;
            result.EnsureSuccessStatusCode();
            var input = result.Content.ReadAsStringAsync().Result.TrimEnd();
            File.WriteAllText(Path.Combine("input", $"{Day:D2}.problem"), input);
            return input;
        }
    }
}