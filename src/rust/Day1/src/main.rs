use std::collections::HashMap;
use std::fs;
use std::io::{BufRead, BufReader};

fn main() {
    let (ids_1, ids_2) = get_ids();

    let (sum, similarity) = get_sum_and_similarity(ids_1, ids_2);

    println!("Sum: {} Similarity: {}", sum, similarity);
}

fn get_sum_and_similarity(ids_1: Vec<u32>, ids_2: Vec<u32>) -> (u32, u32) {
    let mut occurrences = HashMap::<u32, u32>::new();
    for id in &ids_2 {
        if let Some(val) = occurrences.get_mut(id) {
            *val += 1;
        } else {
            occurrences.insert(*id, 1);
        }
    }

    let mut sum = 0;
    let mut similarity = 0;
    let mut i = 0;
    while i < ids_1.len() {
        let id1 = ids_1[i];
        let id2 = ids_2[i];

        sum += id1.abs_diff(id2);

        if let Some(val) = occurrences.get(&id1) {
            similarity += id1 * val;
        }

        i += 1;
    }

    (sum, similarity)
}

fn get_ids() -> (Vec<u32>, Vec<u32>) {
    let file = fs::File::open("src/Inputs.txt").expect("Failed to open inputs file.");
    let reader = BufReader::new(file);

    let mut ids_1: Vec<u32> = vec![];
    let mut ids_2: Vec<u32> = vec![];

    for line in reader.lines() {
        match line {
            Ok(line_ok) => {
                let ids_line: Vec<&str> = line_ok
                    .split_whitespace()
                    .into_iter()
                    .filter(|x| !x.is_empty())
                    .collect();

                ids_1.push(ids_line[0].parse::<u32>().expect("Failed to parse id."));
                ids_2.push(ids_line[1].parse::<u32>().expect("Failed to parse id."));
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
