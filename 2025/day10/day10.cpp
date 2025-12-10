#include <iostream>
#include <string>
#include <vector>
#include <stdexcept>
#include <functional>
#include <cctype>
#include <set>
#include <sstream>
#include <cassert>
#include <algorithm>
#include <cmath>
#include <numeric>
#include <queue>

#include "read_file.hpp"

struct Problem {
    // Initially all off
    std::vector<bool> lights;

    // Each button toggles the lights (ids are in the list)
    std::vector<std::vector<int>> buttons;

    // Pattern to match
    std::vector<bool> answer;

    Problem(std::string line)  {
        auto parts = AoC::utils::split_by_delimiter(line, ' ');
        for (char c : parts[0]) {
            if (c == '.' || c == '#') {
                answer.emplace_back(c);
                lights.emplace_back(false);
            }
        }

        for (int i = 1; i < parts.size() - 1; i++) {
            std::vector<int> new_button;
            for (char c : parts[i]) {
                if (isdigit(c)) {
                    new_button.emplace_back(c - '0');
                }
            }
        }
    }

    void press_button(int button_id) {
        for (int light_id : buttons[button_id]) {
            lights[light_id] != lights[light_id];
        }
    }

    bool check_sequence(const std::vector<int>& button_ids) {
        reset();
        for (int button_id : button_ids) {
            press_button(button_id);
        }
        return is_done();
    }

    bool is_done() const {
        return lights == answer;
    }

    void reset() {
        std::fill(lights.begin(), lights.end(), false);
    }
};

long solve_problem_part_1(std::string line) {
    Problem problem(line);
    
    long button_presses = 0;
    
    // Generate all combinations of button presses
    int options = problem.buttons.size();
    std::vector<std::vector<int>> prev_sequences;
    std::vector<std::vector<int>> new_sequences;

    while(! problem.is_done()) {
        // First loop populate the sequences
        if (prev_sequences.empty()) {
            for (int i = 0; i < options; i++) {
                new_sequences.emplace_back(std::vector<int>{i});
                if (problem.check_sequence({i})) {
                    return 1;
                }
            }
            continue;
        }

        for (int i = 0; i < options; i++) {
            for (std::vector<int> sequence : prev_sequences) {
                sequence.emplace_back(i);
                if (problem.check_sequence(sequence)) {
                    return sequence.size();
                }
                new_sequences.emplace_back(sequence);
            }
        }
    }

    return 0;
}

long part1(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    long sum = 0;
    for (std::string line : lines) {
        sum += solve_problem_part_1(line);
    }
    return sum;
}

long part2(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    return 0;
}

void execute_part(std::function<long(std::string)> part_x, std::string file, int part_number)
{
    long answer_part_1 = part_x(file);
    std::cout << "Answer of part " << part_number << ": " << answer_part_1 << std::endl;
}

int main()
{
    execute_part(part1, "day10.txt", 1);
    // Note: have to guess whether left or right is the outside of the circuit
    execute_part(part2, "day10.txt", 2);
    return 0;
}
