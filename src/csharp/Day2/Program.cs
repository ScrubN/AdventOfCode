namespace Day2;

internal static class Program {
    internal static void Main(string[] args) {
        using var fs = File.OpenRead("Inputs.txt");
        using var sr = new StreamReader(fs);

        var safe = 0;
        while (sr.ReadLine() is { } line) {
            var numbers = line.Split(' ').Select(int.Parse).ToArray();

            if (CheckSafety(numbers, ref safe)) {
                continue;
            }

            for (var i = 0; i < numbers.Length; i++) {
                var numbersList = numbers.ToList();
                numbersList.RemoveAt(i);
                if (CheckSafety(numbersList.ToArray(), ref safe)) {
                    break;
                }
            }
        }

        Console.WriteLine(safe);
    }

    private static bool CheckSafety(int[] numbers, ref int safe) {
        var increasing = false;
        var decreasing = false;
        for (var i = 0; i < numbers.Length - 1; i++) {
            if (!CheckSafetyInner(numbers[i], numbers[i + 1], ref increasing, ref decreasing)) {
                return false;
            }

            if (i + 1 == numbers.Length - 1) {
                safe++;
                return true;
            }
        }

        return false;
    }

    private static bool CheckSafetyInner(int a, int b, ref bool increasing, ref bool decreasing) {
        if (a == b) {
            return false;
        }

        if (Math.Abs(a - b) > 3) {
            return false;
        }

        var localIncreasing = increasing;
        if (!increasing && b > a) {
            localIncreasing = true;
        }

        var localDecreasing = decreasing;
        if (!decreasing && b < a) {
            localDecreasing = true;
        }

        if (localIncreasing && localDecreasing) {
            return false;
        }

        increasing = localIncreasing;
        decreasing = localDecreasing;
        return true;
    }
}