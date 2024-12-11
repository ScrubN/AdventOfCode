using System.Runtime.InteropServices;

namespace Day11;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllText("Inputs.txt").Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();

        var sum1 = Part1(input);
        var sum2 = Part2(input);

        Console.WriteLine($"Part 1: {sum1}");
        Console.WriteLine($"Part 2: {sum2}");
    }

    private static long Part1(long[] input) {
        return ObserveStones(input, 25);
    }

    private static long Part2(long[] input) {
        return ObserveStones(input, 75);
    }

    private static long ObserveStones(long[] input, long iterations) {
        Dictionary<long, long> stones = [];
        foreach (var stone in input) {
            stones.GetValueRefOrAddDefault(stone)++;
        }

        for (var i = 0; i < iterations; i++) {
            Dictionary<long, long> iStones = [];
            foreach (var (value, count) in stones) {
                if (value == 0) {
                    iStones.GetValueRefOrAddDefault(1) += count;
                    continue;
                }

                var digits = CountDigits(value);
                if (digits % 2 == 0) {
                    var d = 1;
                    for (var j = 0; j < digits / 2; j++) {
                        d *= 10;
                    }

                    iStones.GetValueRefOrAddDefault(value / d) += count;
                    iStones.GetValueRefOrAddDefault(value % d) += count;
                    continue;
                }

                iStones.GetValueRefOrAddDefault(value * 2024) += count;
            }

            stones = iStones;
        }

        return stones.Sum(x => x.Value);
    }

    private static ref TValue? GetValueRefOrAddDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) where TKey : notnull {
        return ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out _);
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