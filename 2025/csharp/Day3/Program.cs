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
            batteries.Add(
                line
                    .Select(x => int.Parse([x]))
                    .ToArray()
            );
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
        var (idx, left) = battery
            .Index()
            .SkipLast(1)
            .MaxBy(x => x.Item);

        var right = battery
            .Skip(idx + 1)
            .Max();

        return left * 10 + right;
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

        var num = 0L;
        var start = 0;
        for (var i = 0; i < DEPTH; i++) {
            var (idx, value) = battery
                .Index()
                .Skip(start)
                .SkipLast(DEPTH - 1 - i)
                .MaxBy(x => x.Item);

            num *= 10;
            num += value;
            battery[idx] = 0;
            start = idx;
        }

        return num;
    }
}