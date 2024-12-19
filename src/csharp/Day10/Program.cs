using System.Diagnostics;
using System.Drawing;

namespace Day10;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadLines("Inputs.txt").Select(x => x.Select(y => y - '0').ToArray()).ToArray();
        Debug.Assert(input.All(x => x.All(y => y is >= 0 and <= 9)));

        // var sum1 = Part1(input);
        // var sum2 = Part2(input);
        var (sum1, sum2) = Part1And2(input);
        
        Console.WriteLine($"Part 1: {sum1}");
        Console.WriteLine($"Part 2: {sum2}");
    }

    private static int Part1(int[][] input) {
        var sum = 0;
        for (var y = 0; y < input.Length; y++) {
            for (var x = 0; x < input[y].Length; x++) {
                if (input[y][x] != 0) {
                    continue;
                }

                sum += ExploreNext1(0, input, x, y);
            }
        }

        return sum;
    }

    private static int ExploreNext1(int num, int[][] input, int x, int y, HashSet<Point>? taken = null) {
        taken ??= [];

        if (num == 9) {
            return Convert.ToInt32(taken.Add(new Point(x, y)));
        }

        var nextNum = num + 1;
        var total = 0;
        if (y - 1 >= 0 && input[y - 1][x] == nextNum) {
            total += ExploreNext1(nextNum, input, x, y - 1, taken);
        }

        if (y + 1 < input.Length && input[y + 1][x] == nextNum) {
            total += ExploreNext1(nextNum, input, x, y + 1, taken);
        }

        if (x - 1 >= 0 && input[y][x - 1] == nextNum) {
            total += ExploreNext1(nextNum, input, x - 1, y, taken);
        }

        if (x + 1 < input[y].Length && input[y][x + 1] == nextNum) {
            total += ExploreNext1(nextNum, input, x + 1, y, taken);
        }

        return total;
    }

    private static int Part2(int[][] input) {
        var sum = 0;
        for (var y = 0; y < input.Length; y++) {
            for (var x = 0; x < input[y].Length; x++) {
                if (input[y][x] != 0) {
                    continue;
                }

                sum += ExploreNext2(0, input, x, y);
            }
        }

        return sum;
    }

    private static int ExploreNext2(int num, int[][] input, int x, int y) {
        if (num == 9) {
            return 1;
        }

        var nextNum = num + 1;
        var total = 0;
        if (y - 1 >= 0 && input[y - 1][x] == nextNum) {
            total += ExploreNext2(nextNum, input, x, y - 1);
        }

        if (y + 1 < input.Length && input[y + 1][x] == nextNum) {
            total += ExploreNext2(nextNum, input, x, y + 1);
        }

        if (x - 1 >= 0 && input[y][x - 1] == nextNum) {
            total += ExploreNext2(nextNum, input, x - 1, y);
        }

        if (x + 1 < input[y].Length && input[y][x + 1] == nextNum) {
            total += ExploreNext2(nextNum, input, x + 1, y);
        }

        return total;
    }

    private static (int sum1, int sum2) Part1And2(int[][] input) {
        var sum1 = 0;
        var sum2 = 0;
        for (var y = 0; y < input.Length; y++) {
            for (var x = 0; x < input[y].Length; x++) {
                if (input[y][x] != 0) {
                    continue;
                }

                var (a, b) = ExploreNext1And2(0, input, x, y, []);
                sum1 += a;
                sum2 += b;
            }
        }

        return (sum1, sum2);
    }

    private static (int sum1, int sum2) ExploreNext1And2(int num, int[][] input, int x, int y, HashSet<Point> taken) {
        if (num == 9) {
            return (Convert.ToInt32(taken.Add(new Point(x, y))), 1);
        }

        var nextNum = num + 1;
        var sum1 = 0;
        var sum2 = 0;
        if (y - 1 >= 0 && input[y - 1][x] == nextNum) {
            var (a, b) = ExploreNext1And2(nextNum, input, x, y - 1, taken);
            sum1 += a;
            sum2 += b;
        }

        if (y + 1 < input.Length && input[y + 1][x] == nextNum) {
            var (a, b) = ExploreNext1And2(nextNum, input, x, y + 1, taken);
            sum1 += a;
            sum2 += b;
        }

        if (x - 1 >= 0 && input[y][x - 1] == nextNum) {
            var (a, b) = ExploreNext1And2(nextNum, input, x - 1, y, taken);
            sum1 += a;
            sum2 += b;
        }

        if (x + 1 < input[y].Length && input[y][x + 1] == nextNum) {
            var (a, b) = ExploreNext1And2(nextNum, input, x + 1, y, taken);
            sum1 += a;
            sum2 += b;
        }

        return (sum1, sum2);
    }
}