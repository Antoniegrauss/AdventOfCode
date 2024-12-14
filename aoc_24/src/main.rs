use std::fs;

fn main() {
    let data = fs::read_to_string("input/InputDay1.txt").expect("Unable to read file");
    print!("{}", day1::day1(&data));
}
