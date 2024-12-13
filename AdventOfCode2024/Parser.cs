namespace AdventOfCode2024;

public abstract class Parser
{
    protected static readonly string HomeDir = Path.Combine(AppContext.BaseDirectory, "../../../");

    protected static string[] ParseLines(int day)
    {
        try
        {
            var filePath = Path.Combine(HomeDir, $"Day{day}", $"input{day}.txt");
            return File.ReadAllLines(filePath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return [];
        }
    }
}