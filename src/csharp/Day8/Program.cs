namespace Day8;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllLines("Inputs.txt");

        // Inserting into boolean arrays and counting the amount of trues is faster than using a HashSet<Point>
        var antiNodes1 = new bool[input.Length, input[0].Length];
        var antiNodes2 = new bool[input.Length, input[0].Length];

        // Part1(input, antiNodes1);
        // Part2(input, antiNodes2);
        Part1And2(input, antiNodes1, antiNodes2);

        var sum1 = CountAntiNodes(antiNodes1);
        var sum2 = CountAntiNodes(antiNodes2);

        Console.WriteLine($"Part 1: {sum1}");
        Console.WriteLine($"Part 2: {sum2}");
    }

    private static void Part1(string[] input, bool[,] antiNodes) {
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
                            antiNodes[x3, y3] = true;
                        }

                        // Down
                        y3 = y2 + diffY;
                        x3 = x2 + diffX;
                        if (y3 >= 0 && x3 >= 0 && y3 < input.Length && x3 < input[y3].Length) {
                            antiNodes[x3, y3] = true;
                        }
                    }
                }
            }
        }
    }

    private static void Part2(string[] input, bool[,] antiNodes) {
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
                        while (y3 >= 0 && x3 >= 0 && y3 < input.Length && x3 < input[y3].Length) {
                            antiNodes[x3, y3] = true;
                            y3 -= diffY;
                            x3 -= diffX;
                        }

                        // Down
                        y3 = y2;
                        x3 = x2;
                        while (y3 >= 0 && x3 >= 0 && y3 < input.Length && x3 < input[y3].Length) {
                            antiNodes[x3, y3] = true;
                            y3 += diffY;
                            x3 += diffX;
                        }
                    }
                }
            }
        }
    }

    private static void Part1And2(string[] input, bool[,] antiNodes1, bool[,] antiNodes2) {
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

                        // Up part 1
                        var y3 = y - diffY;
                        var x3 = x - diffX;
                        if (y3 >= 0 && x3 >= 0 && y3 < input.Length && x3 < input[y3].Length) {
                            antiNodes1[x3, y3] = true;
                        }

                        // Up part 1
                        y3 = y;
                        x3 = x;
                        while (y3 >= 0 && x3 >= 0 && y3 < input.Length && x3 < input[y3].Length) {
                            antiNodes2[x3, y3] = true;
                            y3 -= diffY;
                            x3 -= diffX;
                        }

                        // Down part 1
                        y3 = y2 + diffY;
                        x3 = x2 + diffX;
                        if (y3 >= 0 && x3 >= 0 && y3 < input.Length && x3 < input[y3].Length) {
                            antiNodes1[x3, y3] = true;
                        }

                        // Down part 2
                        y3 = y2;
                        x3 = x2;
                        while (y3 >= 0 && x3 >= 0 && y3 < input.Length && x3 < input[y3].Length) {
                            do {
                                antiNodes2[x3, y3] = true;
                                y3 += diffY;
                                x3 += diffX;
                            } while (y3 >= 0 && x3 >= 0 && y3 < input.Length && x3 < input[y3].Length);
                        }
                    }
                }
            }
        }
    }

    private static int CountAntiNodes(bool[,] grid) {
        var count = 0;
        var length0 = grid.GetLength(0);
        var length1 = grid.GetLength(1);
        for (var x = 0; x < length0; x++) {
            for (var y = 0; y < length1; y++) {
                if (grid[x, y]) {
                    count++;
                }
            }
        }

        return count;
    }
}