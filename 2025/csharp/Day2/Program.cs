using System.Diagnostics;

namespace Day2;

internal static class Program {
    public readonly record struct Range(long Start, long End);

    public static void Main(string[] args) {
        var data = GetIdRanges();

        var part1 = SumInvalidIds(data);

        Console.WriteLine($"Part 1: {part1}");
        // Console.WriteLine($"Part 2: {part2}");
    }

    private static List<Range> GetIdRanges() {
        var input = File.ReadAllText("Inputs.txt")
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var idRanges = new List<Range>();
        foreach (var str in input) {
            var separator = str.IndexOf('-');

            var startId = long.Parse(str.AsSpan(0, separator));
            var endId = long.Parse(str.AsSpan(separator + 1));

            idRanges.Add(new Range(startId, endId));
        }

        return idRanges;
    }

    private static long SumInvalidIds(List<Range> data) {
        var invalidIds = new List<long>();

        foreach (var range in data) {
            for (var i = range.Start; i <= range.End; i++) {
                var digits = CountDigits(i);

                // Cannot be repeated sequence if even
                if (digits % 2 != 0)
                    continue;

                // ReSharper disable once PossibleLossOfFraction
                var magnitude = (long)Math.Pow(10, digits / 2);
                var upper = i / magnitude;
                var lower = i - upper * magnitude;

                if (upper == lower) {
                    invalidIds.Add(i);
                }
            }
        }

        return invalidIds.Sum();
    }

    private static int CountDigits(long number) {
        Debug.Assert(number > 0);

        // Not supporting 0 is faster
        var digits = 0;
        while (number > 0) {
            digits++;
            number /= 10;
        }

        return digits;
    }
}