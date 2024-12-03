use std::fs;
use std::io::{BufRead, BufReader};

fn main() {
    let (ids_1, ids_2) = get_ids();

    let mut sum = 0;
    let mut i = 0;
    while i < ids_1.len() {
        let diff = ids_1[i].abs_diff(ids_2[i]);
        sum += diff;

        i += 1;
    }

    println!("Sum: {}", sum);
}

fn get_ids() -> (Vec<i32>, Vec<i32>) {
    let file = fs::File::open("src/Inputs.txt").expect("Failed to open inputs file.");
    let reader = BufReader::new(file);

    let mut ids_1: Vec<i32> = vec![];
    let mut ids_2: Vec<i32> = vec![];

    for line in reader.lines() {
        match line {
            Ok(line_ok) => {
                let ids_line: Vec<&str> = line_ok
                    .split_whitespace()
                    .into_iter()
                    .filter(|x| !x.is_empty())
                    .collect();

                ids_1.push(ids_line[0].parse::<i32>().expect("Failed to parse id."));
                ids_2.push(ids_line[1].parse::<i32>().expect("Failed to parse id."));
            }
            Err(_) => {
                eprintln!("{}", line.err().unwrap());
                break;
            }
        }
    }

    ids_1.sort();
    ids_2.sort();

    (ids_1, ids_2)
}
