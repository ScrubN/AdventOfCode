using System.Runtime.InteropServices;

namespace Day22;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadLines("Inputs.txt").Select(long.Parse).ToArray();

        var sum1 = Part1(input);
        var sum2 = Part2(input);

        Console.WriteLine($"Part 1: {sum1}");
        Console.WriteLine($"Part 2: {sum2}");
    }

    private static long Part1(long[] input) {
        var sum = 0L;

        foreach (var l in input) {
            var l2 = l;
            for (var i = 0; i < 2_000; i++) {
                l2 = PseudoRandom(l2);
            }

            sum += l2;
        }

        return sum;
    }

    private static int Part2(long[] input) {
        Dictionary<(int, int, int, int), int> sequences = [];

        foreach (var l in input) {
            var a = 0;
            var b = 0;
            var c = 0;
            var d = 0;
            Dictionary<(int, int, int, int), int> sequences2 = [];

            var l2 = l;
            for (var i = 0; i < 2_000; i++) {
                var l3 = PseudoRandom(l2);

                var bananas = (int)(l3 % 10);
                var oldBananas = (int)(l2 % 10);
                var diff = bananas - oldBananas;

                a = b;
                b = c;
                c = d;
                d = diff;
                l2 = l3;

                if (i < 3) {
                    continue;
                }

                var key = (a, b, c, d);
                ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(sequences2, key, out var exists);
                if (!exists) {
                    value = bananas;
                }
            }

            foreach (var (sequence, bananas) in sequences2) {
                ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(sequences, sequence, out _);
                value += bananas;
            }
        }

        return sequences.MaxBy(x => x.Value).Value;
    }

    private static long PseudoRandom(long secretNum) {
        var a = Prune(Mix(secretNum, secretNum * 64));
        var b = Prune(Mix(a, a / 32));
        var c = Prune(Mix(b, b * 2048));
        return c;
    }

    private static long Mix(long secretNum, long value) => secretNum ^ value;

    private static long Prune(long secretNum) => secretNum % 16777216;
}