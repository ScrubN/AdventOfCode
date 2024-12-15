using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Day14;

internal static partial class Program {
    private record struct Robot(Point Position, Point Velocity);

    [GeneratedRegex(@"^p=(\d+),(\d+) v=(-?\d+),(-?\d+)$")]
    private static partial Regex RobotRegex { get; }

    internal static void Main(string[] args) {
        var robots = GetRobots();

        var safety = Part1(robots);

        Console.WriteLine(safety);
    }

    private static List<Robot> GetRobots() {
        var lines = File.ReadAllLines("Inputs.txt");

        List<Robot> robots = [];
        foreach (var line in lines) {
            var match = RobotRegex.Match(line);
            if (!match.Success) {
                throw new FormatException();
            }

            var position = new Point(int.Parse(match.Groups[1].ValueSpan), int.Parse(match.Groups[2].ValueSpan));
            var velocity = new Point(int.Parse(match.Groups[3].ValueSpan), int.Parse(match.Groups[4].ValueSpan));
            robots.Add(new Robot(position, velocity));
        }

        return robots;
    }

    private static long Part1(List<Robot> robots) {
        const int GRID_WIDTH = 101;
        const int GRID_HEIGHT = 103;
        var grid = SetupGrid(robots);

        for (var i = 0; i < 100; i++) {
            grid = MoveRobots(grid, GRID_WIDTH, GRID_HEIGHT);
        }

        // PrintGrid(GRID_WIDTH, GRID_HEIGHT, grid);

        // A | B
        // --+--
        // C | D
        var safetyA = ComputeSafety(0, GRID_WIDTH / 2, 0, GRID_HEIGHT / 2, grid);
        var safetyB = ComputeSafety((int)Math.Ceiling(GRID_WIDTH / 2d), GRID_WIDTH, 0, GRID_HEIGHT / 2, grid);
        var safetyC = ComputeSafety(0, GRID_WIDTH / 2, (int)Math.Ceiling(GRID_HEIGHT / 2d), GRID_HEIGHT, grid);
        var safetyD = ComputeSafety((int)Math.Ceiling(GRID_WIDTH / 2d), GRID_WIDTH, (int)Math.Ceiling(GRID_HEIGHT / 2d), GRID_HEIGHT, grid);

        return safetyA * safetyB * safetyC * safetyD;
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

    private static Dictionary<Point, List<Point>> MoveRobots(Dictionary<Point, List<Point>> grid, int gridWidth, int gridHeight) {
        var newGrid = new Dictionary<Point, List<Point>>(grid.Count);
        foreach (var (oldPosition, velocities) in grid) {
            foreach (var velocity in velocities) {
                var newPosition = oldPosition.Add(velocity).ConstrainToPositive(gridWidth, gridHeight);

                ref var newVelocities = ref CollectionsMarshal.GetValueRefOrAddDefault(newGrid, newPosition, out _);
                newVelocities ??= [];
                newVelocities.Add(velocity);
            }
        }

        return newGrid;
    }

    private static void PrintGrid(int gridWidth, int gridHeight, Dictionary<Point, List<Point>> grid) {
        for (var y = 0; y < gridHeight; y++) {
            for (var x = 0; x < gridWidth; x++) {
                if (grid.TryGetValue(new Point(x, y), out var robots)) {
                    Console.Write($"{robots.Count:X}");
                }
                else {
                    Console.Write(".");
                }
            }

            Console.WriteLine();
        }
    }

    private static long ComputeSafety(int xStart, int xEnd, int yStart, int yEnd, Dictionary<Point, List<Point>> grid) {
        var safety = 0L;
        for (var y = yStart; y < yEnd; y++) {
            for (var x = xStart; x < xEnd; x++) {
                if (grid.TryGetValue(new Point(x, y), out var robots)) {
                    safety += robots.Count;
                }
            }
        }

        return safety;
    }

    private static Point Add(this Point a, Point b) => new(a.X + b.X, a.Y + b.Y);

    private static Point ConstrainToPositive(this Point a, int maxX, int maxY) {
        var x = a.X;
        while (x < 0) {
            x += maxX;
        }

        x %= maxX;

        var y = a.Y;
        while (y < 0) {
            y += maxY;
        }

        y %= maxY;

        return new Point(x, y);
    }
}