namespace Day3;

internal static class Program {
    public static void Main(string[] args) {
        var batteries = GetBatteries();

        var part1 = Part1(batteries);
        var part2 = Part2(batteries);

        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
    }

    private static List<int[]> GetBatteries() {
        var batteries = new List<int[]>();

        using var sr = new StreamReader("Inputs.txt");
        while (sr.ReadLine() is { } line) {
            batteries.Add(line.Select(x => int.Parse([x]))
                .ToArray());
        }

        return batteries;
    }

    private static int Part1(List<int[]> batteries) {
        var sum = 0;

        foreach (var battery in batteries) {
            sum += TurnOnBatteriesPart1(battery);
        }

        return sum;
    }

    private static int TurnOnBatteriesPart1(int[] battery) {
        var max = 0;

        for (var i = 0; i < battery.Length - 1; i++) {
            var left = battery[i] * 10;

            for (var j = i + 1; j < battery.Length; j++) {
                var right = battery[j];

                var sum = left + right;
                if (sum > max) {
                    max = sum;
                }
            }
        }

        return max;
    }

    private static long Part2(List<int[]> batteries) {
        var sum = 0L;

        foreach (var battery in batteries) {
            sum += TurnOnBatteriesPart2(battery);
        }

        return sum;
    }

    private static long TurnOnBatteriesPart2(int[] battery) {
        const int DEPTH = 12;

        var mutableBattery = battery.ToArray();

        var num = 0L;
        var taken = 0;
        for (var i = 0; i < DEPTH; i++) {
            var (maxIdx, value) = mutableBattery
                .Index()
                .Skip(taken)
                .SkipLast(DEPTH - 1 - i)
                .MaxBy(x => x.Item);

            num *= 10;
            num += value;
            mutableBattery[maxIdx] = 0;
            taken = Math.Max(taken, maxIdx);
        }

        return num;
    }
}