use std::iter::zip;
use std::fs;

fn main() {
    let data = fs::read_to_string("input/InputDay1.txt").expect("Unable to read file");
    print!("{}", day1(&data));
}

#[cfg(test)]
mod tests {
    // Note this useful idiom: importing names from outer (for mod tests) scope.
    use super::*;
    
    #[test]
    fn test_day_1() {
        // This assert would fire and test will fail.
        // Please note, that private functions can be tested too!
        assert_eq!(day1("1 1"), "1");
    }
}

fn day1(data: &str) -> String {
    let pairs = data.lines();
    let mut left: Vec<i64> = Vec::new();
    let mut right: Vec<i64> = Vec::new();

    for pair in pairs {
        let parts: Vec<String> = pair.split_whitespace().map(str::to_string).collect();
        if parts.len() != 2 {
            continue;
        }

        let left_number: i64 = parts
            .get(0)
            .unwrap()
            .parse()
            .expect("Unable to parse left number");
        left.push(left_number);

        let right_number: i64 = parts
            .get(1)
            .unwrap()
            .parse()
            .expect("Unable to parse right number");
        right.push(right_number);
    }

    left.sort();
    right.sort();

    let mut sum = 0;
    let iter = zip(left, right.clone());
    for pair in iter {
        let occurence = right.clone().into_iter().filter(|x| *x == pair.0).count() as i64;
        sum += pair.0 * occurence as i64;
    }

    return sum.to_string();
}
