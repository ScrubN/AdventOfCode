using System.Drawing;

namespace Day6;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllLines("Inputs.txt");

        Point start = default;
        for (var y = 0; y < input.Length; y++) {
            for (var x = 0; x < input[y].Length; x++) {
                if (input[y][x] == '^') {
                    start = new Point(x, y);
                }
            }
        }

        var positions = new HashSet<Point>();
        var current = start;
        while (true) {
            if (MoveUp(input, current, positions) is not { } up) {
                break;
            }

            if (MoveRight(input, up, positions) is not { } right) {
                break;
            }

            if (MoveDown(input, right, positions) is not { } down) {
                break;
            }

            if (MoveLeft(input, down, positions) is not { } left) {
                break;
            }

            current = left;
        }

        Console.WriteLine(positions.Count);
    }

    private static Point? MoveUp(string[] input, Point current, HashSet<Point> positions) {
        var last = current;
        for (var i = current.Y - 1; i >= 0; i--) {
            if (input[i][current.X] == '#') {
                return last;
            }

            last = current with { Y = i };
            positions.Add(last);
        }

        return null;
    }

    private static Point? MoveDown(string[] input, Point current, HashSet<Point> positions) {
        var last = current;
        for (var i = current.Y + 1; i < input.Length; i++) {
            if (input[i][current.X] == '#') {
                return last;
            }

            last = current with { Y = i };
            positions.Add(last);
        }

        return null;
    }

    private static Point? MoveRight(string[] input, Point current, HashSet<Point> positions) {
        var last = current;
        for (var i = current.X + 1; i < input[current.Y].Length; i++) {
            if (input[current.Y][i] == '#') {
                return last;
            }

            last = current with { X = i };
            positions.Add(last);
        }

        return null;
    }
    private static Point? MoveLeft(string[] input, Point current, HashSet<Point> positions) {
        var last = current;
        for (var i = current.X - 1; i >= 0; i--) {
            if (input[current.Y][i] == '#') {
                return last;
            }

            last = current with { X = i };
            positions.Add(last);
        }

        return null;
    }
}