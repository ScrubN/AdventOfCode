using System.Drawing;

namespace Day12;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllLines("Inputs.txt");

        var cost = Part1(input);

        Console.WriteLine(cost);
    }

    private static int Part1(string[] input) {
        var points = input
            .SelectMany((s, y) => s.Select((_, x) => (Point?)new Point(x, y)))
            .ToHashSet();

        Dictionary<Point, int> costs = [];
        while (points.FirstOrDefault() is { } point) {
            var type = input[point.Y][point.X];
            HashSet<Point> discovered = [];
            HashSet<Point?> queue = [point];

            Console.WriteLine($"{type}: {point.X}, {point.Y} - {points.Count} left.");

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

            var area = discovered.Count;
            var perimeter = ComputePerimeter(discovered);

            costs[point] = area * perimeter;
        }

        return costs.Sum(x => x.Value);
    }

    private static void PrintArea(string[] input, HashSet<Point> discovered) {
        for (var y = 0; y < input.Length; y++) {
            for (var x = 0; x < input[y].Length; x++) {
                if (discovered.Contains(new Point(x, y))) {
                    Console.Write(input[y][x]);
                }
                else {
                    Console.Write('.');
                }
            }

            Console.WriteLine();
        }
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
}