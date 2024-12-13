using System.Security.Principal;

namespace AdventOfCode2024.Day12;

public class Day12 : Parser
{
    private static List<List<char>> _farm = [];
    private static int _height;
    private static int _width;

    private static readonly int[][] Moves = new int[][]
    {
        [-1, 0],
        [0, 1],
        [1, 0],
        [0, -1]
    };


    public static void Run()
    {
        Console.WriteLine("Day 11");

        ParseInput();

        Part1();
        Part2();
    }

    private static void Part1()
    {
        var result = 0;

        var visited = CreateVisited();

        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                if (!visited[y][x])
                {
                    var price = GetPrice(y, x, visited);
                    result += price;
                }
            }
        }

        Console.WriteLine($"Part1 solution: {result}");
    }

    private static void Part2()
    {
        var result = 0;

        var visited = CreateVisited();

        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                if (!visited[y][x])
                {
                    var price = GetDiscountedPrice(y, x, visited);
                    result += price;
                }
            }
        }

        Console.WriteLine($"Part2 solution: {result}");
    }


    private static int GetPrice(int y, int x, List<List<bool>> globalVisited)
    {
        var area = 0;
        var perimeter = 0;

        var visited = CreateVisited();
        visited[y][x] = true;

        List<int> start = [y, x];
        var queue = new Stack<List<int>>();
        queue.Push(start);

        while (queue.Count != 0)
        {
            var currField = queue.Pop();
            area++;

            foreach (var move in Moves)
            {
                var newY = currField[0] + move[0];
                var newX = currField[1] + move[1];
                if (InBounds(newY, newX))
                {
                    List<int> newField = [newY, newX];
                    if (_farm[currField[0]][currField[1]] != _farm[newY][newX]) perimeter++;

                    if (_farm[currField[0]][currField[1]] == _farm[newY][newX] && !visited[newY][newX])
                    {
                        queue.Push(newField);
                        visited[newY][newX] = true;
                        globalVisited[newY][newX] = true;
                    }
                }
                else
                {
                    perimeter++;
                }
            }
        }

        var ret = area * perimeter;
        return ret;
    }

    private static int GetDiscountedPrice(int y, int x, List<List<bool>> globalVisited)
    {
        var area = 0;

        var minX = int.MaxValue;
        var minY = int.MaxValue;
        var maxX = 0;
        var maxY = 0;

        var visited = CreateVisited();
        visited[y][x] = true;

        List<int> start = [y, x];
        var queue = new Stack<List<int>>();
        queue.Push(start);

        while (queue.Count != 0)
        {
            var currField = queue.Pop();
            area++;

            minY = Math.Min(minY, currField[0]);
            minX = Math.Min(minX, currField[1]);
            maxY = Math.Max(maxY, currField[0]);
            maxX = Math.Max(maxX, currField[1]);

            foreach (var move in Moves)
            {
                var newY = currField[0] + move[0];
                var newX = currField[1] + move[1];
                if (InBounds(newY, newX))
                {
                    List<int> newField = [newY, newX];

                    if (_farm[currField[0]][currField[1]] == _farm[newY][newX] && !visited[newY][newX])
                    {
                        queue.Push(newField);
                        visited[newY][newX] = true;
                        globalVisited[newY][newX] = true;
                    }
                }
            }
        }

        var perimeter = GetFences(minX, minY, maxX, maxY, visited);

        return area * perimeter;
    }

    private static int GetFences(int minX, int minY, int maxX, int maxY, List<List<bool>> visited)
    {
        var fences = 0;
        for (var x = minX; x <= maxX; x++)
        {
            var foundRight = false;
            var foundLeft = false;
            for (var y = minY; y <= maxY; y++)
            {
                if (visited[y][x])
                {
                    if (!foundRight && (!InBounds(y, x + 1) || !visited[y][x + 1]))
                    {
                        foundRight = true;
                        fences += 1;
                    }
                    else if (foundRight && InBounds(y, x + 1) && visited[y][x + 1])
                    {
                        foundRight = false;
                    }

                    if (!foundLeft && (!InBounds(y, x - 1) || !visited[y][x - 1]))
                    {
                        foundLeft = true;
                        fences += 1;
                    }
                    else if (foundLeft && InBounds(y, x - 1) && visited[y][x - 1])
                    {
                        foundLeft = false;
                    }
                }
                else
                {
                    foundLeft = false;
                    foundRight = false;
                }
            }
        }


        for (var y = minY; y <= maxY; y++)
        {
            var foundTop = false;
            var foundBottom = false;
            for (var x = minX; x <= maxX; x++)
            {
                if (visited[y][x])
                {
                    if (!foundBottom && (!InBounds(y + 1, x) || !visited[y + 1][x]))
                    {
                        foundBottom = true;
                        fences += 1;
                    }
                    else if (foundBottom && InBounds(y + 1, x) && visited[y + 1][x])
                    {
                        foundBottom = false;
                    }

                    if (!foundTop && (!InBounds(y - 1, x) || !visited[y - 1][x]))
                    {
                        foundTop = true;
                        fences += 1;
                    }
                    else if (foundTop && InBounds(y - 1, x) && visited[y - 1][x])
                    {
                        foundTop = false;
                    }
                }
                else
                {
                    foundBottom = false;
                    foundTop = false;
                }
            }
        }

        return fences;
    }

    private static List<List<bool>> CreateVisited()
    {
        var visited = new List<List<bool>>();
        for (var i = 0; i < _height; i++)
        {
            visited.Add([]);
            for (var j = 0; j < _width; j++)
            {
                visited[i].Add(false);
            }
        }

        return visited;
    }


    private static void PrintVisited(List<List<bool>> visited, int currY, int currX)
    {
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                if (y == currY && x == currX) Console.Write("@ ");
                else if (visited[y][x]) Console.Write("# ");
                else Console.Write(". ");
            }

            Console.Write("\n");
        }

        Console.WriteLine("--------------------");
    }


    private static bool InBounds(int y, int x)
    {
        return y >= 0 &&
               y < _height &&
               x >= 0 &&
               x < _width;
    }

    private static void ParseInput()
    {
        var lines = ParseLines(12);
        _height = lines.Length;
        _width = lines[0].Length;

        for (var y = 0; y < _height; y++)
        {
            var row = new List<char>();
            for (var x = 0; x < _width; x++)
            {
                row.Add(lines[y][x]);
            }

            _farm.Add(row);
        }
    }
}