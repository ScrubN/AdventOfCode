namespace Day8;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllLines("Inputs.txt").Select(x => x.ToArray()).ToArray();

        var antiNodes = Enumerable.Range(0, input.Length).Select(x => input[x].ToArray()).ToArray();
        foreach (var line in antiNodes) {
            Array.Fill(line, '.');
        }

        for (var y = 0; y < input.Length; y++) {
            for (var x = 0; x < input[y].Length; x++) {
                if (input[y][x] is '.') {
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

                        // Up
                        var y3 = y - (y2 - y);
                        var x3 = x - (x2 - x);
                        if (y3 >= 0 && x3 >= 0 && y3 < input.Length && x3 < input[y3].Length) {
                            antiNodes[y3][x3] = '#';
                        }

                        // Down
                        y3 = y2 + (y2 - y);
                        x3 = x2 + (x2 - x);
                        if (y3 < 0 || x3 < 0 || y3 >= input.Length || x3 >= input[y3].Length) {
                            continue;
                        }

                        antiNodes[y3][x3] = '#';
                    }
                }
            }
        }

        // foreach (var line in input) {
        //     Console.WriteLine(line);
        // }
        //
        // foreach (var line in antiNodes) {
        //     Console.WriteLine(line);
        // }

        Console.WriteLine(antiNodes.Sum(x => x.Sum(c => Convert.ToInt32(c == '#'))));
    }
}