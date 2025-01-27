using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Day14;

internal static partial class Program {
    private record struct Robot(Point Position, Point Velocity);

    [GeneratedRegex(@"^p=(\d+),(\d+) v=(-?\d+),(-?\d+)$")]
    private static partial Regex RobotRegex { get; }

    private const int GRID_WIDTH = 101;
    private const int GRID_HEIGHT = 103;

    internal static void Main(string[] args) {
        var robots = GetRobots();

        var safety = Part1(robots);
        var secondsElapsed = Part2(robots);

        Console.WriteLine($"Part 1: {safety}");
        Console.WriteLine($"Part 2: {secondsElapsed}");
    }

    private static List<Robot> GetRobots() {
        List<Robot> robots = [];
        foreach (var line in File.ReadLines("Inputs.txt")) {
            var match = RobotRegex.Match(line);
            Debug.Assert(match.Success);

            var position = new Point(int.Parse(match.Groups[1].ValueSpan), int.Parse(match.Groups[2].ValueSpan));
            var velocity = new Point(int.Parse(match.Groups[3].ValueSpan), int.Parse(match.Groups[4].ValueSpan));
            robots.Add(new Robot(position, velocity));
        }

        return robots;
    }

    private static long Part1(List<Robot> robots) {
        var grid = SetupGrid(robots);

        var counts = CountRobotsAfterMove(grid, 100);

        // A | B
        // --+--
        // C | D
        var safetyA = ComputeSafety(0, GRID_WIDTH / 2, 0, GRID_HEIGHT / 2, counts);
        var safetyB = ComputeSafety((int)Math.Ceiling(GRID_WIDTH / 2d), GRID_WIDTH, 0, GRID_HEIGHT / 2, counts);
        var safetyC = ComputeSafety(0, GRID_WIDTH / 2, (int)Math.Ceiling(GRID_HEIGHT / 2d), GRID_HEIGHT, counts);
        var safetyD = ComputeSafety((int)Math.Ceiling(GRID_WIDTH / 2d), GRID_WIDTH, (int)Math.Ceiling(GRID_HEIGHT / 2d), GRID_HEIGHT, counts);

        return safetyA * safetyB * safetyC * safetyD;
    }

    private static long Part2(List<Robot> robots) {
        var grid = SetupGrid(robots);

        // This should not work but it does lmao
        for (var elapsed = 1;; elapsed++) {
            var counts = CountRobotsAfterMove(grid, elapsed);

            if (counts.Values.All(x => x == 1)) {
                return elapsed;
            }
        }
    }

    private static Dictionary<Point, List<Point>> SetupGrid(List<Robot> robots) {
        Dictionary<Point, List<Point>> grid = [];
        foreach (var robot in robots) {
            ref var position = ref CollectionsMarshal.GetValueRefOrAddDefault(grid, robot.Position, out _);
            position ??= [];
            position.Add(robot.Velocity);
        }

        return grid;
    }

    private static Dictionary<Point, int> CountRobotsAfterMove(Dictionary<Point, List<Point>> grid, int iterations) {
        var newGrid = new Dictionary<Point, int>();
        foreach (var (oldPosition, velocities) in grid) {
            foreach (var velocity in velocities) {
                var newPosition = oldPosition.Add(velocity.Mult(iterations)).ConstrainToPositive(GRID_WIDTH, GRID_HEIGHT);

                ref var robotCount = ref CollectionsMarshal.GetValueRefOrAddDefault(newGrid, newPosition, out _);
                robotCount++;
            }
        }

        return newGrid;
    }

    private static Point Mult(this Point a, int b) => new(a.X * b, a.Y * b);

    private static Point Add(this Point a, Point b) => new(a.X + b.X, a.Y + b.Y);

    private static Point ConstrainToPositive(this Point a, int maxX, int maxY) {
        var x = a.X % maxX;
        if (x < 0) {
            x += maxX;
        }

        var y = a.Y % maxY;
        if (y < 0) {
            y += maxY;
        }

        return new Point(x, y);
    }

    private static long ComputeSafety(int xStart, int xEnd, int yStart, int yEnd, Dictionary<Point, int> grid) {
        var safety = 0L;
        for (var y = yStart; y < yEnd; y++) {
            for (var x = xStart; x < xEnd; x++) {
                if (grid.TryGetValue(new Point(x, y), out var robots)) {
                    safety += robots;
                }
            }
        }

        return safety;
    }
}