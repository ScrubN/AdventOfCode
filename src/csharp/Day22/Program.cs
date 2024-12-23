using System.Runtime.InteropServices;

namespace Day22;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadLines("Inputs.txt").Select(long.Parse).ToArray();

        // var sum1 = Part1(input);
        // var sum2 = Part2(input);
        var (sum1, sum2) = Part1And2(input);

        Console.WriteLine($"Part 1: {sum1}");
        Console.WriteLine($"Part 2: {sum2}");
    }

    private static long Part1(long[] input) {
        var sum = 0L;

        foreach (var l in input) {
            var l2 = l;
            for (var i = 0; i < 2_000; i++) {
                l2 = PseudoRandomFast(l2);
            }

            sum += l2;
        }

        return sum;
    }

    private static int Part2(long[] input) {
        var sequences = new Dictionary<(int, int, int, int), int>();
        var sequenceLock = new Lock();

        Parallel.ForEach(input, l => {
            var a = 0;
            var b = 0;
            var c = 0;
            var d = 0;
            var sequences2 = new Dictionary<(int, int, int, int), int>();

            var l2 = l;
            for (var i = 0; i < 2_000; i++) {
                var l3 = PseudoRandomFast(l2);

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

            lock (sequenceLock) {
                foreach (var (sequence, bananas) in sequences2) {
                    CollectionsMarshal.GetValueRefOrAddDefault(sequences, sequence, out _) += bananas;
                }
            }
        });

        return sequences.Values.Max();
    }

    private static (long sum1, int sum2) Part1And2(long[] input) {
        var sequences = new Dictionary<int, int>();
        var sequenceLock = new Lock();
        var sum1 = 0L;

        Parallel.ForEach(input, l => {
            var a = 0;
            var b = 0;
            var c = 0;
            var d = 0;
            var sequences2 = new Dictionary<int, int>();

            var l2 = l;
            for (var i = 0; i < 2_000; i++) {
                var l3 = PseudoRandomFast(l2);

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

                var key = HashCode.Combine(a, b, c, d);
                ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(sequences2, key, out var exists);
                if (!exists) {
                    value = bananas;
                }
            }

            Interlocked.Add(ref sum1, l2);

            lock (sequenceLock) {
                foreach (var (sequence, bananas) in sequences2) {
                    CollectionsMarshal.GetValueRefOrAddDefault(sequences, sequence, out _) += bananas;
                }
            }
        });

        return (sum1, sequences.Values.Max());
    }

    private static long PseudoRandom(long secretNum) {
        var a = Prune(Mix(secretNum, secretNum * 64));
        var b = Prune(Mix(a, a / 32));
        var c = Prune(Mix(b, b * 2048));

        return c;

        static long Mix(long secretNum, long value) {
            return secretNum ^ value;
        }

        static long Prune(long secretNum) {
            return secretNum % 16777216;
        }
    }

    private static long PseudoRandomFast(long secretNum) {
        var a = (secretNum ^ (secretNum * 64)) % 16777216;
        var b = (a ^ (a / 32)) % 16777216;
        return (b ^ (b * 2048)) % 16777216;
    }
}