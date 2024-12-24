using System.Diagnostics;

namespace AdventOfCode2024.Day23;

public class Day23 : Parser
{
    private static Dictionary<string, List<string>> _network = new();
    private static int _tripletsWithTComputers = 0;

    private static string minizincArguments = $"--solver cp-sat {HomeDir}/Day23/model.mzn ";
    private static string dznFilePath = $"{HomeDir}/Day23/data.dzn";
    private const string minizincCommand = "/Applications/MiniZincIDE.app/Contents/Resources/minizinc";
    private const string Unsatisfiable = "=====UNSATISFIABLE=====";

    public static void Run()
    {
        Console.WriteLine("Day 23");
        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        var triplets = new List<List<string>>();

        foreach (var computer in _network.Keys)
        {
            FillTriplets([computer], triplets);
        }

        Console.WriteLine($"Part1 solution: {_tripletsWithTComputers}");
    }

    private static void Part2()
    {
        List<string> maxCliqueNodes = [];

        var adjacencyMatrix = GenerateAdjacencyMatrix();
        var dznContent = GenerateDznContent(adjacencyMatrix);
        File.WriteAllText(dznFilePath, dznContent);

        var output = Solve(minizincCommand, minizincArguments, dznFilePath);
        var keys = _network.Keys.ToList();
        if (output != Unsatisfiable)
        {
            Console.WriteLine(output);
            var nodesString = output.Split("|")[1].Trim('[', ']', ' ', '\n', '\r', '-', '=');
            var nodes = nodesString.Split(',').Select(int.Parse).ToList();
            foreach (var node in nodes)
            {
                maxCliqueNodes.Add(keys[node - 1]);
            }
            
            maxCliqueNodes.Sort();
        }

        Console.WriteLine($"Part2 solution: {string.Join(",", maxCliqueNodes)}");
    }

    private static string Solve(string command, string arguments, string dznFilePath)
    {
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = $"{arguments} {dznFilePath}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        process.Start();
        return process.StandardOutput.ReadToEnd();
    }

    private static string GenerateDznContent(List<List<bool>> adjacencyMatrix)
    {
        int n = adjacencyMatrix.Count;
        var matrixString = new System.Text.StringBuilder($"n = {n};\nedges = [|");

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                matrixString.Append(adjacencyMatrix[i][j] ? "true" : "false");
                if (j < n - 1) matrixString.Append(", "); // Comma between elements in a row
            }

            if (i < n - 1) matrixString.Append(" | "); // Add `|` between rows
        }

        matrixString.Append(" |];\n"); // Close the matrix properly
        return matrixString.ToString();
    }



    private static List<List<bool>> GenerateAdjacencyMatrix()
    {
        var adjacencyMatrix = new List<List<bool>>();
        var keys = _network.Keys.ToList();
        foreach (var computer in keys)
        {
            var row = new List<bool>();
            foreach (var otherComputer in keys)
            {
                row.Add(_network[computer].Contains(otherComputer));
            }

            adjacencyMatrix.Add(row);
        }

        return adjacencyMatrix;
    }

    private static void FillTriplets(List<string> triplet, List<List<string>> triplets)
    {
        if (triplet.Count == 3)
        {
            var sortedTriplet = triplet.OrderBy(c => c).ToList();
            if (!triplets.Any(t => t.SequenceEqual(sortedTriplet)))
            {
                triplets.Add(sortedTriplet);
                if (triplet.Any(c => c[0] == 't')) _tripletsWithTComputers++;
            }

            return;
        }

        foreach (var computer in _network[triplet.Last()])
        {
            if (triplet.Count == 2 && _network[computer].Contains(triplet.First()) || triplet.Count == 1)
            {
                var newTriplet = new List<string>(triplet) { computer };
                FillTriplets(newTriplet, triplets);
            }
        }
    }

    private static void ParseInput()
    {
        var lines = ParseLines(23);
        foreach (var line in lines)
        {
            var computers = line.Split("-");
            if (!_network.ContainsKey(computers[0])) _network[computers[0]] = [];
            _network[computers[0]].Add(computers[1]);

            if (!_network.ContainsKey(computers[1])) _network[computers[1]] = [];
            _network[computers[1]].Add(computers[0]);
        }
    }
}