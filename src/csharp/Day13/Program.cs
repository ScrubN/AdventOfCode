using System.Drawing;
using System.Text.RegularExpressions;

namespace Day13;

internal static partial class Program {
    [GeneratedRegex(@"^Button [AB]: X\+(\d+), Y\+(\d+)$")]
    private static partial Regex ButtonRegex { get; }

    [GeneratedRegex(@"^Prize: X=(\d+), Y=(\d+)$")]
    private static partial Regex PrizeRegex { get; }

    private record struct LongPoint(Int128 X, Int128 Y) {
        public static implicit operator LongPoint(Point p) => new(p.X, p.Y);
        public static LongPoint operator +(LongPoint a, ulong b) => new(a.X + b, a.Y + b);
    }

    internal static void Main(string[] args) {
        var machines = GetMachines();

        var res1 = Part1(machines);
        var res2 = Part2(machines);

        Console.WriteLine(res1);
        Console.WriteLine(res2);
    }

    private static List<(Point A, Point B, Point Prize)> GetMachines() {
        List<(Point A, Point B, Point Prize)> machine = [];

        Point a = default;
        Point b = default;
        Point prize = default;
        using var sr = new StreamReader("Inputs.txt");
        while (sr.ReadLine() is { } line) {
            if (string.IsNullOrWhiteSpace(line)) {
                machine.Add((a, b, prize));
                a = default;
                b = default;
                prize = default;
                continue;
            }

            if (line.StartsWith("Button A:")) {
                a = ParseButton(line);
            }
            else if (line.StartsWith("Button B:")) {
                b = ParseButton(line);
            }
            else if (line.StartsWith("Prize:")) {
                prize = ParsePrize(line);
            }
        }

        if (a != default && b != default && prize != default) {
            machine.Add((a, b, prize));
        }

        return machine;
    }

    private static Point ParseButton(string str) {
        var match = ButtonRegex.Match(str);
        if (!match.Success) {
            throw new FormatException();
        }

        return new Point(int.Parse(match.Groups[1].ValueSpan), int.Parse(match.Groups[2].ValueSpan));
    }

    private static Point ParsePrize(string str) {
        var match = PrizeRegex.Match(str);
        if (!match.Success) {
            throw new FormatException();
        }

        return new Point(int.Parse(match.Groups[1].ValueSpan), int.Parse(match.Groups[2].ValueSpan));
    }

    private static long Part1(IEnumerable<(Point A, Point B, Point Prize)> input) {
        var res = 0;

        foreach (var machine in input) {
            if (SolveMachine1(machine, out var tokens)) {
                res += tokens;
            }
        }

        return res;
    }

    private static bool SolveMachine1((Point A, Point B, Point Prize) machine, out int tokens) {
        var aX = machine.A.X;
        var aY = machine.A.Y;

        var bX = machine.B.X;
        var bY = machine.B.Y;

        var pX = machine.Prize.X;
        var pY = machine.Prize.Y;

        // | aX bX |
        // | aY bY |
        var det = (aX * bY) - (aY * bX);
        if (det == 0) {
            tokens = 0;
            return false;
        }

        // | pX bX |
        // | pY bY |
        var sA = ((pX * bY) - (pY * bX)) / (double)det;

        // | aX pX |
        // | aY pY |
        var sB = ((aX * pY) - (aY * pX)) / (double)det;

        if (sA is < 0 or > 100 || !IsWholeNumber(sA) || sB is < 0 or > 100 || !IsWholeNumber(sB)) {
            tokens = 0;
            return false;
        }

        tokens = RoundToInt(sA) * 3 + RoundToInt(sB);
        return true;
    }

    private static ulong Part2(IEnumerable<(Point A, Point B, Point Prize)> input) {
        var res = 0UL;

        foreach (var machine in input.Select(x => (x.A, x.B, (LongPoint)x.Prize + 10000000000000))) {
            if (SolveMachine2(machine, out var tokens)) {
                res += tokens;
            }
        }

        return res;
    }

    private static bool SolveMachine2((LongPoint A, LongPoint B, LongPoint Prize) machine, out ulong tokens) {
        var aX = machine.A.X;
        var aY = machine.A.Y;

        var bX = machine.B.X;
        var bY = machine.B.Y;

        var pX = machine.Prize.X;
        var pY = machine.Prize.Y;

        // | aX bX |
        // | aY bY |
        var det = (aX * bY) - (aY * bX);
        if (det == 0) {
            tokens = 0;
            return false;
        }

        // | pX bX |
        // | pY bY |
        var sA = (double)((pX * bY) - (pY * bX)) / (double)det;

        // | aX pX |
        // | aY pY |
        var sB = (double)((aX * pY) - (aY * pX)) / (double)det;

        if (!IsWholeNumber(sA) || !IsWholeNumber(sB)) {
            tokens = 0;
            return false;
        }

        tokens = RoundToULong(sA) * 3 + RoundToULong(sB);
        return true;
    }

    private static bool IsWholeNumber(double value) => Math.Abs(value - Math.Round(value)) < 0.0001;

    private static int RoundToInt(double value) => (int)Math.Round(value);

    private static ulong RoundToULong(double value) => (ulong)Math.Round(value);
}