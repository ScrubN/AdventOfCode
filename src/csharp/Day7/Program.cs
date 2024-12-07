namespace Day7;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllLines("Inputs.txt");

        var sum = input.Sum(Compute);

        Console.WriteLine(sum);
    }

    private static long Compute(string line) {
        var expected = long.Parse(line.AsSpan(0, line.IndexOf(':')));
        var parameters = line[(line.IndexOf(':') + 1)..]
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToArray();

        for (var mask = 0L; mask < Math.Pow(3, parameters.Length); mask++) {
            var runningTotal = parameters[0];
            var workingMask = mask;
            for (var i = 1; i < parameters.Length; i++) {
                var param = parameters[i];
                var op = workingMask % 3;
                switch (op) {
                    case 0:
                        runningTotal += param;
                        break;
                    case 1:
                        runningTotal *= param;
                        break;
                    case 2:
                        runningTotal = runningTotal * (long)Math.Pow(10, CountDigits(param)) + param;
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

    private static int CountDigits(long number) {
        var digits = 0;
        while (number > 0) {
            digits++;
            number /= 10;
        }

        return digits;
    }
}