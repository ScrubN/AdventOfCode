namespace Day7;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllLines("Inputs.txt");

        var sum = 0L;
        foreach (var line in input) {
            sum += Compute(line);
        }

        Console.WriteLine(sum);
    }

    private static long Compute(string line) {
        var expected = long.Parse(line.AsSpan(0, line.IndexOf(':')));
        var parameters = line[(line.IndexOf(':') + 1)..]
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToArray();

        for (var mask = 0L; mask < Math.Pow(2, parameters.Length); mask++) {
            var workingMask = mask;
            var runningTotal = parameters[0];
            foreach (var param in parameters.Skip(1)) {
                var mult = (workingMask & 1) == 1;
                if (mult) {
                    runningTotal *= param;
                }
                else {
                    runningTotal += param;
                }

                workingMask >>= 1;
            }

            if (runningTotal == expected) {
                return expected;
            }
        }

        return 0;
    }
}