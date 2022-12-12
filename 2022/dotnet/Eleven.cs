namespace Advent;

public class Eleven : AdventDay
{
    public int Day => 11;

    public void PartOne(string input)
    {
        var monkeys = Parse(input);

        var multiple = monkeys.Select(x => x.Divisor)
            .Aggregate((a, b) => a * b);

        foreach (var m in monkeys)
        {
            m.GivesAFuck = true;
            m.Multiple = multiple;
        }


        for (var i = 1; i <= 20; i++)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.TryInspect(out var result))
                {
                    monkeys.Single(x => x.Id == result.newMonkey)
                        .Inventory.Enqueue(result.item);
                }
            }
        }

        var monkeyBusiness = monkeys
            .Select(x => x.InspectionCount)
            .OrderByDescending(x => x)
            .Take(2)
            .Aggregate((a, b) => a * b);

        Console.WriteLine(monkeyBusiness);
    }

    public void PartTwo(string input)
    {
        var monkeys = Parse(input);
        var multiple = monkeys.Select(x => x.Divisor)
            .Aggregate((a, b) => a * b);

        foreach (var m in monkeys)
        {
            m.GivesAFuck = false;
            m.Multiple = multiple;
        }

        for (var i = 1; i <= 10_000; i++)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.TryInspect(out var result))
                {
                    monkeys.Single(x => x.Id == result.newMonkey)
                        .Inventory.Enqueue(result.item);
                }
            }

            if (i % 1000 == 0 | i == 1 | i == 20)
            {
                Console.WriteLine($"round {i}");
                foreach (var m in monkeys)
                {
                    Console.WriteLine($"monkey {m.Id} inspected items {m.InspectionCount}");
                }
            }
        }

        var monkeyBusiness = monkeys
            .Select(x => x.InspectionCount)
            .OrderByDescending(x => x)
            .Take(2)
            .Aggregate((a, b) => a * b);

        Console.WriteLine(monkeyBusiness);
    }

    List<Monkey> Parse(string input)
    {
        var monkeys = new List<Monkey>();

        var bigOleRegex = new Regex(
            @"Monkey (?<id>\d+):\n" + 
            @"  Starting items: (?<items>.+)\n" +
            @"  Operation: new = old (?<operator>\*|\+) (?<rightOperand>\d+|old)\n" +
            @"  Test: divisible by (?<divisibleBy>\d+)\n" +
            @"    If true: throw to monkey (?<ifTrue>\d+)\n" +
            @"    If false: throw to monkey (?<ifFalse>\d+)"
        );

        var matches = bigOleRegex.Matches(input);

        if (!matches.All(x => x.Success)) throw new Exception("did not find matches");

        foreach (Match match in matches)
        {
            var monkey = new Monkey();
            monkey.Id = long.Parse(match.Groups["id"].Value);
            foreach (var item in match.Groups["items"].Value.Split(", "))
            {
                monkey.Inventory.Enqueue(long.Parse(item));
            }
            var operatorz = match.Groups["operator"].Value;
            var operand = match.Groups["rightOperand"].Value;

            monkey.Operation = operatorz switch {
                "*" => x => x * (operand == "old" ? x : long.Parse(operand)),
                "+" => x => x + (operand == "old" ? x : long.Parse(operand)),
                _ => throw new Exception($"unexpected operator {operatorz}"),
            };

            var divisibleBy = long.Parse(match.Groups["divisibleBy"].Value);
            monkey.Divisor = divisibleBy;
            var ifTrue = long.Parse(match.Groups["ifTrue"].Value);
            var ifFalse = long.Parse(match.Groups["ifFalse"].Value);
            monkey.Test = x => x % divisibleBy == 0 ? ifTrue : ifFalse;
            monkeys.Add(monkey);
        }

        return monkeys;
    }

    class Monkey
    {
        public long Id { get; set; }
        public long InspectionCount { get; set; }
        public Queue<long> Inventory { get; set; } = new();
        public Func<long, long> Operation { get; set; } = x => x;
        public long Divisor { get; set; }
        public Func<long, long> Test { get; set; } = _ => throw new NotImplementedException();
        public bool GivesAFuck { get; set; } = true;
        public long Multiple { get; set; }

        public bool TryInspect(out (long newMonkey, long item) result)
        {
            if (Inventory.TryDequeue(out var item))
            {
                InspectionCount++;
                var newLevel = Operation(item);
                if (GivesAFuck)
                {
                    newLevel /= 3;
                }
                newLevel %= Multiple;
                result = (Test(newLevel), newLevel);
                return true;
            }
            result = (0,0);
            return false;
        }
    }
}