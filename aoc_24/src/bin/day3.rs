use core::num;
use std::fs;

use regex::Regex;

fn main() {
    let regex_part1 = Regex::new(r"mul\([0-9]{1,3},[0-9]{1,3}\)").unwrap();
    let regex_part2 = Regex::new(r"(don't\(\))|(do\(\))|(mul\([0-9]{1,3},[0-9]{1,3}\))").unwrap();

    let data = fs::read_to_string("input/Day3.txt").expect("Unable to read file");
    // let test_string = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";

    let muls: Vec<&str> = regex_part2.find_iter(&data).map(|m| m.as_str()).collect();
    print!("{:?}", muls);

    let mut sum = 0;
    let mut disabled = false;
    for multiplier in muls {
        if multiplier == "do()" {
            disabled = false;
            print!("Enabled again!");
            continue;
        }
        else if multiplier == "don't()" {
            disabled = true;
            print!("Disabled!");
            continue;
        }

        if disabled {
            continue;
        }

        let stripped_string = multiplier
            .strip_prefix("mul(")
            .unwrap()
            .strip_suffix(")")
            .unwrap();
        let numbers: Vec<i64> = stripped_string.split(",").map(|x| x.parse().unwrap()).collect();

        sum += numbers[0] * numbers[1];
        println!("{:?}", numbers);
        println!("{}", sum);
    }
    println!("{}", sum);
}
