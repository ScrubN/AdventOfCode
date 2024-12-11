namespace Day11;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllText("Inputs.txt").Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();

        var sum1 = Part1(input);

        Console.WriteLine(sum1);
    }

    private static long Part1(long[] input) {
        var stones = input.ToList();
        for (var i = 0; i < 25; i++) {
            stones = FindNewStones1(stones);
        }

        return stones.Count;
    }

    private static List<long> FindNewStones1(IEnumerable<long> stones) {
        List<long> newStones = [];
        foreach (var stone in stones) {
            if (stone == 0) {
                newStones.Add(1);
                continue;
            }

            var digits = CountDigits(stone);
            if (digits % 2 == 0) {
                var a = stone;
                for (var i = 0; i < digits / 2; i++) {
                    a /= 10;
                }

                newStones.Add(a);

                for (var i = 0; i < digits / 2; i++) {
                    a *= 10;
                }

                var b = stone - a;
                newStones.Add(b);
                continue;
            }

            newStones.Add(stone * 2024);
        }

        return newStones;
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