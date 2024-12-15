namespace AdventOfCode2024.Day15;

public class Day15 : Parser
{
    private static List<List<char>> _warehouse;
    private static (int, int) _pos;
    private static string _moves;

    private static readonly Dictionary<char, List<int>> Moves = new()
    {
        { '^', [-1, 0] },
        { 'v', [1, 0] },
        { '<', [0, -1] },
        { '>', [0, 1] }
    };

    public static void Run()
    {
        Console.WriteLine("Day 15");

        ParseInput();
        Part1();
        Part2();
    }

    private static void Part1()
    {
        foreach (var move in _moves)
        {
            var moveVec = Moves[move];
            var newPos = (_pos.Item1 + moveVec[0], _pos.Item2 + moveVec[1]);
            if (_warehouse[newPos.Item1][newPos.Item2] == '#') continue;
            if (_warehouse[newPos.Item1][newPos.Item2] == 'O') MoveBoxes(newPos, moveVec);
            if (_warehouse[newPos.Item1][newPos.Item2] != 'O')
            {
                _warehouse[_pos.Item1][_pos.Item2] = '.';
                _warehouse[newPos.Item1][newPos.Item2] = '@';
                _pos = newPos;
            }
        }

        Console.WriteLine($"Part1 solution: {GetGPSScore(_warehouse)}");
        ParseInput(); // Reset warehouse
    }

    private static void Part2()
    {
        var newWarehouse = EnlargeWarehouse(_warehouse);

        foreach (var move in _moves)
        {
            var moveVec = Moves[move];
            var newPos = (_pos.Item1 + moveVec[0], _pos.Item2 + moveVec[1]);
            var newElem = newWarehouse[newPos.Item1][newPos.Item2];
            if (newElem == '[') MoveBigBoxes([newPos, (newPos.Item1, newPos.Item2 + 1)], moveVec, newWarehouse);
            else if (newElem == ']') MoveBigBoxes([(newPos.Item1, newPos.Item2 - 1), newPos], moveVec, newWarehouse);
            if (newWarehouse[newPos.Item1][newPos.Item2] == '.')
            {
                newWarehouse[_pos.Item1][_pos.Item2] = '.';
                newWarehouse[newPos.Item1][newPos.Item2] = '@';
                _pos = newPos;
            }
        }

        Console.WriteLine($"Part2 solution: {GetGPSScore(newWarehouse)}");
    }


    private static void MoveBigBoxes(List<(int, int)> boxPos, List<int> moveVec, List<List<char>> warehouse)
    {
        if (moveVec == Moves['<'] || moveVec == Moves['>']) MoveBigBoxesLeftOrRight(boxPos, moveVec, warehouse);
        else if (CanMoveBigBoxesUpOrDown(boxPos, moveVec, warehouse)) MoveBigBoxesUpOrDown(boxPos, moveVec, warehouse);
    }

    private static void MoveBigBoxesUpOrDown(List<(int, int)> boxPos, List<int> moveVec, List<List<char>> warehouse)
    {
        var newBoxPosLeft = (boxPos[0].Item1 + moveVec[0], boxPos[0].Item2 + moveVec[1]);
        var newBoxPosRight = (boxPos[1].Item1 + moveVec[0], boxPos[1].Item2 + moveVec[1]);
        var newElemLeft = warehouse[newBoxPosLeft.Item1][newBoxPosLeft.Item2];
        var newElemRight = warehouse[newBoxPosRight.Item1][newBoxPosRight.Item2];


        if (newElemLeft == ']')
            MoveBigBoxesUpOrDown([(newBoxPosLeft.Item1, newBoxPosLeft.Item2 - 1), newBoxPosLeft], moveVec, warehouse);
        if (newElemRight == '[')
            MoveBigBoxesUpOrDown([newBoxPosRight, (newBoxPosRight.Item1, newBoxPosRight.Item2 + 1)], moveVec,
                warehouse);
        else if (newElemRight == ']') MoveBigBoxesUpOrDown([newBoxPosLeft, newBoxPosRight], moveVec, warehouse);


        warehouse[newBoxPosLeft.Item1][newBoxPosLeft.Item2] = '[';
        warehouse[newBoxPosRight.Item1][newBoxPosRight.Item2] = ']';
        warehouse[boxPos[0].Item1][boxPos[0].Item2] = '.';
        warehouse[boxPos[1].Item1][boxPos[1].Item2] = '.';
    }

    private static bool CanMoveBigBoxesUpOrDown(List<(int, int)> boxPos, List<int> moveVec, List<List<char>> warehouse)
    {
        var newBoxPosLeft = (boxPos[0].Item1 + moveVec[0], boxPos[0].Item2 + moveVec[1]);
        var newBoxPosRight = (boxPos[1].Item1 + moveVec[0], boxPos[1].Item2 + moveVec[1]);
        var newElemLeft = warehouse[newBoxPosLeft.Item1][newBoxPosLeft.Item2];
        var newElemRight = warehouse[newBoxPosRight.Item1][newBoxPosRight.Item2];

        var canMove = true;

        if (newElemLeft == '#' || newElemRight == '#') return false;
        if (newElemLeft == ']')
            canMove &= CanMoveBigBoxesUpOrDown([(newBoxPosLeft.Item1, newBoxPosLeft.Item2 - 1), newBoxPosLeft], moveVec,
                warehouse);

        if (newElemRight == '[')
            canMove &= CanMoveBigBoxesUpOrDown([newBoxPosRight, (newBoxPosRight.Item1, newBoxPosRight.Item2 + 1)],
                moveVec, warehouse);
        else if (newElemRight == ']')
            canMove &= CanMoveBigBoxesUpOrDown([newBoxPosLeft, newBoxPosRight], moveVec, warehouse);

        if (newElemLeft == ']' || newElemRight == '[' || newElemRight == ']') return canMove;

        if (canMove && warehouse[newBoxPosLeft.Item1][newBoxPosLeft.Item2] == '.' &&
            warehouse[newBoxPosRight.Item1][newBoxPosRight.Item2] == '.')
        {
            return true;
        }

        return false;
    }

    private static void MoveBigBoxesLeftOrRight(List<(int, int)> boxPos, List<int> moveVec, List<List<char>> warehouse)
    {
        var startingPos = boxPos[0];
        if (moveVec == Moves['<']) startingPos = boxPos[1];
        var nextPos = (startingPos.Item1 + moveVec[0], startingPos.Item2 + moveVec[1]);
        var nextElem = warehouse[nextPos.Item1][nextPos.Item2];
        if (nextElem == '[' || nextElem == ']') MoveBigBoxesLeftOrRight([nextPos, nextPos], moveVec, warehouse);
        if (warehouse[nextPos.Item1][nextPos.Item2] == '.')
        {
            warehouse[nextPos.Item1][nextPos.Item2] = warehouse[startingPos.Item1][startingPos.Item2];
            warehouse[startingPos.Item1][startingPos.Item2] = '.';
        }
    }

    private static void MoveBoxes((int, int) newPos, List<int> moveVec)
    {
        var nextPos = (newPos.Item1 + moveVec[0], newPos.Item2 + moveVec[1]);
        if (_warehouse[nextPos.Item1][nextPos.Item2] == 'O') MoveBoxes(nextPos, moveVec);
        if (_warehouse[nextPos.Item1][nextPos.Item2] == '.')
        {
            _warehouse[nextPos.Item1][nextPos.Item2] = 'O';
            _warehouse[newPos.Item1][newPos.Item2] = '.';
        }
    }

    private static List<List<char>> EnlargeWarehouse(List<List<char>> warehouse)
    {
        var height = warehouse.Count;
        var width = warehouse[0].Count;

        List<List<char>> newWarehouse = [];
        for (var y = 0; y < height; y++)
        {
            newWarehouse.Add(new List<char>());
            for (var x = 0; x < width; x++)
            {
                var firstElem = _warehouse[y][x] == 'O' ? '[' : _warehouse[y][x];
                var secondElem = firstElem == '[' ? ']' : firstElem == '@' ? '.' : firstElem;
                if (firstElem == '@') _pos = (y, newWarehouse[y].Count);
                newWarehouse[y].Add(firstElem);
                newWarehouse[y].Add(secondElem);
            }
        }

        return newWarehouse;
    }

    private static void PrintWearehouse(List<List<char>> warehouse)
    {
        var height = warehouse.Count;
        var width = warehouse[0].Count;

        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                Console.Write(warehouse[i][j]);
            }

            Console.WriteLine();
        }
    }

    private static int GetGPSScore(List<List<char>> warehouse)
    {
        var height = warehouse.Count;
        var width = warehouse[0].Count;

        var result = 0;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (warehouse[y][x] == 'O' || warehouse[y][x] == '[') result += y * 100 + x;
            }
        }

        return result;
    }

    private static void ParseInput()
    {
        _warehouse = [];
        _moves = "";
        var onMoves = false;
        var lines = ParseLines(15);

        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            if (line == "") onMoves = true;
            if (!onMoves)
            {
                var x = line.IndexOf('@');
                if (x != -1) _pos = (x, y);
                _warehouse.Add(line.ToList());
            }
            else _moves += line;
        }
    }
}