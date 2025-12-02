#include <iostream>
#include <string>
#include <vector>
#include <stdexcept>

#include "read_file.hpp"

enum class Direction {
    Left,
    Right
};

struct Instruction {
    Direction direction;
    int amount;

    int apply(int original) const {
        if (direction == Direction::Right) {
            return (original + amount) % 100;
        } else {
            return (original - amount) % 100;
        }
    }

    // Very naive approach, but works fine for these small numbers
    int count_zeroes(int& dial_position) const {
        int mutable_amount = amount;
        int zeroes_counter = 0;
        while (mutable_amount > 0) {
            dial_position = turn_dial_once(dial_position);
            if (dial_position == 0) {
                zeroes_counter += 1;
            }
            mutable_amount -= 1;
        }
        return zeroes_counter;
    }

    int turn_dial_once(int dial_position) const {
        if (direction == Direction::Right) {
            dial_position += 1;
            if (dial_position == 100) {
                dial_position = 0;
            }
        } else {
            dial_position -= 1;
            if (dial_position == -1) {
                dial_position = 99;
            }
        }
        return dial_position;
    }

    static Instruction parse(const std::string& line) {
        Instruction instruction;
        if (line[0] == 'R') {
            instruction.direction = Direction::Right;
        } else if (line[0] == 'L') {
            instruction.direction = Direction::Left;
        } else {
            throw std::invalid_argument("Line should start with R or L");
        }

        instruction.amount = std::stoi(line.substr(1, line.length() - 1));
        return instruction;
    }
};

int part1(std::string filename) {
    auto lines = AoC::utils::read_input_file(filename);
    std::vector<Instruction> instructions = {};
    for (const std::string& line : lines) {
        instructions.emplace_back(Instruction::parse(line));
    }

    // Counts how many times we stop on 0
    int zero_counter = 0;
    int dial_start = 50;
    for (const Instruction& instruction : instructions) {
        dial_start = instruction.apply(dial_start);
        if (dial_start == 0) {
            zero_counter++;
        }
    }

    return zero_counter;
}


int part2(std::string filename) {
    auto lines = AoC::utils::read_input_file(filename);
    std::vector<Instruction> instructions = {};
    for (const std::string& line : lines) {
        instructions.emplace_back(Instruction::parse(line));
    }

    // Counts how many times we stop on 0
    int zero_counter = 0;
    int dial_start = 50;
    for (const Instruction& instruction : instructions) {
        zero_counter += instruction.count_zeroes(dial_start);
    }

    return zero_counter;
}

int main() {
    int answer_part_1 = part1("day1.txt");
    std::cout << "Answer of part 1: " << answer_part_1 << std::endl;

    int answer_part_2 = part2("day1.txt");
    std::cout << "Answer of part 1: " << answer_part_2 << std::endl;
}
