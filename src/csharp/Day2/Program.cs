namespace Day2;

internal static class Program {
    internal static void Main(string[] args) {
        using var fs = File.OpenRead("Inputs.txt");
        using var sr = new StreamReader(fs);

        var safe = 0;
        while (sr.ReadLine() is { } line)
        {
            var numbers = line.Split(' ').Select(int.Parse).ToArray();

            var increasing = false;
            var decreasing = false;
            for (var i = 0; i < numbers.Length - 1; i++)
            {
                var a = numbers[i];
                var b = numbers[i + 1];

                if (a == b) break;

                if (!increasing && b > a) increasing = true;
                if (!decreasing && b < a) decreasing = true;

                if (increasing && decreasing) break;

                if (Math.Abs(a - b) > 3) break;

                if (i + 1 == numbers.Length - 1)
                {
                    safe++;
                }
            }
        }

        Console.WriteLine(safe);
    }
}
