namespace Advent;

public class Two : AdventDay
{
    public override int Day => 2;

    protected override void PartOne(string input)
    {
        var score = 0;
        foreach (var round in input.Split("\n"))
        {
            var (theirs, mine) = Parse(round);
            score += Score(Play(mine, theirs));
            score += Score(mine);
        }
        Console.WriteLine(score);
    }

    protected override void PartTwo(string input)
    {
        var score = 0;
        foreach (var round in input.Split("\n"))
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

        var mine = needed switch {
            Result.Win => theirs switch {
                Choice.Rock => Choice.Paper,
                Choice.Paper => Choice.Scissors,
                Choice.Scissors => Choice.Rock,
                _ => throw new Exception(),
            },
            Result.Lose => theirs switch {
                Choice.Rock => Choice.Scissors,
                Choice.Paper => Choice.Rock,
                Choice.Scissors => Choice.Paper,
                _ => throw new Exception(),
            },
            Result.Draw => theirs,
            _ => throw new Exception(),
        };

        return (theirs, mine);
    }

    (Choice theirs, Choice mine) Parse(string input)
    {
        var parts = input.Split(" ");
        var theirs = parts[0] switch {
            "A" => Choice.Rock,
            "B" => Choice.Paper,
            "C" => Choice.Scissors,
            string x => throw new Exception($"unknown code {x}")
        };
        var mine = parts[1] switch {
            "X" => Choice.Rock,
            "Y" => Choice.Paper,
            "Z" => Choice.Scissors,
            string x => throw new Exception($"unknown code {x}")
        };
        return (theirs, mine);
    }

    Result Play(Choice mine, Choice theirs) => mine switch
    {
        Choice.Rock => theirs switch {
            Choice.Rock => Result.Draw,
            Choice.Paper => Result.Lose,
            Choice.Scissors => Result.Win,
            _ => throw new Exception(),
        },
        Choice.Paper => theirs switch {
            Choice.Rock => Result.Win,
            Choice.Paper => Result.Draw,
            Choice.Scissors => Result.Lose,
            _ => throw new Exception(),
        },
        Choice.Scissors => theirs switch {
            Choice.Rock => Result.Lose,
            Choice.Paper => Result.Win,
            Choice.Scissors => Result.Draw,
            _ => throw new Exception(),
        },
        _ => throw new Exception(),
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

