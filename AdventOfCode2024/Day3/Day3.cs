using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day3;

public class Day3 : Parser
{
    private static string _memory = "jd";
    private const string Pattern = @"(mul\((\d{1,3}),(\d{1,3})\))|(do\(\))|(don't\(\))";
    
    public static void Run()
    {
        Console.WriteLine("Day 3");

        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        var result = 0;

        var regex = new Regex(Pattern);
        var matches = regex.Matches(_memory);

        foreach (Match match in matches)
        {
            if (match.Groups[1].Success)
            {
                var firstValue = int.Parse(match.Groups[2].Value);
                var secondValue = int.Parse(match.Groups[3].Value);
                result += firstValue * secondValue;
            }
        }

        Console.WriteLine($"Part1 solution: {result}");
    }

    private static void Part2()
    {
        var result = 0;
        var enabled = 1;

        var regex = new Regex(Pattern);
        var matches = regex.Matches(_memory);

        foreach (Match match in matches)
        {
            if (match.Groups[1].Success)
            {
                var firstValue = int.Parse(match.Groups[2].Value);
                var secondValue = int.Parse(match.Groups[3].Value);
                result += firstValue * secondValue * enabled;
            }
            else if (match.Groups[4].Success) enabled = 1;
            else if (match.Groups[5].Success) enabled = 0;
        }

        Console.WriteLine($"Part2 solution: {result}");
    }


    private static void ParseInput()
    {
        foreach (var line in ParseLines(3)) _memory += line;
    }
}