namespace AdventOfCode2024.Day2;

public class Day2 : Parser
{
    private static List<List<int>> _reports = new();
    private const int Range = 3;

    public static void Run()
    {
        Console.WriteLine("Day 2");

        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        var result = _reports.Count(report => GetErrorIndex(report) == -1);

        Console.WriteLine($"Part1 solution: {result}");
    }

    private static void Part2()
    {
        var result = 0;
        foreach (var report in _reports)
        {
            var errorIndex = GetErrorIndex(report);
            if (errorIndex == -1) result += 1;
            else if (CheckReportWithErrorRemoved(report, errorIndex)) result += 1;
            else if (errorIndex == 1 && CheckReportWithErrorRemoved(report, errorIndex - 1)) result += 1;
        }

        Console.WriteLine($"Part2 solution: {result}");
    }

    private static bool CheckReportWithErrorRemoved(List<int> report, int errorIndex)
    {
        return GetErrorIndex(report.Where((_, index) => index != errorIndex).ToList()) == -1 ||
               GetErrorIndex(report.Where((_, index) => index != errorIndex + 1).ToList()) == -1;
    }

    private static int GetErrorIndex(List<int> report)
    {
        int? isIncreasing = null;
        for (var i = 0; i < report.Count - 1; i++)
        {
            var (value1, value2) = (report[i], report[i + 1]);
            var isPairIncreasing = CheckMonotonicity(value1, value2);
            isIncreasing ??= isPairIncreasing;
            if (isPairIncreasing == 0 ||
                isIncreasing == -isPairIncreasing ||
                int.Abs(value1 - value2) > Range) return i;
        }

        return -1;
    }

    private static int CheckMonotonicity(int value1, int value2)
    {
        if (value1 < value2) return 1;
        if (value1 > value2) return -1;
        return 0;
    }

    private static void ParseInput()
    {
        foreach (var line in ParseLines(2))
        {
            _reports.Add(line.Split(" ").Select(int.Parse).ToList());
        }
    }
}