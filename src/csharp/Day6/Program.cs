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

        var sum = 0;
        for (var y = 0; y < input.Length; y++) {
            for (var x = 0; x < input[y].Length; x++) {
                if (input[y][x] == '.') {
                    // This is awful
                    var old = input[y];
                    var arr = old.ToArray();
                    arr[x] = '#';
                    input[y] = new string(arr);
                    if (RunLoop(start, input)) {
                        sum++;
                    }

                    input[y] = old;
                }
            }
        }

        Console.WriteLine(sum);
    }

    private static bool RunLoop(Point start, string[] input) {
        var uPos = new HashSet<Point>();
        var rPos = new HashSet<Point>();
        var dPos = new HashSet<Point>();
        var lPos = new HashSet<Point>();
        var current = start;
        while (true) {
            if (MoveUp(input, current) is not { } up) {
                break;
            }

            if (!uPos.Add(up)) {
                return true;
            }

            if (MoveRight(input, up) is not { } right) {
                break;
            }

            if (!rPos.Add(right)) {
                return true;
            }

            if (MoveDown(input, right) is not { } down) {
                break;
            }

            if (!dPos.Add(down)) {
                return true;
            }

            if (MoveLeft(input, down) is not { } left) {
                break;
            }

            if (!lPos.Add(left)) {
                return true;
            }

            current = left;
        }

        return false;
    }

    private static Point? MoveUp(string[] input, Point current) {
        var last = current;
        for (var i = current.Y - 1; i >= 0; i--) {
            if (input[i][current.X] == '#') {
                return last;
            }

            last = current with { Y = i };
        }

        return null;
    }

    private static Point? MoveDown(string[] input, Point current) {
        var last = current;
        for (var i = current.Y + 1; i < input.Length; i++) {
            if (input[i][current.X] == '#') {
                return last;
            }

            last = current with { Y = i };
        }

        return null;
    }

    private static Point? MoveRight(string[] input, Point current) {
        var last = current;
        for (var i = current.X + 1; i < input[current.Y].Length; i++) {
            if (input[current.Y][i] == '#') {
                return last;
            }

            last = current with { X = i };
        }

        return null;
    }
    private static Point? MoveLeft(string[] input, Point current) {
        var last = current;
        for (var i = current.X - 1; i >= 0; i--) {
            if (input[current.Y][i] == '#') {
                return last;
            }

            last = current with { X = i };
        }

        return null;
    }
}