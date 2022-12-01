namespace advent;

class Input
{
    public static (string? sample, string? problem) GetInput(string day)
    {
        string? TryRead(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (FileNotFoundException) 
            {
                return null;
            }
        }

        return (TryRead(Path.Combine("input", $"{day}.sample")), 
            TryRead(Path.Combine("input", $"{day}.problem")));
    }
}