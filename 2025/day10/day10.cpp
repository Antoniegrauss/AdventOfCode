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

struct Problem
{
    // Initially all off
    std::vector<bool> lights;

    // Each button toggles the lights (ids are in the list)
    std::vector<std::vector<int>> buttons;

    // Pattern to match
    std::vector<bool> answer;

    // Part 2 patter to match
    std::vector<int> current_joltage;
    std::vector<int> joltage_answer;

    Problem(std::string line)
    {
        auto parts = AoC::utils::split_by_delimiter(line, ' ');
        for (char c : parts[0])
        {
            if (c == '.' || c == '#')
            {
                answer.emplace_back(c == '#');
                lights.emplace_back(false);
            }
        }

        for (int i = 1; i < parts.size() - 1; i++)
        {
            std::vector<int> new_button;
            for (char c : parts[i])
            {
                if (isdigit(c))
                {
                    new_button.emplace_back(c - '0');
                }
            }
            buttons.emplace_back(new_button);
        }

        auto joltages = AoC::utils::split_by_delimiter(parts.back(), ',');
        for (std::string joltage : joltages) {
            bool skip = false;
            for (char c : joltage) {
                if (!isdigit(c)) skip = true;
            }
            if (skip) continue;
            joltage_answer.emplace_back(stoi(joltage));
            current_joltage.emplace_back(0);
        }
    }

    void press_button(int button_id)
    {
        for (int light_id : buttons[button_id])
        {
            lights[light_id] = !lights[light_id];
        }
    }

    void press_button_part_2(int button_id)
    {
        for (int joltage_id : buttons[button_id])
        {
            current_joltage[joltage_id] += 1;
        }
    }

    bool check_sequence(const std::vector<int> &button_ids)
    {
        reset();
        for (int button_id : button_ids)
        {
            press_button(button_id);
        }
        return is_done();
    }

    bool check_sequence_part_2(const std::vector<int> &button_ids)
    {
        reset_joltage();
        for (int button_id : button_ids)
        {
            press_button_part_2(button_id);
        }
        return current_joltage == joltage_answer;
    }

    bool is_done() const
    {
        return lights == answer;
    }

    void reset()
    {
        std::fill(lights.begin(), lights.end(), false);
    }

    void reset_joltage() {
        std::fill(current_joltage.begin(), current_joltage.end(), 0);
    }
};

std::vector<std::vector<int>> extend_sequences(int options,
                                               const std::vector<std::vector<int>> &sequences)
{
    std::vector<std::vector<int>> new_sequences;
    for (int i = 0; i < options; i++)
    {
        for (std::vector<int> sequence : sequences)
        {
            sequence.emplace_back(i);
            new_sequences.emplace_back(sequence);
        }
    }
    return new_sequences;
}

long solve_problem_part_1(std::string line)
{
    Problem problem(line);

    long button_presses = 0;

    // Generate all combinations of button presses
    int options = problem.buttons.size();
    std::vector<std::vector<int>> sequences;
    for (int i = 0; i < options; i++)
    {
        sequences.emplace_back(std::vector<int>{i});
    }

    while (!problem.is_done())
    {
        for (const std::vector<int>& sequence : sequences) {
            if (problem.check_sequence(sequence)) {
                return sequence.size();
            }
        }

        sequences = extend_sequences(options, sequences);
    }

    return 0;
}

long solve_problem_part_2(std::string line)
{
    Problem problem(line);

    long button_presses = 0;

    // Generate all combinations of button presses
    int options = problem.buttons.size();
    std::vector<std::vector<int>> sequences;
    for (int i = 0; i < options; i++)
    {
        sequences.emplace_back(std::vector<int>{i});
    }

    while (!problem.is_done())
    {
        for (const std::vector<int>& sequence : sequences) {
            if (problem.check_sequence_part_2(sequence)) {
                return sequence.size();
            }
        }

        sequences = extend_sequences(options, sequences);
    }

    return 0;
}

long part1(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    long sum = 0;
    int counter = 0;
    for (std::string line : lines)
    {
        long solution = solve_problem_part_1(line);
        sum += solution;
        std::cout << "Solved " << counter << "/" << lines.size() << ", solution: " << solution << std::endl;
        counter++;
    }
    return sum;
}

long part2(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    long sum = 0;
    int counter = 0;
    for (std::string line : lines)
    {
        long solution = solve_problem_part_2(line);
        sum += solution;
        std::cout << "Solved " << counter << "/" << lines.size() << ", solution: " << solution << std::endl;
        counter++;
    }
    return sum;}

void execute_part(std::function<long(std::string)> part_x, std::string file, int part_number)
{
    long answer_part_1 = part_x(file);
    std::cout << "Answer of part " << part_number << ": " << answer_part_1 << std::endl;
}

int main()
{
    execute_part(part1, "day10test.txt", 1);
    // Note: have to guess whether left or right is the outside of the circuit
    execute_part(part2, "day10test.txt", 2);
    return 0;
}
