using System.Numerics;

namespace AdventOfCode2024.Day16;

public class Day16 : Parser
{
    private static List<string> _maze;
    private static int _height;
    private static int _width;
    private static (int, int) _start;
    private static (int, int) _end;

    private static readonly (int, int) StartingDirection = (0, 1);

    public static void Run()
    {
        Console.WriteLine("Day 16");

        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        Console.WriteLine($"Part1 solution: {FinishMaze()}");
    }

    private static int FinishMaze()
    {
        var visited = CreateVisited();
        visited[_start.Item1][_start.Item2] = 0;

        var pq = new PriorityQueue<(int, (int, int), (int, int)), int>();
        pq.Enqueue((0, _start, StartingDirection), 0);

        while (pq.Count > 0)
        {
            var (currentCost, currentPos, direction) = pq.Dequeue();

            if (currentPos == _end)
            {
                return currentCost;
            }

            var directionLeft = (-direction.Item2, direction.Item1);
            var directionRight = (direction.Item2, -direction.Item1);
            var posLeft = (currentPos.Item1 + directionLeft.Item1, currentPos.Item2 + directionLeft.Item2);
            var posRight = (currentPos.Item1 + directionRight.Item1, currentPos.Item2 + directionRight.Item2);
            var posForward = (currentPos.Item1 + direction.Item1, currentPos.Item2 + direction.Item2);

            Relax(posLeft, currentPos, directionLeft, visited, pq, currentCost, 1000);
            Relax(posRight, currentPos, directionRight, visited, pq, currentCost, 1000);
            Relax(posForward, currentPos, direction, visited, pq, currentCost);
        }

        return int.MaxValue;
    }

    private static int CountBestSeats()
{
    var parents = CreateParents();
    var visited = CreateVisited();
    visited[_start.Item1][_start.Item2] = 0;

    var endParents = new List<(int cost, (int, int) pos)>();
    var bestSeats = new HashSet<(int, int)>();

    var pq = new PriorityQueue<(int cost, (int, int) pos, (int, int) dir), int>();
    pq.Enqueue((0, _start, StartingDirection), 0);

    while (pq.Count > 0)
    {
        var (currentCost, currentPos, direction) = pq.Dequeue();

        var directionLeft = (-direction.Item2, direction.Item1);
        var directionRight = (direction.Item2, -direction.Item1);
        var posLeft = (currentPos.Item1 + directionLeft.Item1, currentPos.Item2 + directionLeft.Item2);
        var posRight = (currentPos.Item1 + directionRight.Item1, currentPos.Item2 + directionRight.Item2);
        var posForward = (currentPos.Item1 + direction.Item1, currentPos.Item2 + direction.Item2);

        Relax(posLeft, currentPos, directionLeft, visited, pq, currentCost, 1000, parents, endParents);
        Relax(posRight, currentPos, directionRight, visited, pq, currentCost, 1000, parents, endParents);
        Relax(posForward, currentPos, direction, visited, pq, currentCost, 0, parents, endParents);
    }

    endParents.Sort((x, y) => x.cost.CompareTo(y.cost));
    var minCost = endParents[0].cost;
    var queue = new Queue<(int, int)>();
    foreach (var (_, pos) in endParents.Where(x => x.cost == minCost))
    {
        bestSeats.Add(pos);
        queue.Enqueue(pos);
    }

    var visitedBacktrack = new HashSet<(int, int)>();
    while (queue.Count > 0)
    {
        var pos = queue.Dequeue();
        if (!visitedBacktrack.Add(pos)) continue;

        foreach (var parent in parents[pos.Item1][pos.Item2])
        {
            if (bestSeats.Add(parent))
            {
                queue.Enqueue(parent);
            }
        }
    }

    // PrintSeats(bestSeats);

    return bestSeats.Count;
}


    private static void Relax((int, int) newPos, (int, int) currPos, (int, int) direction,
        List<List<int>> visited, PriorityQueue<(int, (int, int), (int, int)), int> pq, int currentCost, int price = 0,
        List<List<List<(int, int)>>>? parents = null, List<(int, (int, int))>? endParents = null)
    {
        if (_maze[newPos.Item1][newPos.Item2] != '#')
        {
            var newCost = currentCost + 1 + price;
            visited[currPos.Item1][currPos.Item2] = Math.Max(visited[currPos.Item1][currPos.Item2], newCost - 1);

            if (visited[newPos.Item1][newPos.Item2] >= newCost)
            {
                if (parents != null)
                {
                    if (visited[newPos.Item1][newPos.Item2] > newCost)
                        parents[newPos.Item1][newPos.Item2] = [currPos];
                    else if (visited[newPos.Item1][newPos.Item2] == newCost)
                        parents[newPos.Item1][newPos.Item2].Add(currPos);
                }
                if (newPos == _end && endParents != null && !endParents.Contains((newCost, newPos)))
                {
                    endParents.Add((newCost, newPos));
                }
                visited[newPos.Item1][newPos.Item2] = newCost;
                pq.Enqueue((newCost, newPos, direction), newCost);
            }
        }
    }


    private static void PrintSeats(HashSet<(int, int)> seats)
    {
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                if (seats.Contains((y, x)))
                {
                    Console.Write("O");
                }
                else
                {
                    Console.Write(_maze[y][x]);
                }
            }

            Console.Write("\n");
        }

        Console.Write("\n");
    }

    private static List<List<int>> CreateVisited()
    {
        var visited = new List<List<int>>();
        for (var i = 0; i < _height; i++)
        {
            visited.Add([]);
            for (var j = 0; j < _width; j++)
            {
                visited[i].Add(int.MaxValue);
            }
        }

        return visited;
    }

    private static List<List<List<(int, int)>>> CreateParents()
    {
        var visited = new List<List<List<(int, int)>>>();
        for (var i = 0; i < _height; i++)
        {
            visited.Add([]);
            for (var j = 0; j < _width; j++)
            {
                visited[i].Add([]);
            }
        }

        return visited;
    }

    private static void Part2()
    {
        Console.WriteLine($"Part2 solution: {CountBestSeats()}");
    }

    private static void ParseInput()
    {
        _maze = ParseLines(16).ToList();
        _height = _maze.Count;
        _width = _maze[0].Length;

        for (var i = 0; i < _height; i++)
        {
            for (var j = 0; j < _width; j++)
            {
                if (_maze[i][j] == 'S')
                {
                    _start = (i, j);
                }
                else if (_maze[i][j] == 'E')
                {
                    _end = (i, j);
                }
            }
        }
    }
}