// MiniZinc is required

namespace AdventOfCode2024.Day13;

using System.Diagnostics;
using System.Text.RegularExpressions;

public class Day13 : Parser
{
    private static List<List<long>> _machines = [];
    private const string Pattern = @"[XY][\+=](\d*)";
    private static string minizincArguments = $"--solver cp-sat {HomeDir}/Day13/model.mzn -D ";
    private const string minizincCommand = "/Applications/MiniZincIDE.app/Contents/Resources/minizinc";
    private const string Unsatisfiable = "=====UNSATISFIABLE=====";

    public static void Run()
    {
        Console.WriteLine("Day 13");

        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        long result = 0;
        var counter = 0;

        foreach (var machine in _machines)
        {
            Console.WriteLine($"Machine {counter++}");
            var dataInput = GenerateDataInput(machine);
            var output = Solve(minizincCommand, minizincArguments + dataInput).Split("\n")[0];
            if (output != Unsatisfiable)
            {
                var split = output.Split(" ").Select(long.Parse).ToList();
                result += split[0] * 3 + split[1];
            }
        }

        Console.WriteLine($"Part1 solution: {result}");
    }

    private static void Part2()
    {
        long result = 0;
        var counter = 0;

        foreach (var machine in _machines)
        {
            Console.WriteLine($"Machine {counter++}");
            var dataInput = GenerateDataInput(machine, 10000000000000);
            var output = Solve(minizincCommand, minizincArguments + dataInput).Split("\n")[0];
            if (output != Unsatisfiable)
            {
                var split = output.Split(" ").Select(long.Parse).ToList();
                result += split[0] * 3 + split[1];
            }
        }

        Console.WriteLine($"Part2 solution: {result}");
    }

    private static string Solve(string command, string arguments)
    {
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = arguments,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        process.Start();
        return process.StandardOutput.ReadToEnd();
    }

    private static string GenerateDataInput(List<long> machine, long error = 0)
    {
        machine[4] += error;
        machine[5] += error;
        
        var maxA = Math.Min(machine[4] / machine[0], machine[5] / machine[1]);
        var maxB = Math.Min(machine[4] / machine[2], machine[5] / machine[3]);
        
        return
            $"\"maxA={maxA};maxB={maxB};x1={machine[0]};y1={machine[1]};" +
            $"x2={machine[2]};y2={machine[3]};t1={machine[4]};t2={machine[5]};\"";
    }


    private static void ParseInput()
    {
        var regex = new Regex(Pattern);
        var machine = new List<long>();

        foreach (var line in ParseLines(13))
        {
            var matches = regex.Matches(line);
            foreach (Match match in matches)
            {
                machine.Add(long.Parse(match.Groups[1].Value));
            }

            if (line == "")
            {
                _machines.Add(machine);
                machine = [];
            }
        }

        _machines.Add(machine);
    }
}