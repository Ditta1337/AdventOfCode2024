namespace AdventOfCode2024.Day22;

public class Day22 : Parser
{
    private static List<int> _startingNums = [];
    private static List<List<int>> _prices = [];
    private static List<List<int>> _diffs = [];
    private static List<Dictionary<(int, int, int, int), int>> _memo = new();
    
    private const int PruneConst = 16777216;

    public static void Run()
    {
        Console.WriteLine("Day 22");
        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        long result = 0;
        foreach (var num in _startingNums)
        {
            List<int> generatedPrices = [num % 10];
            long newNum = num;
            for (var i = 0; i < 2000; i++)
            {
                newNum = ((newNum << 6) ^ newNum) % PruneConst;
                newNum = ((newNum >> 5) ^ newNum) % PruneConst;
                newNum = ((newNum << 11) ^ newNum) % PruneConst;
                generatedPrices.Add((int)(newNum % 10));
            }
            
            _prices.Add(generatedPrices);
            result += newNum;

            List<int> diffs = [];
            for (var j = 1; j < generatedPrices.Count; j++)
            {
                diffs.Add(generatedPrices[j] - generatedPrices[j - 1]);
            }
            
            _diffs.Add(diffs);
        }
        

        Console.WriteLine($"Part1 solution: {result}");
    }

    private static void Part2()
    {
        var pricesIndex = 0;
        foreach (var diffs in _diffs)
        {
            var diffMemo = new Dictionary<(int, int, int, int), int>();
            for (var i = 0; i < diffs.Count - 4; i++)
            {
                var key = (diffs[i], diffs[i + 1], diffs[i + 2], diffs[i + 3]);
                if (!diffMemo.ContainsKey(key)) diffMemo[key] = _prices[pricesIndex][i + 4];
            }
            _memo.Add(diffMemo);
            pricesIndex++;
        }
        
        var combinedDiffMemo = new Dictionary<(int, int, int, int), int>();
        
        foreach (var memo in _memo)
        {
            foreach (var kvp in memo)
            {
                if (combinedDiffMemo.ContainsKey(kvp.Key)) combinedDiffMemo[kvp.Key] += kvp.Value;
                else combinedDiffMemo[kvp.Key] = kvp.Value;
            }
        }
        
        Console.WriteLine($"Part2 solution: {combinedDiffMemo.Values.Max()}");
    }

    private static void ParseInput()
    {
        _startingNums = ParseLines(22).Select(int.Parse).ToList();
    }
}