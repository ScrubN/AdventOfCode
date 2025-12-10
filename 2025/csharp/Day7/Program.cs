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

        Console.WriteLine($"Part 1: {part1}");
        // Console.WriteLine($"Part 2: {part2}");
    }

    private static string[] GetData() {
        return File.ReadAllLines("Inputs.txt");
    }

    private static int Part1(string[] data) {
        var start = data[0].IndexOf('S');
        var splits = new HashSet<Point>();

        CountSplits(new Point(start, 1), splits, data);

        return splits.Count;
    }

    private static void CountSplits(Point p, HashSet<Point> splits, string[] data) {
        if (p.X < 0 || p.X >= data[0].Length || p.Y >= data.Length) {
            return;
        }

        if (splits.Contains(p)) {
            return;
        }

        if (data.At(p) == '^') {
            splits.Add(p);

            CountSplits(p with { X = p.X - 1 },  splits, data);
            CountSplits(p with { X = p.X + 1 },  splits, data);
        }
        else {
            CountSplits(p with{ Y = p.Y + 1}, splits, data);
        }
    }
}