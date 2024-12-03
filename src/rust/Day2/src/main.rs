use std::fs;
use std::io::{BufRead, BufReader};
use std::path::PathBuf;

fn main() {
    let reports = get_reports();

    let safe = check_safe(reports);

    println!("Safe: {}", safe);
}

fn get_reports() -> Vec<Vec<u32>> {
    let file_path: PathBuf = [".", "src", "Inputs.txt"].iter().collect();
    let file = fs::File::open(file_path).expect("Failed to open inputs file.");
    let reader = BufReader::new(file);

    let mut reports = vec![];

    for line in reader.lines() {
        match line {
            Ok(line_ok) => {
                let levels: Vec<u32> = line_ok
                    .split_whitespace()
                    .map(|x| x.parse::<u32>().expect("Failed to parse u32."))
                    .collect();

                reports.push(levels);
            }
            Err(e) => {
                eprintln!("{}", e);
            }
        }
    }

    reports
}

fn check_safe(reports: Vec<Vec<u32>>) -> u32 {
    let mut safe = 0;
    for levels in reports {
        if is_safe(&levels) {
            safe += 1;
            continue;
        }

        let mut i = 0;
        while i < levels.len() {
            let mut new_levels = levels.clone();
            new_levels.remove(i);
            if is_safe(&new_levels) {
                safe += 1;
                break;
            }

            i += 1;
        }
    }

    safe
}

fn is_safe(levels: &Vec<u32>) -> bool {
    let ascending = levels[0] < *levels.get(1).unwrap();

    let mut i = 0;
    while i < levels.len() - 1 {
        let a = levels[i];
        let b = levels[i + 1];

        if a == b {
            return false;
        }

        if a.abs_diff(b) > 3 {
            return false;
        }

        if ascending {
            if a > b {
                return false;
            }
        } else {
            if a < b {
                return false;
            }
        }

        i += 1;
    }

    true
}
