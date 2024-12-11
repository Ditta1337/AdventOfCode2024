namespace AdventOfCode2024.Day11;

public class Day11 : Parser
{
    private static List<long> _stones;
    private static Dictionary<(long, int, int), long> _memo = new();

    public static void Run()
    {
        Console.WriteLine("Day 11");

        ParseInput();

        Part1();
        Part2();
    }

    private static void Part1()
    {
        long result = 0;

        foreach (var stone in _stones)
        {
            result += SplitStones(stone, 0, 25);
        }

        Console.WriteLine($"Part1 solution: {result}");
    }

    private static void Part2()
    {
        long result = 0;

        foreach (var stone in _stones)
        {
            result += SplitStones(stone, 0, 75);
        }

        Console.WriteLine($"Part2 solution: {result}");
    }

    private static long SplitStones(long stone, int blink, int blinks)
    {
        if (_memo.ContainsKey((stone, blink, blinks))) return _memo[(stone, blink, blinks)];

        if (blink == blinks) return 1;

        long result;

        if (stone == 0) result = SplitStones(1, blink + 1, blinks);
        else
        {
            var length = (int)Math.Log10(stone) + 1;
            if (length % 2 == 0)
            {
                var middle = (long)Math.Pow(10, length / 2);
                result = SplitStones(stone / middle, blink + 1, blinks) +
                         SplitStones(stone % middle, blink + 1, blinks);
            }
            else result = SplitStones(stone * 2024, blink + 1, blinks);
        }

        _memo[(stone, blink, blinks)] = result;
        return result;
    }

    private static void ParseInput()
    {
        _stones = ParseLines(11)[0].Split(' ').Select(long.Parse).ToList();
    }
}