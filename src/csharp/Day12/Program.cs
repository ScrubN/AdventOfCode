using System.Diagnostics;
using System.Drawing;

namespace Day12;

internal static class Program {
    private enum Direction {
        Up,
        Down,
        Left,
        Right
    }

    private static readonly Point NoPoint = new(-1, -1);

    internal static void Main(string[] args) {
        var input = File.ReadAllLines("Inputs.txt");
        var points = input
            .SelectMany((s, y) => s.Select((_, x) => new Point(x, y)))
            .ToArray();

        var cost1 = Part1(input, points);
        var cost2 = Part2(input, points);

        Console.WriteLine($"Part 1: {cost1}");
        Console.WriteLine($"Part 2: {cost2}");
    }

    private static int Part1(string[] input, Point[] inputPoints) {
        var points = inputPoints.ToHashSet();

        var cost = 0;
        Point start;
        while ((start = points.FirstOrDefault(NoPoint)) != NoPoint) {
            var discovered = FloodFill(input, points, start);

            var area = discovered.Count;
            var perimeter = ComputePerimeter(discovered);
            cost += area * perimeter;
        }

        return cost;
    }

    private static HashSet<Point> FloodFill(string[] input, HashSet<Point> points, Point start) {
        var type = input[start.Y][start.X];
        HashSet<Point> discovered = [];
        HashSet<Point> queue = [start];

        Point point;
        while ((point = queue.FirstOrDefault(NoPoint)) != NoPoint) {
            queue.Remove(point);

            Debug.Assert(input[point.Y][point.X] == type);

            discovered.Add(point);
            points.Remove(point);

            for (var y = -1; y <= 1; y++) {
                var y2 = point.Y + y;
                if (y2 < 0 || y2 >= input.Length) {
                    continue;
                }

                for (var x = -1; x <= 1; x++) {
                    var x2 = point.X + x;
                    if (x2 < 0 || x2 >= input[y2].Length) {
                        continue;
                    }

                    if (Math.Abs(x) == Math.Abs(y)) {
                        continue;
                    }

                    if (input[y2][x2] != type) {
                        continue;
                    }

                    var newPoint = new Point(x2, y2);
                    if (!discovered.Contains(newPoint)) {
                        queue.Add(newPoint);
                    }
                }
            }
        }

        return discovered;
    }

    private static int ComputePerimeter(HashSet<Point> discovered) {
        var perimeter = 0;
        foreach (var point in discovered) {
            if (!discovered.Contains(point with { X = point.X + 1 })) {
                perimeter++;
            }

            if (!discovered.Contains(point with { X = point.X - 1 })) {
                perimeter++;
            }

            if (!discovered.Contains(point with { Y = point.Y + 1 })) {
                perimeter++;
            }

            if (!discovered.Contains(point with { Y = point.Y - 1 })) {
                perimeter++;
            }
        }

        return perimeter;
    }

    private static int Part2(string[] input, Point[] inputPoints) {
        var points = inputPoints.ToHashSet();

        var cost = 0;
        Point start;
        while ((start = points.FirstOrDefault(NoPoint)) != NoPoint) {
            var discovered = FloodFill(input, points, start);

            var area = discovered.Count;
            var sides = ComputeSides(discovered);
            cost += area * sides;
        }

        return cost;
    }

    private static int ComputeSides(HashSet<Point> discovered) {
        HashSet<(Point point, Direction normal)> sides = [];
        foreach (var point in discovered) {
            var newPoint = point with { X = point.X + 1 };
            if (!discovered.Contains(newPoint)) {
                sides.Add((point, Direction.Right));
            }

            newPoint = point with { X = point.X - 1 };
            if (!discovered.Contains(newPoint)) {
                sides.Add((point, Direction.Left));
            }

            newPoint = point with { Y = point.Y + 1 };
            if (!discovered.Contains(newPoint)) {
                sides.Add((point, Direction.Down));
            }

            newPoint = point with { Y = point.Y - 1 };
            if (!discovered.Contains(newPoint)) {
                sides.Add((point, Direction.Up));
            }
        }

        var orderedSides = sides.OrderBy(x => x.point.X).ThenBy(x => x.point.Y).ToArray();
        foreach (var side in orderedSides) {
            var newSide = side with { point = side.point with { X = side.point.X + 1 } };
            if (sides.Contains(newSide)) {
                sides.Remove(side);
            }

            newSide = side with { point = side.point with { Y = side.point.Y + 1 } };
            if (sides.Contains(newSide)) {
                sides.Remove(side);
            }
        }

        return sides.Count;
    }
}