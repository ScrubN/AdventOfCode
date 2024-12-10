using System.Drawing;

namespace Day10;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllLines("Inputs.txt").Select(x => x.Select(ToInt).ToArray()).ToArray();

        var sum1 = Part1(input);

        Console.WriteLine(sum1);
    }

    private static int ToInt(char arg) {
        if (arg is >= '0' and <= '9') {
            return arg - '0';
        }

        throw new ArgumentException(null, nameof(arg));
    }

    private static int Part1(int[][] input) {
        var sum = 0;
        for (var y = 0; y < input.Length; y++) {
            for (var x = 0; x < input[y].Length; x++) {
                if (input[y][x] != 0) {
                    continue;
                }

                sum += ExploreNext(0, input, x, y);
            }
        }

        return sum;
    }

    private static int ExploreNext(int i, int[][] input, int x, int y, HashSet<Point>? taken = null) {
        taken ??= [];

        if (i == 9) {
            return Convert.ToInt32(taken.Add(new Point(x, y)));
        }

        var total = 0;
        if (y - 1 >= 0) {
            if (input[y - 1][x] == i + 1) {
                total += ExploreNext(i + 1, input, x, y - 1, taken);
            }
        }

        if (y + 1 < input.Length) {
            if (input[y + 1][x] == i + 1) {
                total += ExploreNext(i + 1, input, x, y + 1, taken);
            }
        }

        if (x - 1 >= 0) {
            if (input[y][x - 1] == i + 1) {
                total += ExploreNext(i + 1, input, x - 1, y, taken);
            }
        }

        if (x + 1 < input[y].Length) {
            if (input[y][x + 1] == i + 1) {
                total += ExploreNext(i + 1, input, x + 1, y, taken);
            }
        }

        return total;
    }
}