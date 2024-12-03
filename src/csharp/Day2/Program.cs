namespace Day2;

internal static class Program {
    internal static void Main(string[] args) {
        using var fs = File.OpenRead("Inputs.txt");
        using var sr = new StreamReader(fs);

        var safe = 0;
        while (sr.ReadLine() is { } line) {
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

        Console.WriteLine(safe);
    }

    private static bool CheckSafety(int[] numbers) {
        var ascending = numbers[0] < numbers[1];
        for (var i = 0; i < numbers.Length - 1; i++) {
            var a = numbers[i];
            var b = numbers[i + 1];

            if (a == b) {
                return false;
            }

            if (Math.Abs(a - b) > 3) {
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
        }

        return true;
    }
}