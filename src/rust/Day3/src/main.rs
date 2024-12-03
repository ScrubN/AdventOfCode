use regex::Regex;
use std::fs;
use std::path::PathBuf;

fn main() {
    let file_path: PathBuf = [".", "src", "Inputs.txt"].iter().collect();
    let input = fs::read_to_string(file_path).expect("Failed to read input");
    let reg = Regex::new(r"mul\((\d+),(\d+)\)").expect("Failed to create regex");

    let mut sum = 0;
    for cap in reg.captures_iter(input.as_str()) {
        let a = cap
            .get(1)
            .expect("Failed to capture num")
            .as_str()
            .parse::<u32>()
            .expect("Failed to parse num");
        let b = cap
            .get(2)
            .expect("Failed to capture num")
            .as_str()
            .parse::<u32>()
            .expect("Failed to parse num");

        sum += a * b;
    }

    println!("Sum: {}", sum);
}
