namespace AdventOfCode2024.Day5;

public class Day5 : Parser
{
    private static List<Dictionary<int, List<int>>> _orderMap = new();
    private static List<int[]> _orders = new();

    public static void Run()
    {
        Console.WriteLine("Day 5");

        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        var result = 0;
        var toDelete = new List<Dictionary<int, List<int>>>();
        
        foreach (var map in _orderMap)
        {
            var keys = map.Keys;
            foreach (var order in _orders)
            {
                if (keys.Contains(order[0]) && keys.Contains(order[1])) map[order[0]].Add(order[1]);
            }

            var correctnessValue = CheckCorrectness(map, keys.ToArray());

            if (correctnessValue > 0)
            {
                result += correctnessValue;
                toDelete.Add(map);
            }
        }

        _orderMap.RemoveAll(map => toDelete.Contains(map));
        
        Console.WriteLine($"Part1 solution: {result}");
    }

    private static void Part2()
    {
        var result = 0;
        
        foreach (var map in _orderMap)
        {
            var keys = map.Keys;
            result += CorrectErrors(map, keys.ToArray());
        }
        
        Console.WriteLine($"Part2 solution: {result}");
    }

    private static int CorrectErrors(Dictionary<int, List<int>> map, int[] keys)
    {
        var middle = (int) Math.Floor((double) keys.Length / 2);
        foreach (var kvPair in map)
        {
            if (kvPair.Value.Count == middle) return kvPair.Key;
        }

        return 0;

    }
    private static int CheckCorrectness(Dictionary<int, List<int>> map, int[] keys)
    {
        var keysLength = keys.Length;
        for (var i = 0; i < keysLength; i++)
        {
            var pagesAfter = map[keys[i]];
            if (pagesAfter.Count != keysLength - i - 1) return 0;
        }
        
        return keys[(int)Math.Floor((double) keysLength / 2)];
    }

    private static void ParseInput()
    {
        var foundSeparator = false;
        foreach (var line in ParseLines(5))
        {
            if (line == "")
            {
                foundSeparator = true;
                continue;
            }

            if (!foundSeparator)
            {
                var order = line.Split('|').Select(int.Parse).ToArray();
                _orders.Add(order);
            }
            else
            {
                var keys = line.Split(',').Select(int.Parse).ToList();
                var map = new Dictionary<int, List<int>>();
                keys.ForEach(key => map.Add(key, []));
                _orderMap.Add(map);
            }
        }
    }
}