namespace Advent;

public class Ten : AdventDay
{
    public int Day => 10;

    public void PartOne(string input)
    {
        var instructions = ParseInstructions(input);
        var cpu = new Cpu { Instructions = new Queue<Instruction>(instructions) };

        var sum = 0;
        cpu.PerCycleAction += (Cpu c) => 
        {
            if (cpu.Cycle == 20 || (cpu.Cycle - 20) % 40 == 0)
            {
                sum += cpu.X * cpu.Cycle;
            }
        };

        cpu.Run();

        Console.WriteLine(sum);
    }

    class Cpu
    {
        public int X { get; set; } = 1;
        public int Cycle { get; set; }
        public required Queue<Instruction> Instructions { get; set; }
        public Action<Cpu> PerCycleAction { get; set; } = (x) => {};

        public void Run()
        {
            while (Instructions.TryDequeue(out var ins))
            {
                if (ins is Noop)
                {
                    IncrCycle();
                    continue;
                }
                if (ins is AddX add)
                {
                    IncrCycle();
                    IncrCycle();
                    X += add.Value;
                    continue;
                }
            }
        }

        private void IncrCycle()
        {
            Cycle++;
            PerCycleAction(this);
        }

        public void StartInstruction(Instruction ins) => Cycle++;
        public void DoInstruction(Instruction ins)
        {
            if (ins is AddX add)
            {
                Cycle++;
            }
        }
        public void EndInstruction(Instruction ins)
        {
            if (ins is AddX add)
            {
                X += add.Value;
            }
        }
    }

    class Instruction {}
    class Noop : Instruction {}
    class AddX : Instruction 
    {
        public required int Value { get; set; }
    }

    List<Instruction> ParseInstructions(string input) => input.SplitLines()
        .Select<string, Instruction>(x => x == "noop"
            ? new Noop() 
            : new AddX { Value = int.Parse(x.Substring(5)) })
        .ToList();

    void Print(Instruction ins)
    {
        if (ins is Noop) Console.WriteLine("noop");
        if (ins is AddX add) Console.WriteLine($"addx {add.Value}");
    }
}