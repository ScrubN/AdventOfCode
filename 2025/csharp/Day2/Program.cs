using System.Diagnostics;

namespace Day2;

internal static class Program {
    public readonly record struct Range(long Start, long End);

    public static void Main(string[] args) {
        var data = GetIdRanges();

        var part1 = SumInvalidIdsPart1(data);
        var part2 = SumInvalidIdsPart2(data);

        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
    }

    private static List<Range> GetIdRanges() {
        var input = File.ReadAllText("Inputs.txt")
            .Split(',');

        var idRanges = new List<Range>();
        foreach (var str in input) {
            var separator = str.IndexOf('-');

            var startId = long.Parse(str.AsSpan(0, separator));
            var endId = long.Parse(str.AsSpan(separator + 1));

            idRanges.Add(new Range(startId, endId));
        }

        return idRanges;
    }

    private static long SumInvalidIdsPart1(List<Range> data) {
        var sum = 0L;

        foreach (var range in data) {
            for (var i = range.Start; i <= range.End; i++) {
                var digits = CountDigits(i);

                if (IsInvalidIdPart1(digits, i)) {
                    sum += i;
                }
            }
        }

        return sum;
    }

    private static bool IsInvalidIdPart1(int digits, long i) {
        // Cannot be repeated sequence if even
        if (digits % 2 != 0)
            return false;

        // ReSharper disable once PossibleLossOfFraction
        var magnitude = (long)Math.Pow(10, digits / 2);
        var upper = i / magnitude;
        var lower = i - upper * magnitude;

        return upper == lower;
    }


    private static long SumInvalidIdsPart2(List<Range> data) {
        var digitFactors = Enumerable.Range(0, 30)
            .Select(Factors)
            .ToArray();

        var sum = 0L;

        foreach (var range in data) {
            for (var i = range.Start; i <= range.End; i++) {
                var digits = CountDigits(i);

                if (!IsInvalidIdPart2(digits, i, digitFactors[digits])) {
                    continue;
                }

                sum += i;
            }
        }

        return sum;
    }

    private static bool IsInvalidIdPart2(int digits, long id, int[] digitFactors) {
        Span<char> str = stackalloc char[digits];
        var success = id.TryFormat(str, out var written);
        Debug.Assert(success);
        Debug.Assert(str.Length == written);

        foreach (var factor in digitFactors) {
            if (HasRepeatingSequence(str, factor)) {
                return true;
            }
        }

        return false;
    }

    private static bool HasRepeatingSequence(ReadOnlySpan<char> str, int divisions) {
        Debug.Assert(str.Length % divisions == 0);

        var divisionLen = str.Length / divisions;

        var pattern = str[..divisionLen];
        for (var i = 1; i < divisions; i++) {
            var start = divisionLen * i;
            var end = start + divisionLen;
            var current = str[start..end];

            if (!pattern.SequenceEqual(current)) {
                return false;
            }
        }

        return true;
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

    private static readonly int[] FactorScratch = new int[30];

    private static int[] Factors(int number) {
        var factors = 0;

        for (var i = 2; i <= number; i++) {
            if (number % i == 0) {
                FactorScratch[factors] = i;
                factors++;
            }
        }

        return FactorScratch.AsSpan(0, factors).ToArray();
    }
}