namespace Day4;

internal static class Program {
    internal static void Main(string[] args) {
        var input = File.ReadAllLines("Inputs.txt");

        var xmasCount = 0;

        var yLen = input.Length;
        for (var y = 0; y < yLen; y++) {
            var line = input[y];
            var xLen = line.Length;
            for (var x = 0; x < xLen; x++) {
                if (line[x] != 'X') {
                    continue;
                }

                if (line.AsSpan(x).StartsWith("XMAS")) {
                    xmasCount++;
                }

                if (line.AsSpan(0, x + 1).EndsWith("SAMX")) {
                    xmasCount++;
                }

                if (input.Up("XMAS", x, y)) {
                    xmasCount++;
                }

                if (input.UpLeft("XMAS", x, y)) {
                    xmasCount++;
                }

                if (input.UpRight("XMAS", x, y)) {
                    xmasCount++;
                }

                if (input.Down("XMAS", x, y)) {
                    xmasCount++;
                }

                if (input.DownLeft("XMAS", x, y)) {
                    xmasCount++;
                }

                if (input.DownRight("XMAS", x, y)) {
                    xmasCount++;
                }
            }
        }

        Console.WriteLine(xmasCount);
    }

    private static bool UpLeft(this string[] s, string pattern, int x, int y) {
        if (y + 1 - pattern.Length < 0) {
            return false;
        }

        if (x + 1 - pattern.Length < 0) {
            return false;
        }

        for (var i = 0; i < pattern.Length; i++) {
            if (s[y - i][x - i] != pattern[i]) {
                return false;
            }
        }

        return true;
    }

    private static bool Up(this string[] s, string pattern, int x, int y) {
        if (y + 1 - pattern.Length < 0) {
            return false;
        }

        for (var i = 0; i < pattern.Length; i++) {
            if (s[y - i][x] != pattern[i]) {
                return false;
            }
        }

        return true;
    }

    private static bool UpRight(this string[] s, string pattern, int x, int y) {
        if (y + 1 - pattern.Length < 0) {
            return false;
        }

        if (x + pattern.Length > s[y].Length) {
            return false;
        }

        for (var i = 0; i < pattern.Length; i++) {
            if (s[y - i][x + i] != pattern[i]) {
                return false;
            }
        }

        return true;
    }

    private static bool DownLeft(this string[] s, string pattern, int x, int y) {
        if (pattern.Length + y > s.Length) {
            return false;
        }

        if (x + 1 - pattern.Length < 0) {
            return false;
        }

        for (var i = 0; i < pattern.Length; i++) {
            if (s[y + i][x - i] != pattern[i]) {
                return false;
            }
        }

        return true;
    }

    private static bool Down(this string[] s, string pattern, int x, int y) {
        if (pattern.Length + y > s.Length) {
            return false;
        }

        for (var i = 0; i < pattern.Length; i++) {
            if (s[y + i][x] != pattern[i]) {
                return false;
            }
        }

        return true;
    }

    private static bool DownRight(this string[] s, string pattern, int x, int y) {
        if (pattern.Length + y > s.Length) {
            return false;
        }

        if (x + pattern.Length > s[y].Length) {
            return false;
        }

        for (var i = 0; i < pattern.Length; i++) {
            if (s[y + i][x + i] != pattern[i]) {
                return false;
            }
        }

        return true;
    }
}