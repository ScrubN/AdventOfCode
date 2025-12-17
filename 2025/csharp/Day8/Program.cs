namespace Day8;

internal static class Program {
    extension<T>(HashSet<T> set) {
        public void AddRange(IEnumerable<T> collection) {
            foreach (var item in collection) {
                set.Add(item);
            }
        }
    }

    private record struct Connection(Point A, Point B) {
        public long SquareMagnitude => A.SquareDistance(B);
    }

    private record Point(int X, int Y, int Z) {
        public HashSet<Point>? Circuit { get; set; }

        public long SquareDistance(Point other) =>
            ((long)X - other.X) * ((long)X - other.X) +
            ((long)Y - other.Y) * ((long)Y - other.Y) +
            ((long)Z - other.Z) * ((long)Z - other.Z);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public virtual bool Equals(Point? other) {
            if (other is null) {
                return false;
            }

            return X == other.X &&
                   Y == other.Y &&
                   Z == other.Z;
        }
    }

    private static void Main(string[] args) {
        var points = GetPoints();

        var part1 = Part1(points);

        Console.WriteLine($"Part 1: {part1}");
    }

    private static List<Point> GetPoints() {
        var points = new List<Point>();

        using var sr = new StreamReader("Inputs.txt");
        while (sr.ReadLine() is { } line) {
            var split = line.Split(',');
            points.Add(new Point(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2])));
        }

        return points;
    }

    private static long Part1(List<Point> points) {
        var circuits = CreateCircuits(points);

        return circuits
            .OrderByDescending(x => x.Count)
            .Take(3)
            .Aggregate(1L, (accumulate, list) => accumulate * list.Count);
    }

    private static IEnumerable<HashSet<Point>> CreateCircuits(List<Point> points) {
        var connections = new List<Connection>();
        for (var i = 0; i < points.Count - 1; i++) {
            var a = points[i];

            for (var j = i + 1; j < points.Count; j++) {
                var b = points[j];

                var connection = new Connection(a, b);

                connections.Add(connection);
            }
        }

        var closest = connections
            .OrderBy(x => x.SquareMagnitude)
            .Take(1_000);

        CollapseCircuits(closest);

        return points
            .Where(x => x.Circuit != null)
            .Select(x => x.Circuit!)
            .Distinct();
    }

    private static void CollapseCircuits(IEnumerable<Connection> closest) {
        foreach (var (a, b) in closest) {
            // A is in a circuit
            if (a.Circuit != null) {
                // B is in a circuit
                if (b.Circuit != null) {
                    // They are in the same circuit
                    if (ReferenceEquals(a.Circuit, b.Circuit)) {
                        continue;
                    }

                    // They are in different circuits
                    a.Circuit.AddRange(b.Circuit);
                    foreach (var point in b.Circuit) {
                        point.Circuit = a.Circuit;
                    }
                }
                // B is not in a circuit
                else {
                    a.Circuit.Add(b);
                    b.Circuit = a.Circuit;
                }
            }
            // A is not in a circuit, B is in a circuit
            else if (b.Circuit != null) {
                a.Circuit = b.Circuit;
                a.Circuit.Add(a);
            }
            // Neither are in a circuit
            else {
                a.Circuit = [a, b];
                b.Circuit = a.Circuit;
            }
        }
    }
}