using CommunityToolkit.HighPerformance;

namespace Day4;

internal static class Program {
    public static void Main(string[] args) {
        var data = GetData();

        var part1 = Part1(data);
        var part2 = Part2(data);

        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
    }

    private static bool[,] GetData() {
        var lines = File.ReadAllLines("Inputs.txt");
        var data = new bool[lines.Length, lines[0].Length];

        for (var y = 0; y < lines.Length; y++) {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++) {
                data[x, y] = line[x] == '@';
            }
        }

        return data;
    }

    private static int Part1(bool[,] data) {
        var count = 0;

        var yLen = data.GetLength(1);
        var xLen = data.GetLength(0);
        for (var y = 0; y < yLen; y++)
        for (var x = 0; x < xLen; x++) {
            if (!data[x, y]) {
                continue;
            }

            if (IsValidTile(xLen, yLen, x, y, data, 3)) {
                count++;
            }
        }

        return count;
    }

    private static int Part2(bool[,] data) {
        var yLen = data.GetLength(1);
        var xLen = data.GetLength(0);

        var grid = new bool[yLen, xLen];
        data.AsSpan2D().CopyTo(grid);

        var startingRolls = grid.Count(true);
        while (true) {
            var removed = false;

            for (var y = 0; y < yLen; y++)
            for (var x = 0; x < xLen; x++) {
                if (!grid[x, y]) {
                    continue;
                }

                if (IsValidTile(xLen, yLen, x, y, grid, 3)) {
                    grid[x, y] = false;
                    removed = true;
                }
            }

            if (!removed) {
                break;
            }
        }

        var endingRolls = grid.Count(true);
        return startingRolls - endingRolls;
    }

    private static bool IsValidTile(int xLen, int yLen, int x, int y, bool[,] grid, int maxAdjacent) {
        var adjacent = 0;

        for (var i = -1; i <= 1; i++)
        for (var j = -1; j <= 1; j++) {
            if (i == 0 && j == 0) {
                continue;
            }

            var dX = x + i;
            var dY = y + j;
            if (dX < 0 || dX >= xLen || dY < 0 || dY >= yLen) {
                continue;
            }

            if (!grid[dX, dY]) {
                continue;
            }

            adjacent++;
            if (adjacent > maxAdjacent) {
                return false;
            }
        }

        return true;
    }
}