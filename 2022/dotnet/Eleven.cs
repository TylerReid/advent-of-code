using System.Text.RegularExpressions;

namespace Advent;

public class Eleven : AdventDay
{
    public int Day => 11;

    public void PartOne(string input)
    {
        var monkeys = Parse(input);

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
            monkey.Id = int.Parse(match.Groups["id"].Value);
            foreach (var item in match.Groups["items"].Value.Split(", "))
            {
                monkey.Inventory.Enqueue(int.Parse(item));
            }
            var operatorz = match.Groups["operator"].Value;
            var operand = match.Groups["rightOperand"].Value;

            monkey.Operation = operatorz switch {
                "*" => x => x * (operand == "old" ? x : int.Parse(operand)),
                "+" => x => x + (operand == "old" ? x : int.Parse(operand)),
                _ => throw new Exception($"unexpected operator {operatorz}"),
            };

            var divisibleBy = int.Parse(match.Groups["divisibleBy"].Value);
            var ifTrue = int.Parse(match.Groups["ifTrue"].Value);
            var ifFalse = int.Parse(match.Groups["ifFalse"].Value);
            monkey.Test = x => x % divisibleBy == 0 ? ifTrue : ifFalse;
            monkeys.Add(monkey);
        }

        return monkeys;
    }

    class Monkey
    {
        public int Id { get; set; }
        public int InspectionCount { get; set; }
        public Queue<int> Inventory { get; set; } = new();
        public Func<int, int> Operation { get; set; } = x => x;
        public Func<int, int> Test { get; set; } = _ => throw new NotImplementedException();

        public bool TryInspect(out (int newMonkey, int item) result)
        {
            if (Inventory.TryDequeue(out var item))
            {
                InspectionCount++;
                var newLevel = Operation(item) / 3;
                result = (Test(newLevel), newLevel);
                return true;
            }
            result = (0,0);
            return false;
        }
    }
}