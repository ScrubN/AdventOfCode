using System.Drawing;

namespace Day6;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllLines("Inputs.txt").Select(x => x.ToArray()).ToArray();

        var start = FindStart(input);

        var sum1 = Part1(input, start);
        var sum2 = Part2(input, start);

        Console.WriteLine($"Part 1: {sum1}");
        Console.WriteLine($"Part 2: {sum2}");
    }

    private static Point FindStart(char[][] input) {
        for (var y = 0; y < input.Length; y++) {
            for (var x = 0; x < input[y].Length; x++) {
                if (input[y][x] == '^') {
                    return new Point(x, y);
                }
            }
        }

        throw new Exception("No starting position was found.");
    }

    private static int Part1(char[][] input, Point start) {
        var positions = new HashSet<Point> { start };
        var current = start;
        while (true) {
            if (MoveUp1(input, current, positions) is not { } up) {
                break;
            }

            if (MoveRight1(input, up, positions) is not { } right) {
                break;
            }

            if (MoveDown1(input, right, positions) is not { } down) {
                break;
            }

            if (MoveLeft1(input, down, positions) is not { } left) {
                break;
            }

            current = left;
        }

        return positions.Count;
    }

    private static Point? MoveUp1(char[][] input, Point current, HashSet<Point> positions) {
        var last = current;
        for (var i = current.Y - 1; i >= 0; i--) {
            if (input[i][current.X] == '#') {
                return last;
            }

            last.Y = i;
            positions.Add(last);
        }

        return null;
    }

    private static Point? MoveDown1(char[][] input, Point current, HashSet<Point> positions) {
        var last = current;
        for (var i = current.Y + 1; i < input.Length; i++) {
            if (input[i][current.X] == '#') {
                return last;
            }

            last.Y = i;
            positions.Add(last);
        }

        return null;
    }

    private static Point? MoveLeft1(char[][] input, Point current, HashSet<Point> positions) {
        var last = current;
        for (var i = current.X - 1; i >= 0; i--) {
            if (input[current.Y][i] == '#') {
                return last;
            }

            last.X = i;
            positions.Add(last);
        }

        return null;
    }

    private static Point? MoveRight1(char[][] input, Point current, HashSet<Point> positions) {
        var last = current;
        for (var i = current.X + 1; i < input[current.Y].Length; i++) {
            if (input[current.Y][i] == '#') {
                return last;
            }

            last.X = i;
            positions.Add(last);
        }

        return null;
    }

    private static int Part2(char[][] input, Point start) {
        var sum = 0;
        for (var y = 0; y < input.Length; y++) {
            for (var x = 0; x < input[y].Length; x++) {
                if (input[y][x] != '.') {
                    continue;
                }

                var old = input[y][x];
                input[y][x] = '#';

                if (Part2Loop(start, input)) {
                    sum++;
                }

                input[y][x] = old;
            }
        }

        return sum;
    }

    private static bool Part2Loop(Point start, char[][] input) {
        var uPos = new HashSet<Point>();
        var rPos = new HashSet<Point>();
        var dPos = new HashSet<Point>();
        var lPos = new HashSet<Point>();
        var current = start;
        while (true) {
            if (MoveUp2(input, current) is not { } up) {
                break;
            }

            if (!uPos.Add(up)) {
                return true;
            }

            if (MoveRight2(input, up) is not { } right) {
                break;
            }

            if (!rPos.Add(right)) {
                return true;
            }

            if (MoveDown2(input, right) is not { } down) {
                break;
            }

            if (!dPos.Add(down)) {
                return true;
            }

            if (MoveLeft2(input, down) is not { } left) {
                break;
            }

            if (!lPos.Add(left)) {
                return true;
            }

            current = left;
        }

        return false;
    }

    private static Point? MoveUp2(char[][] input, Point current) {
        for (var i = current.Y - 1; i >= 0; i--) {
            if (input[i][current.X] == '#') {
                return current with { Y = i + 1 };
            }
        }

        return null;
    }

    private static Point? MoveDown2(char[][] input, Point current) {
        for (var i = current.Y + 1; i < input.Length; i++) {
            if (input[i][current.X] == '#') {
                return current with { Y = i - 1 };
            }
        }

        return null;
    }

    private static Point? MoveLeft2(char[][] input, Point current) {
        for (var i = current.X - 1; i >= 0; i--) {
            if (input[current.Y][i] == '#') {
                return current with { X = i + 1 };
            }
        }

        return null;
    }

    private static Point? MoveRight2(char[][] input, Point current) {
        for (var i = current.X + 1; i < input[current.Y].Length; i++) {
            if (input[current.Y][i] == '#') {
                return current with { X = i - 1 };
            }
        }

        return null;
    }
}