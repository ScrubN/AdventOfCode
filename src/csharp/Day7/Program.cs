namespace Day7;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllLines("Inputs.txt");

        var sum1 = 0L;
        foreach (var line in input) {
            sum1 += Part1(line);
        }

        var sum2 = 0L;
        foreach (var line in input) {
            sum2 += Part2(line);
        }

        Console.WriteLine($"Part 1: {sum1}");
        Console.WriteLine($"Part 2: {sum2}");
    }

    private static long Part1(string line) {
        var (expected, parameters) = ParseLine(line);

        var maskMax = Math.Pow(2, parameters.Length); // Removing from the loop prevents it from being recomputed every iteration
        for (var mask = 0L; mask < maskMax; mask++) {
            var runningTotal = parameters[0];

            var workingMask = mask;
            for (var i = 1; i < parameters.Length; i++) {
                var param = parameters[i];

                if ((workingMask & 1) == 1) {
                    runningTotal *= param;
                }
                else {
                    runningTotal += param;
                }

                // Strangely, this makes it 5-10% slower
                // if (runningTotal > expected) {
                //     break;
                // }

                workingMask >>= 1;
            }

            if (runningTotal == expected) {
                return expected;
            }
        }

        return 0;
    }

    private static long Part2(string line) {
        var (expected, parameters) = ParseLine(line);

        var maskMax = Math.Pow(3, parameters.Length); // Removing from the loop prevents it from being recomputed every iteration
        for (var mask = 0L; mask < maskMax; mask++) {
            var runningTotal = parameters[0];

            var workingMask = mask;
            for (var i = 1; i < parameters.Length; i++) {
                var param = parameters[i];

                switch (workingMask % 3) {
                    case 0:
                        runningTotal += param;
                        break;
                    case 1:
                        runningTotal *= param;
                        break;
                    case 2:
                        // For loop is faster than Math.Pow
                        var digits = CountDigits(param); // Removing from the loop prevents it from being recomputed every iteration
                        for (var j = 0; j < digits; j++) {
                            runningTotal *= 10;
                        }

                        runningTotal += param;
                        break;
                }

                if (runningTotal > expected) {
                    break;
                }

                workingMask /= 3;
            }

            if (runningTotal == expected) {
                return expected;
            }

            mask++;
        }

        return 0;
    }

    private static (long, long[]) ParseLine(string line) {
        var colonIndex = line.IndexOf(':');
        var expected = long.Parse(line.AsSpan(0, colonIndex));
        var parameters = line[(colonIndex + 1)..]
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToArray();

        return (expected, parameters);
    }

    private static int CountDigits(long number) {
        var digits = 0;
        while (number > 0) {
            digits++;
            number /= 10;
        }

        return digits;
    }
}