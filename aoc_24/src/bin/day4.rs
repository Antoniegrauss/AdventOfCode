use std::fs;
use std::io::Lines;
use std::str;

#[test]
fn test_day4() {
    let puzzle_18_xmas = "MMMSXXMASM\nMSAMXMSMSA\nAMXSXMAAMM\nMSAMASMSMX\nXMASAMXAMM\nXXAMMXXAMA\nSMSMSASXSS\nSAXAMASAAA\nMAMMMXMMMM\nMXMXAXMASX";

    let puzzle_4_xmas = "..X...\n.SAMX.\n.A..A.\nXMAS.S\n.X....";

    let puzzle_3_xmas = "..X...\n.SAMX.\n.A....\nXMAS..\n.X....";

    assert_eq!(day4("."), 0);
    assert_eq!(day4("XMAS"), 1);
    assert_eq!(day4("SAMX"), 1);
    assert_eq!(day4("XXMAS"), 1);
    assert_eq!(day4("LKJDFGHOXMASJKLSDFGSAMX"), 2);

    // Test vertical lines
    assert_eq!(day4("X\nM\nA\nS"), 1);
    assert_eq!(day4("S\nA\nM\nX"), 1);
    assert_eq!(day4(puzzle_3_xmas), 3);

    // Test diagonal lines
    assert_eq!(day4(puzzle_4_xmas), 4);

    // Diagonals the other way as well
    assert_eq!(day4(puzzle_18_xmas), 18);
}

fn day4(data: &str) -> i64 {
    println!("{}", data);
    let lines: Vec<&str> = data.lines().collect::<Vec<&str>>();

    let horizontal = lines.clone();
    let mut count = count_xmax_in_lines(horizontal);

    let vertical = get_vertical_lines(&lines);
    count += count_xmax_in_lines_str(vertical);

    count += count_xmax_in_lines_str(get_diagonal_lines(&lines));

    // Diagonals the other way by mirroring the lines
    let mut lines_mirrored: Vec<String> = Vec::new();
    for line in &lines {
        lines_mirrored.push(line.chars().rev().collect());
    }

    let mut lines_mirrored_str: Vec<&str> = Vec::new();
    for line in &lines_mirrored {
        lines_mirrored_str.push(line);
    }

    count += count_xmax_in_lines_str(get_diagonal_lines(&lines_mirrored_str));

    count
}

fn get_vertical_lines(lines: &Vec<&str>) -> Vec<String> {
    // Assume all lines same length

    let mut vertical_lines: Vec<String> = Vec::new();
    for id in 0..lines.get(0).unwrap().len() {
        let mut new_line: Vec<&str> = Vec::new();
        for line in lines {
            new_line.push(&line[id..id + 1]);
        }
        vertical_lines.push(new_line.join("").clone());
    }

    vertical_lines
}

// Note can get diagonal right to left by mirroring lines
fn get_diagonal_lines(lines: &Vec<&str>) -> Vec<String> {
    let mut diagonal_left_to_right: Vec<String> = Vec::new();

    let width = lines.get(0).unwrap().len();
    let height = lines.len();

    for diagonal_start_x in 0..width {
        // Go from (0,0) to (n, n)
        // Then start at (0, 1) to (n-1, n)
        // Untill (0, n)
        let mut new_line: Vec<&str> = Vec::new();

        for x_coord in diagonal_start_x..width {
            let y_coord = x_coord - diagonal_start_x;
            if y_coord >= height {
                break;
            }
            new_line.push(&lines.get(y_coord).unwrap()[x_coord..x_coord + 1]);
        }
        diagonal_left_to_right.push(new_line.join("").clone());
    }

    for diagonal_start_y in 1..height {
        let mut new_line: Vec<&str> = Vec::new();

        for y_coord in diagonal_start_y..height {
            let x_coord = y_coord - diagonal_start_y;
            // Is this one necessary? Not sure
            if x_coord >= width {
                break;
            }
            new_line.push(&lines.get(y_coord).unwrap()[x_coord..x_coord + 1]);
        }

        diagonal_left_to_right.push(new_line.join("").clone());
    }

    diagonal_left_to_right
}

fn count_xmax_in_lines_str(lines: Vec<String>) -> i64 {
    let mut count = 0;
    for line in lines {
        count += match_xmas(&line);
    }

    count
}

fn count_xmax_in_lines(lines: Vec<&str>) -> i64 {
    let mut count = 0;
    for line in lines {
        count += match_xmas(line);
    }

    count
}

fn match_xmas(line: &str) -> i64 {
    (line.matches("XMAS").count() + line.matches("SAMX").count()) as i64
}

#[test]
fn test_day4_part4() {
    assert_eq!(day4_part2("."), 0);
    assert_eq!(day4_part2("M.S\n.A.\nM.S"), 1);
}

fn day4_part2(data: &str) -> i64 {
    let lines: Vec<&str> = data.lines().collect::<Vec<&str>>();

    let width = lines.get(0).unwrap().len();
    let height = lines.len();

    let mut xmasses = 0;
    for x_coord in 0..width {
        for y_coord in 0..height {
            let x_id: i64 = x_coord as i64;
            let y_id: i64 = y_coord as i64;
            if x_id - 1 < 0 || x_id + 1 >= width as i64 || y_id - 1 < 0 || y_id + 1 >= height as i64 {
                continue;
            }
            let center: &str = &lines.get(y_coord).unwrap()[x_coord..x_coord + 1];
            let top_left = &lines.get(y_coord - 1).unwrap()[x_coord - 1..x_coord];
            let top_right = &lines.get(y_coord - 1).unwrap()[x_coord + 1..x_coord + 2];

            let bottom_left = &lines.get(y_coord + 1).unwrap()[x_coord - 1..x_coord];
            let bottom_right = &lines.get(y_coord + 1).unwrap()[x_coord + 1..x_coord + 2];

            if match_mas(String::new() + top_left + center + bottom_right)
                && match_mas(String::new() + top_right + center + bottom_left)
            {
                xmasses += 1;
            }
        }
    }

    xmasses
}

fn match_mas(string: String) -> bool {
    string == "MAS" || string == "SAM"
}

fn main() {
    let data = fs::read_to_string("input/Day4.txt").expect("Unable to read file");
    print!("{}", day4_part2(&data));
}
