namespace Day8;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllLines("Inputs.txt");

        var antiNodes1 = InitAntiNodes(input);
        var antiNodes2 = InitAntiNodes(input);

        Part1(input, antiNodes1);
        Part2(input, antiNodes2);

        var sum1 = CountAntiNodes(antiNodes1);
        var sum2 = CountAntiNodes(antiNodes2);

        Console.WriteLine($"Part 1: {sum1}");
        Console.WriteLine($"Part 2: {sum2}");
    }

    private static char[][] InitAntiNodes(string[] input) {
        var antiNodes = Enumerable.Range(0, input.Length).Select(x => input[x].ToArray()).ToArray();
        foreach (var line in antiNodes) {
            Array.Fill(line, '.');
        }

        return antiNodes;
    }

    private static void Part1(string[] input, char[][] antiNodes) {
        for (var y = 0; y < input.Length; y++) {
            for (var x = 0; x < input[y].Length; x++) {
                if (input[y][x] == '.') {
                    continue;
                }

                var type = input[y][x];
                for (var y2 = y; y2 < input.Length; y2++) {
                    for (var x2 = 0; x2 < input[y2].Length; x2++) {
                        if (y2 == y && x2 == x) {
                            continue;
                        }

                        if (input[y2][x2] != type) {
                            continue;
                        }

                        var diffY = y2 - y;
                        var diffX = x2 - x;

                        // Up
                        var y3 = y - diffY;
                        var x3 = x - diffX;
                        if (y3 >= 0 && x3 >= 0 && y3 < input.Length && x3 < input[y3].Length) {
                            antiNodes[y3][x3] = '#';
                        }

                        // Down
                        y3 = y2 + diffY;
                        x3 = x2 + diffX;
                        if (y3 >= 0 && x3 >= 0 && y3 < input.Length && x3 < input[y3].Length) {
                            antiNodes[y3][x3] = '#';
                        }
                    }
                }
            }
        }
    }

    private static void Part2(string[] input, char[][] antiNodes) {
        for (var y = 0; y < input.Length; y++) {
            for (var x = 0; x < input[y].Length; x++) {
                if (input[y][x] == '.') {
                    continue;
                }

                var type = input[y][x];
                for (var y2 = y; y2 < input.Length; y2++) {
                    for (var x2 = 0; x2 < input[y2].Length; x2++) {
                        if (y2 == y && x2 == x) {
                            continue;
                        }

                        if (input[y2][x2] != type) {
                            continue;
                        }

                        var diffY = y2 - y;
                        var diffX = x2 - x;

                        // Up
                        var y3 = y;
                        var x3 = x;
                        while (y3 >= 0 && x3 >= 0 &&
                               y3 < input.Length && x3 < input[y3].Length) {
                            antiNodes[y3][x3] = '#';
                            y3 -= diffY;
                            x3 -= diffX;
                        }

                        // Down
                        y3 = y2;
                        x3 = x2;
                        while (y3 >= 0 && x3 >= 0 &&
                               y3 < input.Length && x3 < input[y3].Length) {
                            antiNodes[y3][x3] = '#';
                            y3 += diffY;
                            x3 += diffX;
                        }
                    }
                }
            }
        }
    }

    private static int CountAntiNodes(char[][] grid) {
        return grid.Sum(x => x.Sum(c => Convert.ToInt32(c == '#')));
    }
}