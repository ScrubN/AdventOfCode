using System.Drawing;

namespace Day12;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllLines("Inputs.txt");

        var cost1 = Part1(input);
        var cost2 = Part2(input);

        Console.WriteLine($"Part 1: {cost1}");
        Console.WriteLine($"Part 2: {cost2}");
    }

    private static int Part1(string[] input) {
        var points = input
            .SelectMany((s, y) => s.Select((_, x) => (Point?)new Point(x, y)))
            .ToHashSet();

        Dictionary<Point, int> costs = [];
        while (points.FirstOrDefault() is { } start) {
            var discovered = FloodFill(input, points, start);

            var area = discovered.Count;
            var perimeter = ComputePerimeter(discovered);
            costs[start] = area * perimeter;
        }

        return costs.Sum(x => x.Value);
    }

    private static HashSet<Point> FloodFill(string[] input, HashSet<Point?> points, Point start) {
        var type = input[start.Y][start.X];
        HashSet<Point> discovered = [];
        HashSet<Point?> queue = [start];

        while (queue.FirstOrDefault() is { } pointToCheck) {
            queue.Remove(pointToCheck);

            if (input[pointToCheck.Y][pointToCheck.X] != type) {
                continue;
            }

            discovered.Add(pointToCheck);
            points.Remove(pointToCheck);

            for (var y = -1; y <= 1; y++) {
                var y2 = pointToCheck.Y + y;
                if (y2 < 0 || y2 >= input.Length) {
                    continue;
                }

                for (var x = -1; x <= 1; x++) {
                    var x2 = pointToCheck.X + x;
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
                    if (points.Contains(newPoint) && !discovered.Contains(newPoint)) {
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

    private static int Part2(string[] input) {
        var points = input
            .SelectMany((s, y) => s.Select((_, x) => (Point?)new Point(x, y)))
            .ToHashSet();

        Dictionary<Point, int> costs = [];
        while (points.FirstOrDefault() is { } start) {
            var discovered = FloodFill(input, points, start);

            var area = discovered.Count;
            var sides = ComputeSides(discovered);
            costs[start] = area * sides;
        }

        return costs.Sum(x => x.Value);
    }

    private enum Direction {
        Up,
        Down,
        Left,
        Right
    }

    private static int ComputeSides(HashSet<Point> discovered) {
        HashSet<(Point point, Direction direction)> sides = [];
        foreach (var point in discovered) {
            var newPoint = point with { X = point.X + 1 };
            if (!discovered.Contains(newPoint)) {
                sides.Add((newPoint, Direction.Right));
            }

            newPoint = point with { X = point.X - 1 };
            if (!discovered.Contains(newPoint)) {
                sides.Add((newPoint, Direction.Left));
            }

            newPoint = point with { Y = point.Y + 1 };
            if (!discovered.Contains(newPoint)) {
                sides.Add((newPoint, Direction.Down));
            }

            newPoint = point with { Y = point.Y - 1 };
            if (!discovered.Contains(newPoint)) {
                sides.Add((newPoint, Direction.Up));
            }
        }

        foreach (var side in sides.OrderBy(x => x.point.X).ThenBy(x => x.point.Y).ToArray()) {
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