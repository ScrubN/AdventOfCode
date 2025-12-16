using System.Drawing;

namespace Day7;

internal static class Program {
    extension(string[] data) {
        private char At(Point p) {
            return data[p.Y][p.X];
        }
    }

    private static void Main(string[] args) {
        var data = GetData();

        var part1 = Part1(data);
        var part2 = Part2(data);

        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
    }

    private static string[] GetData() {
        return File.ReadAllLines("Inputs.txt");
    }

    private static int Part1(string[] data) {
        var start = new Point(data[0].IndexOf('S'), 0);
        var splits = new HashSet<Point>();

        CountSplits(start, splits, data);

        return splits.Count;
    }

    private static void CountSplits(Point p, HashSet<Point> splits, string[] data) {
        if (p.X < 0 || p.X >= data[0].Length || p.Y >= data.Length) {
            return;
        }

        while (data.At(p) != '^') {
            p = p with { Y = p.Y + 1 };

            if (p.Y >= data.Length) {
                return;
            }
        }

        if (!splits.Add(p)) {
            return;
        }

        CountSplits(new Point(p.X - 1, p.Y + 1), splits, data);
        CountSplits(new Point(p.X + 1, p.Y + 1), splits, data);
    }

    private static long Part2(string[] data) {
        var start = new Point(data[0].IndexOf('S'), 0);

        return CountChildren(start, data, []);
    }

    private static long CountChildren(Point p, string[] data, Dictionary<Point, long> costs) {
        if (p.X < 0 || p.X >= data[0].Length) {
            return 0;
        }

        if (p.Y >= data.Length) {
            return 1;
        }

        while (data.At(p) != '^') {
            p = p with { Y = p.Y + 1 };

            if (p.Y >= data.Length) {
                return 1;
            }
        }

        if (costs.TryGetValue(p, out var visitedCost)) {
            return visitedCost;
        }

        var left = CountChildren(new Point(p.X - 1, p.Y + 1), data, costs);
        var right = CountChildren(new Point(p.X + 1, p.Y + 1), data, costs);
        var cost = left + right;

        costs.Add(p, cost);
        return cost;
    }
}