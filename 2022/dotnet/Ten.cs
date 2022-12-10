namespace Advent;

public class Ten : AdventDay
{
    public int Day => 10;

    public void PartOne(string input)
    {
        var instructions = ParseInstructions(input);
        var cpu = new Cpu();

        var sum = 0;
        foreach (var (i, ins) in instructions.Enumerate())
        {
            cpu.StartInstruction(ins);
            if (cpu.Cycle == 20 || (cpu.Cycle - 20) % 40 == 0)
            {
                sum += cpu.X * cpu.Cycle;
            }
            if (ins is Noop) continue; //eww
            cpu.DoInstruction(ins);
            if (cpu.Cycle == 20 || (cpu.Cycle - 20) % 40 == 0)
            {
                sum += cpu.X * cpu.Cycle;
            }

            cpu.EndInstruction(ins);
        }

        Console.WriteLine(sum);
    }

    class Cpu
    {
        public int X { get; set; } = 1;
        public int Cycle { get; set; }

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