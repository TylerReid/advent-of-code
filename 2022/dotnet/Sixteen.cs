namespace Advent;

public class Sixteen : AdventDay
{
    public int Day => 16;

    public void PartOne(string input)
    {
        var valves = ParseValves(input);

        Console.WriteLine(JsonConvert.SerializeObject(valves, Formatting.Indented));
        throw new NotImplementedException();
    }

    record Valve
    {
        public required string Name { get; set; }
        public required int Rate { get; set; }
        public List<string> Tunnels { get; set; } = new();

        private static Regex _regex = new Regex(@"Valve (?<name>\w+) has flow rate=(?<rate>\d+); \w+ \w+ to \w+ (?<tunnels>.+)", 
            RegexOptions.Compiled);
        public static Valve Parse(string s)
        {
            var match = _regex.Match(s);
            return new Valve 
            {
                Name = match.Groups["name"].Value,
                Rate = int.Parse(match.Groups["rate"].Value),
                Tunnels = match.Groups["tunnels"].Value.Split(", ").ToList(),
            };
        }
    }

    List<Valve> ParseValves(string input) => input.SplitLines()
        .Select(x => Valve.Parse(x))
        .ToList();
}