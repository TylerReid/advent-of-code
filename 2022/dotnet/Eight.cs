namespace Advent;

public class Eight : AdventDay
{
    public int Day => 8;

    public void PartOne(string input)
    {
        var forest = Parse(input);
        var visible = Visible(forest);
        Console.WriteLine(visible.Count());
    }

    public void PartTwo(string input)
    {
        var forest = Parse(input);
        var maxSceneScore = 0;
        for (var x = 0; x < forest.Count(); x++)
        {
            for (var y = 0; y < forest[x].Count(); y++)
            {
                var score = SceneScore(forest, (x, y));
                maxSceneScore = score > maxSceneScore ? score : maxSceneScore;
            }
        }

        Console.WriteLine(maxSceneScore);
    }

    List<List<int>> Parse(string input)
    {
        var forest = new List<List<int>>();
        foreach (var line in input.SplitLines())
        {
            var row = new List<int>();
            foreach (var c in line.ToCharArray())
            {
                row.Add(int.Parse(c.ToString()));
            }
            forest.Add(row);
        }
        return forest;
    }

    int SceneScore(List<List<int>> forest, (int x, int y) pos)
    {
        var maxX = forest.Count();
        var maxY = forest.First().Count();
        var treeHouseHeight = forest[pos.x][pos.y];

        var a = 0;
        for (var i = 1; pos.x + i < maxX; i++)
        {
            var tree = forest[pos.x + i][pos.y];
            a += 1;
            if (tree >= treeHouseHeight) break;
        }

        var b = 0;
        for (var i = 1; pos.x - i >= 0; i++)
        {
            var tree = forest[pos.x - i][pos.y];
            b += 1;
            if (tree >= treeHouseHeight) break;
        }

        var c = 0;
        for (var i = 1; pos.y + i < maxY; i++)
        {
            var tree = forest[pos.x][pos.y + i];
            c += 1;
            if (tree >= treeHouseHeight) break;
        }

        var d = 0;
        for (var i = 1; pos.y - i >= 0; i++)
        {
            var tree = forest[pos.x][pos.y - i]; 
            d += 1;
            if (tree >= treeHouseHeight) break;
        }

        return a * b * c * d;
    }


    HashSet<(int x, int y)> Visible(List<List<int>> forest)
    {
        var visible = new HashSet<(int x, int y)>();

        foreach (var (i, line) in forest.Enumerate())
        {
            foreach (var tree in Visible(line))
            {
                visible.Add((i, tree));
            }
        }

        for (var i = 0; i < forest.First().Count(); i++)
        {
            // this is dumb, turn a vertical line into a list, could probably just do some weird enumerable
            var derplist = new List<int>();
            foreach (var line in forest)
            {
                derplist.Add(line[i]);
            }
            foreach (var tree in Visible(derplist))
            {
                visible.Add((tree, i));
            }
        }

        return visible;
    }

    HashSet<int> Visible(List<int> line)
    {
        var visible = new HashSet<int>();
        var tallest = line.First();
        visible.Add(0);
        foreach (var (j, tree) in line.Enumerate().Skip(1))
        {
            if (tallest == 9) break;
            if (tree > tallest)
            {
                visible.Add(j);
                tallest = tree;
            }
        }

        tallest = line.Last();
        visible.Add(line.Count() - 1);
        foreach (var (j, tree) in line.Enumerate().Reverse().Skip(1))
        {
            if (tallest == 9) break;
            if (tree > tallest)
            {
                visible.Add(j);
                tallest = tree;
            }
        }

        return visible;
    }

}