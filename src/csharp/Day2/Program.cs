namespace Day2;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllLines("Inputs.txt");

        var safe1 = Part1(input);
        var safe2 = Part2(input);

        Console.WriteLine($"Part 1: {safe1}");
        Console.WriteLine($"Part 2: {safe2}");
    }

    private static int Part1(string[] input) {
        var safe = 0;
        foreach (var line in input) {
            var numbers = line.Split(' ').Select(int.Parse).ToArray();

            if (CheckSafety(numbers)) {
                safe++;
            }
        }

        return safe;
    }

    private static int Part2(string[] input) {
        var safe = 0;
        foreach (var line in input) {
            var numbers = line.Split(' ').Select(int.Parse).ToArray();

            if (CheckSafety(numbers)) {
                safe++;
                continue;
            }

            for (var i = 0; i < numbers.Length; i++) {
                var numbersList = numbers.ToList();
                numbersList.RemoveAt(i);
                if (CheckSafety(numbersList.ToArray())) {
                    safe++;
                    break;
                }
            }
        }

        return safe;
    }

    private static bool CheckSafety(int[] numbers) {
        var ascending = numbers[0] < numbers[1];
        for (var i = 0; i < numbers.Length - 1; i++) {
            var a = numbers[i];
            var b = numbers[i + 1];

            if (a == b) {
                return false;
            }

            if (ascending) {
                if (a > b) {
                    return false;
                }
            }
            else {
                if (a < b) {
                    return false;
                }
            }

            if (Math.Abs(a - b) > 3) {
                return false;
            }
        }

        return true;
    }
}