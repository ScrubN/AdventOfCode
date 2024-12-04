use std::fs;
use std::path::PathBuf;

fn main() {
    let input = get_input();

    let mut xmas = 0;

    let mut y = 0;
    while y < input.len() {
        let mut x = 0;
        while x < input[y].len() {
            if input[y].as_bytes()[x] as char != 'X' {
                x += 1;
                continue;
            }

            if input[y][x..].starts_with("XMAS") {
                xmas += 1;
            }

            if input[y][..=x].ends_with("SAMX") {
                xmas += 1;
            }

            if is_up(&input, x, y) {
                xmas += 1;
            }

            if is_down(&input, x, y) {
                xmas += 1;
            }

            if is_up_right(&input, x, y) {
                xmas += 1;
            }

            if is_down_left(&input, x, y) {
                xmas += 1;
            }

            if is_down_right(&input, x, y) {
                xmas += 1;
            }

            if is_up_left(&input, x, y) {
                xmas += 1;
            }

            x += 1;
        }

        y += 1;
    }

    println!("{}", xmas);
}

fn get_input() -> Vec<String> {
    let file_path: PathBuf = [".", "src", "Inputs.txt"].iter().collect();
    let input = fs::read_to_string(file_path).expect("Failed to read input");
    input.lines().map(|x| x.to_string()).collect()
}

fn is_down(input: &Vec<String>, x: usize, y: usize) -> bool {
    check_vertical(input, "XMAS", x, y)
}

fn is_up(input: &Vec<String>, x: usize, y: usize) -> bool {
    if y < 3 {
        return false;
    }

    check_vertical(input, "SAMX", x, y - 3)
}

fn check_vertical(input: &Vec<String>, pattern: &str, x: usize, y: usize) -> bool {
    if y + pattern.len() > input.len() {
        return false;
    }

    if x >= input[y].len() {
        return false;
    }

    let mut offset = 0;
    for p in pattern.chars() {
        if input[y + offset].as_bytes()[x] as char != p {
            return false;
        }

        offset += 1;
    }

    true
}

//  /
// /
// -->
fn is_up_right(input: &Vec<String>, x: usize, y: usize) -> bool {
    if y < 3 {
        return false;
    }

    check_diagonal_forward(input, "SAMX", x + 3, y - 3)
}

//  /
// /
// <--
fn is_down_left(input: &Vec<String>, x: usize, y: usize) -> bool {
    check_diagonal_forward(input, "XMAS", x, y)
}

//  /
// /
// <--
fn check_diagonal_forward(input: &Vec<String>, pattern: &str, x: usize, y: usize) -> bool {
    if y + pattern.len() > input.len() {
        return false;
    }

    if x < pattern.len() - 1 || x >= input[y].len() {
        return false;
    }

    let mut offset = 0;
    for p in pattern.chars() {
        if input[y + offset].as_bytes()[x - offset] as char != p {
            return false;
        }

        offset += 1;
    }

    true
}

// \
//  \
// -->
fn is_down_right(input: &Vec<String>, x: usize, y: usize) -> bool {
    check_diagonal_backward(input, "XMAS", x, y)
}

// \
//  \
// <--
fn is_up_left(input: &Vec<String>, x: usize, y: usize) -> bool {
    if x < 3 || y < 3 {
        return false;
    }

    check_diagonal_backward(input, "SAMX", x - 3, y - 3)
}

// \
//  \
// -->
fn check_diagonal_backward(input: &Vec<String>, pattern: &str, x: usize, y: usize) -> bool {
    if y + pattern.len() > input.len() {
        return false;
    }

    if x + pattern.len() > input[y].len() {
        return false;
    }

    let mut offset = 0;
    for p in pattern.chars() {
        if input[y + offset].as_bytes()[x + offset] as char != p {
            return false;
        }

        offset += 1;
    }

    true
}
