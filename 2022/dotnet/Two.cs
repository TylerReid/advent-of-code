namespace Advent;

public class Two : AdventDay
{
    public int Day => 2;
    public string Example => """
    A Y
    B X
    C Z
    """;

    public void PartOne(string input)
    {
        var score = 0;
        foreach (var round in input.Split("\n").Where(x => x != ""))
        {
            var (theirs, mine) = Parse(round);
            score += Score(Play(mine, theirs));
            score += Score(mine);
        }
        Console.WriteLine(score);
    }

    public void PartTwo(string input)
    {
        var score = 0;
        foreach (var round in input.Split("\n").Where(x => x != ""))
        {
            var (theirs, mine) = ParsePartTwo(round);
            score += Score(Play(mine, theirs));
            score += Score(mine);
        }
        Console.WriteLine(score);
    }

    (Choice theirs, Choice mine) ParsePartTwo(string input)
    {
        var parts = input.Split(" ");
        var theirs = parts[0] switch {
            "A" => Choice.Rock,
            "B" => Choice.Paper,
            "C" => Choice.Scissors,
            string x => throw new Exception($"unknown code {x}")
        };
        var needed = parts[1] switch {
            "X" => Result.Lose,
            "Y" => Result.Draw,
            "Z" => Result.Win,
            string x => throw new Exception($"unknown code {x}")
        };

        var mine = (needed, theirs) switch {
            (Result.Win, Choice.Rock) => Choice.Paper,
            (Result.Win, Choice.Paper) => Choice.Scissors,
            (Result.Win, Choice.Scissors) => Choice.Rock,

            (Result.Lose, Choice.Rock) => Choice.Scissors,
            (Result.Lose, Choice.Paper) => Choice.Rock,
            (Result.Lose, Choice.Scissors) => Choice.Paper,

            (Result.Draw, Choice x) => x,

            _ => throw new Exception($"Invalid combo {needed} {theirs}"),
        };

        return (theirs, mine);
    }

    (Choice theirs, Choice mine) Parse(string input)
    {
        Choice parse(string s) => s switch {
            "A" or "X" => Choice.Rock,
            "B" or "Y" => Choice.Paper,
            "C" or "Z" => Choice.Scissors,
            string x => throw new Exception($"unknown code {x}"),
        };

        var parts = input.Split(" ");
        return (parse(parts[0]), parse(parts[1]));
    }

    Result Play(Choice mine, Choice theirs) => (mine, theirs) switch 
    {
        (Choice.Rock, Choice.Rock) => Result.Draw,
        (Choice.Rock, Choice.Paper) => Result.Lose,
        (Choice.Rock, Choice.Scissors) => Result.Win,

        (Choice.Paper, Choice.Rock) => Result.Win,
        (Choice.Paper, Choice.Paper) => Result.Draw,
        (Choice.Paper, Choice.Scissors) => Result.Lose,

        (Choice.Scissors, Choice.Rock) => Result.Lose,
        (Choice.Scissors, Choice.Paper) => Result.Win,
        (Choice.Scissors, Choice.Scissors) => Result.Draw,

        _ => throw new Exception($"invalid combination ({mine}, {theirs})"),
    };

    int Score(Choice choice) => choice switch {
        Choice.Rock => 1,
        Choice.Paper => 2,
        Choice.Scissors => 3,
        _ => throw new Exception(),
    };

    int Score(Result result) => result switch {
        Result.Win => 6,
        Result.Lose => 0,
        Result.Draw => 3,
        _ => throw new Exception(),
    };
 

    enum Choice
    {
        Rock,
        Paper,
        Scissors,
    }

    enum Result
    {
        Win,
        Lose,
        Draw,
    }
}

