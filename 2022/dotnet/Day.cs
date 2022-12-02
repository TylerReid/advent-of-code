namespace Advent;

public interface AdventDay
{
    int Day { get; }

    public void DoIt() 
    {
        var (sample, problem) = GetInput($"{Day:D2}");
        PartOne(sample);
        if (problem != "") PartOne(problem);
        PartTwo(sample);
        if (problem != "") PartTwo(problem);
    }

    void PartOne(string input);
    void PartTwo(string input){}

    private (string sample, string problem) GetInput(string day)
    {
        string TryRead(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (FileNotFoundException) 
            {
                return "";
            }
        }

        return (TryRead(Path.Combine("input", $"{day}.sample")), 
            TryRead(Path.Combine("input", $"{day}.problem")));
    }
}