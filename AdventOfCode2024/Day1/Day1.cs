namespace AdventOfCode2024.Day1;

public class Day1 : Parser
{
    private static List<int> _leftList = new();
    private static List<int> _rightList = new();

    public static void Run()
    {
        Console.WriteLine("Day 1");
        
        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        _leftList.Sort();
        _rightList.Sort();

        var result = _leftList.Select((leftVal, i) => int.Abs(leftVal - _rightList[i])).Sum();

        Console.WriteLine($"Part 1 solution: {result}");
    }

    private static void Part2()
    {
        var result = 0;
        
        var leftListSet = _leftList.ToHashSet();

        foreach (var leftVal in leftListSet)
        {
            result += _rightList.FindAll(rightVal => rightVal == leftVal).Count * leftVal;
        }
        
        Console.WriteLine($"Part 2 solution: {result}");
    }

    private static void ParseInput()
    {
        foreach (var line in ParseLines(1))
        {
            var numbers = line.Split("   ").Select(int.Parse).ToArray();
            _leftList.Add(numbers[0]);
            _rightList.Add(numbers[1]);
        }
    }
}