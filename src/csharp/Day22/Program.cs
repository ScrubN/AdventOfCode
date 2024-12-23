namespace Day22;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadLines("Inputs.txt").Select(long.Parse).ToArray();

        var sum1 = Part1(input);

        Console.WriteLine(sum1);
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

    private static long PseudoRandom(long secretNum) {
        var a = Prune(Mix(secretNum, secretNum * 64));
        var b = Prune(Mix(a, a / 32));
        var c = Prune(Mix(b, b * 2048));
        return c;
    }

    private static long Mix(long secretNum, long value) => secretNum ^ value;

    private static long Prune(long secretNum) => secretNum % 16777216;
}