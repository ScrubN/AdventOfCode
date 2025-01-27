using System.Drawing;
using System.Runtime.InteropServices;

namespace Day16;

internal static class Program {
    private enum Direction {
        North,
        East,
        South,
        West
    }

    private record BfsNode(BfsNode? Parent, Point Position, Direction Direction, int Score);

    private record Maze(Point Start, Point End, HashSet<Point> Walls);

    internal static void Main(string[] args) {
        var maze = GetMaze();

        var score = Part1(maze);

        Console.WriteLine(score);
    }

    private static int Part1(Maze maze) {
        var score = Bfs(maze);
        return score;
    }

    private static int Bfs(Maze maze) {
        var start = new BfsNode(null, maze.Start, Direction.East, 0);
        var visited = new Dictionary<Point, BfsNode>();
        var queue = new Queue<BfsNode>();
        queue.Enqueue(start);

        BfsNode? end = null;
        while (queue.TryDequeue(out var node)) {
            ref var nodeOrDefault = ref CollectionsMarshal.GetValueRefOrAddDefault(visited, node.Position, out var exists);
            if (exists) {
                if (node.Score >= nodeOrDefault!.Score) {
                    continue;
                }
            }

            nodeOrDefault = node;

            if (node.Position == maze.End) {
                if (end is null || node.Score < end.Score) {
                    end = node;
                }
            }

            foreach (var newNode in GetNeighbors(node, maze)) {
                if (visited.TryGetValue(newNode.Position, out var oldNode)) {
                    if (oldNode.Score <= newNode.Score) {
                         continue;
                    }

                    visited.Remove(newNode.Position);
                }

                queue.Enqueue(newNode);
            }
        }

        if (end is null) {
            throw new Exception();
        }

        // PrintPath(end, maze);

        return end.Score;
    }

    private static void PrintPath(BfsNode end, Maze maze) {
        var path = new Dictionary<Point, BfsNode>();

        var currentNode = end;
        while (currentNode is not null) {
            path[currentNode.Position] = currentNode;
            currentNode = currentNode.Parent;
        }

        for (var y = 0; y < 15; y++) {
            for (var x = 0; x < 15; x++) {
                var point = new Point(x, y);

                if (path.TryGetValue(point, out var node)) {
                    var c = node.Direction switch {
                        Direction.North => '^',
                        Direction.South => 'v',
                        Direction.East => '>',
                        Direction.West => '<'
                    };
                    Console.Write(c);
                }
                else if (maze.Walls.Contains(point)) {
                    Console.Write('#');
                }
                else if (maze.Start == point) {
                    Console.Write('S');
                }
                else if (maze.End == point) {
                    Console.Write('E');
                }
                else {
                    Console.Write('.');
                }
            }

            Console.WriteLine();
        }
    }

    private static IEnumerable<BfsNode> GetNeighbors(BfsNode node, Maze maze) {
        for (var y = -1; y < 2; y++)
        for (var x = -1; x < 2; x++) {
            // x | 0 | x
            // 0 | x | 0
            // x | 0 | x
            if (Math.Abs(x) == Math.Abs(y)) {
                continue;
            }

            var newPos = node.Position with{ X = node.Position.X + x, Y = node.Position.Y + y };
            if (maze.Walls.Contains(newPos)) {
                continue;
            }

            // I hate this
            var newDirection = y switch {
                < 0 => Direction.North,
                > 0 => Direction.South,
                _ => x < 0 ? Direction.West : Direction.East
            };

            var newScore = node.Score + (newDirection == node.Direction ? 1 : 1_001);

            yield return new BfsNode(node, newPos, newDirection, newScore);
        }
    }

    private static Maze GetMaze() {
        var sr = new StreamReader("Inputs.txt");

        Point start = default;
        Point end = default;
        HashSet<Point> walls = [];

        var y = 0;
        while (sr.ReadLine() is { } line) {
            var x = 0;
            foreach (var c in line) {
                if (c == '#') {
                    walls.Add(new Point(x, y));
                }
                else if (c == 'S') {
                    start = new Point(x, y);
                }
                else if (c == 'E') {
                    end = new Point(x, y);
                }

                x++;
            }

            y++;
        }

        return new Maze(start, end, walls);
    }
}