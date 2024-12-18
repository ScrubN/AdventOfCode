using System.Diagnostics;
using System.Drawing;

namespace Day18;

internal static class Program {
    private const int GRID_SIZE = 71;

    private record BfsNode(BfsNode? Parent, Point Pos);

    internal static void Main(string[] args) {
        var input = File.ReadLines("Inputs.txt")
            .Select(x => new Point(int.Parse(x.AsSpan(0, x.IndexOf(','))), int.Parse(x.AsSpan(x.IndexOf(',') + 1))))
            .ToArray();

        var steps = Part1(input);
        var point = Part2(input);

        Console.WriteLine($"Part 1: {steps}");
        Console.WriteLine($"Part 2: {point.X},{point.Y}");
    }

    private static int Part1(Point[] input) {
        var grid = Enumerable.Repeat(false, GRID_SIZE).Select(_ => Enumerable.Repeat(false, GRID_SIZE).ToArray()).ToArray();

        for (var i = 0; i < 1024; i++) {
            var point = input[i];
            if (point.Y >= grid.Length || point.X >= grid[0].Length) {
                continue;
            }

            grid[point.Y][point.X] = true;
        }

        return Bfs(grid)?.Count ?? -1;
    }

    private static Point Part2(Point[] input) {
        var grid = Enumerable.Repeat(false, GRID_SIZE).Select(_ => Enumerable.Repeat(false, GRID_SIZE).ToArray()).ToArray();

        var i = 0;
        for (; i < 1024; i++) {
            var point = input[i];
            grid[point.Y][point.X] = true;
        }

        HashSet<Point>? path = null;
        while (true) {
            path ??= Bfs(grid);
            if (path == null) {
                return input[i];
            }

            i++;
            var point = input[i];
            grid[point.Y][point.X] = true;

            // Only recompute BFS if the newly added point is on the solution path
            if (path.Contains(point)) {
                path = null;
            }
        }
    }

    private static HashSet<Point>? Bfs(bool[][] grid) {
        var discovered = new HashSet<Point>();
        var queue = new Queue<BfsNode>();
        queue.Enqueue(new BfsNode(null, new Point(0, 0)));

        var end = new Point(grid.Length - 1, grid[0].Length - 1);
        while (queue.TryDequeue(out var node)) {
            if (node.Pos == end) {
                return RetracePath(node);
            }

            for (var y = -1; y <= 1; y++) {
                var y2 = node.Pos.Y + y;
                if (y2 < 0 || y2 >= grid.Length) {
                    continue;
                }

                for (var x = -1; x <= 1; x++) {
                    var x2 = node.Pos.X + x;
                    if (x2 < 0 || x2 >= grid[0].Length) {
                        continue;
                    }

                    if (Math.Abs(x) == Math.Abs(y)) {
                        continue;
                    }

                    if (grid[y2][x2]) {
                        continue;
                    }

                    var newPoint = new Point(x2, y2);
                    if (discovered.Add(newPoint)) {
                        queue.Enqueue(new BfsNode(node, newPoint));
                    }
                }
            }
        }

        return null;
    }

    private static HashSet<Point> RetracePath(BfsNode node) {
        var path = new HashSet<Point>();
        while (node.Parent != null) {
            path.Add(node.Pos);
            node = node.Parent;
        }

        return path;
    }
}