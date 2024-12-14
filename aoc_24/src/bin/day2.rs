use core::num;
use std::ops;
use std::fs;

fn main() {
    let data = fs::read_to_string("input/Day2.txt").expect("Unable to read file");
    print!("{}", day2(&data));
}

#[test]
fn test_day_1_part1() {
    // This assert would fire and test will fail.
    // Please note, that private functions can be tested too!

    assert_eq!(day2("1 2"), 1);
    assert_eq!(day2("1 2 3 7"), 0);
    assert_eq!(day2("1 2 3 6"), 1);
    assert_eq!(day2("1 2 3 7\n1 2 3 6"), 1);
    assert_eq!(day2("1 2 3 6\n1 2 3 6"), 2);    

    assert_eq!(day2("6 3 1"), 1); 
    assert_eq!(day2("1 2 3 6\n1 2 3 6\n 11 8 5 1\n1 3 5 6\n99 97 96"), 4); 
}

#[test]
fn test_day_1_part2() {
    // This assert would fire and test will fail.
    // Please note, that private functions can be tested too!
    
    assert_eq!(day2("1 2"), 1);
    assert_eq!(day2("1 2 3 7"), 1);
    assert_eq!(day2("1 2 3 6"), 1);
    assert_eq!(day2("1 2 3 7\n1 2 3 6"), 2);
    assert_eq!(day2("1 2 3 6\n1 2 3 6"), 2);    
    
    assert_eq!(day2("6 3 1"), 1); 
    assert_eq!(day2("1 2 3 6\n1 2 3 6\n 11 8 5 1\n1 3 5 6\n99 97 96"), 5);  
    
    assert_eq!(day2("1 2 7 8 9"), 0);
    assert_eq!(day2("9 7 6 2 1"), 0);
    assert_eq!(day2("1 3 2 4 5"), 1);
    assert_eq!(day2("8 6 4 4 1"), 1);
    assert_eq!(day2("8 6 4 5 3 2 1"), 1);
    assert_eq!(day2("6 8 11 12 14 16 18 16
                    73 76 79 80 81 84 86 86
                    32 33 34 37 40 44
                    9 11 13 14 17 24
                    59 61 64 62 65"), 5) 
}


fn day2(data: &str) -> i64 {
    let mut good_lists = 0;
    let lines = data.lines();
    for line in lines {
        let numbers: Vec<i64> = line
            .split_whitespace()
            .map(|x| x.parse().expect("Can not parse to i64"))
            .collect();

        // Check all ascending
        let id = list_sorted_till_id(&numbers);
        if id == numbers.len() {
            good_lists += 1;
        }
        else {
            for id in 0..numbers.len() {
                let mut numbers_with_one_removed = numbers.clone();
                numbers_with_one_removed.remove(id);
    
                let id = list_sorted_till_id(&numbers_with_one_removed);
                if id == numbers_with_one_removed.len() {
                    good_lists += 1;
                    break;
                }
            }
        }
    }

    good_lists
}

fn list_sorted_till_id(numbers: &Vec<i64>) -> usize {
    let mut current = 0;
    let mut is_ascending = true;
    let mut is_descending = true;

    let mut id = 0;
    for number in numbers {
        if current == 0 {
            current = *number;
            id += 1;
            continue;
        }
        if !difference_within_range(number - current) {
            is_ascending = false;
        }
        if !difference_within_range(current - number) {
            is_descending = false;
        }
        current = *number;

        if !is_ascending && !is_descending {
            return id;
        }
        id += 1;
    }

    id
}

fn difference_within_range(difference: i64) -> bool {
    let smallest_difference = 1;
    let largest_difference = 3;

    return difference >= smallest_difference && difference <= largest_difference
}