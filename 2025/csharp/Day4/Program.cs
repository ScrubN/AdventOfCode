namespace Day4;

internal static class Program {
    public static void Main(string[] args) {
        var data = GetData();

        var part1 = Part1(data);
        // var part2 = CountZeroClicks(data);

        Console.WriteLine($"Part 1: {part1}");
        // Console.WriteLine($"Part 2: {part2}");
    }

    private static List<> GetData() {
        var data = new List<Turn>();

        using var sr = new StreamReader("Inputs.txt");
        while (sr.ReadLine() is { } line) {
            data.Add();
        }

        return data;
    }

    private static int Part1(object data) {
        throw new NotImplementedException();
    }
}