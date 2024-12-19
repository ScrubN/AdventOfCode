using System.Drawing;
using System.Text.RegularExpressions;

namespace Day13;

internal static partial class Program {
    [GeneratedRegex(@"^Button [AB]: X\+(\d+), Y\+(\d+)$")]
    private static partial Regex ButtonRegex { get; }

    [GeneratedRegex(@"^Prize: X=(\d+), Y=(\d+)$")]
    private static partial Regex PrizeRegex { get; }

    private record struct ClawMachine(Point A, Point B, Point Prize);

    private record struct LongPoint(long X, long Y) {
        public static implicit operator LongPoint(Point p) => new(p.X, p.Y);

        public static LongPoint operator +(LongPoint a, long b) => new(a.X + b, a.Y + b);
    }

    private record struct ClawMachine2(Point A, Point B, LongPoint Prize);

    internal static void Main(string[] args) {
        var machines = GetMachines();

        var res1 = Part1(machines);
        var res2 = Part2(machines);

        Console.WriteLine(res1);
        Console.WriteLine(res2);
    }

    private static List<ClawMachine> GetMachines() {
        List<ClawMachine> machine = [];

        Point a = default;
        Point b = default;
        Point prize = default;
        using var sr = new StreamReader("Inputs.txt");
        while (sr.ReadLine() is { } line) {
            if (string.IsNullOrWhiteSpace(line)) {
                machine.Add(new ClawMachine(a, b, prize));
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
            machine.Add(new ClawMachine(a, b, prize));
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

    private static long Part1(IEnumerable<ClawMachine> input) {
        var res = 0;

        foreach (var machine in input) {
            if (SolveMachine1(machine, out var tokens)) {
                res += tokens;
            }
        }

        return res;
    }

    private static bool SolveMachine1(ClawMachine machine, out int tokens) {
        if (!CramersRule1(machine.A, machine.B, machine.Prize, out var aPresses, out var bPresses)) {
            tokens = 0;
            return false;
        }

        if (aPresses is < 0 or > 100 || !IsWholeNumber(aPresses) || bPresses is < 0 or > 100 || !IsWholeNumber(bPresses)) {
            tokens = 0;
            return false;
        }

        tokens = RoundToInt(aPresses) * 3 + RoundToInt(bPresses);
        return true;
    }

    // ReSharper disable once IdentifierTypo
    private static bool CramersRule1(Point a, Point b, Point p, out double sA, out double sB) {
        // | aX bX |
        // | aY bY |
        var det = (a.X * b.Y) - (a.Y * b.X);
        if (det == 0) {
            sA = 0;
            sB = 0;
            return false;
        }

        // | pX bX |
        // | pY bY |
        sA = ((p.X * b.Y) - (p.Y * b.X)) / (double)det;

        // | aX pX |
        // | aY pY |
        sB = ((a.X * p.Y) - (a.Y * p.X)) / (double)det;
        return true;
    }

    private static ulong Part2(IEnumerable<ClawMachine> input) {
        var res = 0UL;

        foreach (var machine in input) {
            var machine2 = new ClawMachine2(machine.A, machine.B, (LongPoint)machine.Prize + 10000000000000);
            if (SolveMachine2(machine2, out var tokens)) {
                res += tokens;
            }
        }

        return res;
    }

    private static bool SolveMachine2(ClawMachine2 machine, out ulong tokens) {
        if (!CramersRule2(machine.A, machine.B, machine.Prize, out var aPresses, out var bPresses)) {
            tokens = 0;
            return false;
        }

        if (!IsWholeNumber(aPresses) || !IsWholeNumber(bPresses)) {
            tokens = 0;
            return false;
        }

        tokens = RoundToULong(aPresses) * 3 + RoundToULong(bPresses);
        return true;
    }

    // ReSharper disable once IdentifierTypo
    private static bool CramersRule2(Point a, Point b, LongPoint p, out double sA, out double sB) {
        // | aX bX |
        // | aY bY |
        var det = (a.X * b.Y) - (a.Y * b.X);
        if (det == 0) {
            sA = 0;
            sB = 0;
            return false;
        }

        // | pX bX |
        // | pY bY |
        sA = ((p.X * b.Y) - (p.Y * b.X)) / (double)det;

        // | aX pX |
        // | aY pY |
        sB = ((a.X * p.Y) - (a.Y * p.X)) / (double)det;
        return true;
    }

    private static bool IsWholeNumber(double value) => Math.Abs(value - Math.Round(value)) < 0.0001;

    private static int RoundToInt(double value) => (int)Math.Round(value);

    private static ulong RoundToULong(double value) => (ulong)Math.Round(value);
}