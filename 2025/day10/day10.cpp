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
#include <execution>

#include "read_file.hpp"

enum class Result
{
    Failed,
    Succeeded,
    None
};

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

        // Trim first and last character from string
        parts.back() = parts.back().substr(1, parts.back().size() - 2);
        auto joltages = AoC::utils::split_by_delimiter(parts.back(), ',');
        for (std::string joltage : joltages)
        {
            bool skip = false;
            for (char c : joltage)
            {
                if (!isdigit(c))
                    skip = true;
            }
            if (skip)
                continue;
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
            current_joltage[joltage_id]--;
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

    Result check_sequence_part_2(const std::vector<int> &button_ids)
    {
        reset_joltage();
        for (int button_id : button_ids)
        {
            press_button_part_2(button_id);
        }
        if (is_done_part_2())
        {
            return Result::Succeeded;
        }
        if (part2_failed())
        {
            return Result::Failed;
        }
        return Result::None;
    }

    bool part2_failed() const
    {
        return std::any_of(current_joltage.begin(), current_joltage.end(), [](int joltage)
                           { return joltage < 0; });
    }

    bool is_done() const
    {
        return lights == answer;
    }

    bool is_done_part_2() const
    {
        return std::all_of(current_joltage.begin(), current_joltage.end(),
                           [](int joltage)
                           { return joltage == 0; });
    }

    void reset()
    {
        std::fill(lights.begin(), lights.end(), false);
    }

    void reset_joltage()
    {
        current_joltage.clear();
        std::copy(joltage_answer.begin(), joltage_answer.end(), std::back_inserter(current_joltage));
    }

    int light_with_least_buttons() const
    {
        std::vector<int> counts;
        for (const bool light : lights)
        {
            counts.emplace_back(0);
        }

        for (const std::vector<int> &button : buttons)
        {
            for (int connection : button)
            {
                counts[connection] += 1;
            }
        }

        // Return the id of the light that is connected to the least amount of buttons
        return std::distance(std::begin(counts), std::min_element(std::begin(counts), std::end(counts)));
    }

    std::vector<int> buttons_connected_to(int light_id) const
    {
        std::vector<int> button_ids;
        for (int i = 0; i < buttons.size(); i++)
        {
            if (std::find(buttons[i].begin(), buttons[i].end(), light_id) != buttons[i].end())
            {
                button_ids.emplace_back(i);
            }
        }
        return button_ids;
    }

    std::vector<int> buttons_not_connected_to(int light_id) const
    {
        std::vector<int> button_ids;
        for (int i = 0; i < buttons.size(); i++)
        {
            if (std::find(buttons[i].begin(), buttons[i].end(), light_id) == buttons[i].end())
            {
                button_ids.emplace_back(i);
            }
        }
        return button_ids;
    }
};

std::set<std::vector<int>> extend_sequences(int options,
                                            const std::set<std::vector<int>> &sequences)
{
    std::set<std::vector<int>> new_sequences;
    for (int i = 0; i < options; i++)
    {
        for (std::vector<int> sequence : sequences)
        {
            sequence.emplace_back(i);
            std::sort(sequence.begin(), sequence.end());
            new_sequences.insert(sequence);
        }
    }
    return new_sequences;
}

std::set<std::vector<int>> extend_sequences(std::vector<int> options,
                                            const std::set<std::vector<int>> &sequences)
{
    std::set<std::vector<int>> new_sequences;
    for (int option : options)
    {
        for (std::vector<int> sequence : sequences)
        {
            sequence.emplace_back(option);
            std::sort(sequence.begin(), sequence.end());
            new_sequences.insert(sequence);
        }
    }
    return new_sequences;
}

std::set<std::vector<int>> prune_sequences(Problem &problem, const std::set<std::vector<int>> &sequences)
{
    std::set<std::vector<int>> pruned;
    std::copy_if(std::execution::par, sequences.begin(), sequences.end(), std::insert_iterator(pruned, pruned.begin()),
                 [&problem](const std::vector<int> &sequence)
                 {
                     return problem.check_sequence_part_2(sequence) != Result::Failed;
                 });
    return pruned;
}

// Returns the starting sequences and possible buttons
std::set<std::vector<int>> initial_partial_solve(const Problem &problem, int light_to_solve_for)
{
    int light_joltage = problem.joltage_answer[light_to_solve_for];

    std::vector<int> using_buttons = problem.buttons_connected_to(light_to_solve_for);
    if (using_buttons.size() == 1)
    {
        std::vector<int> only_sequence;
        for (int i = 0; i < light_joltage; i++)
        {
            only_sequence.emplace_back(using_buttons[0]);
        }
        return {only_sequence};
    }

    std::set<std::vector<int>> partial_solve;
    // If multiple buttons, have to add all combinations
    for (int i = 0; i < light_joltage; i++)
    {
        if (partial_solve.empty())
        {
            for (int button_id : using_buttons)
            {
                partial_solve.insert({button_id});
            }
            continue;
        }
        partial_solve = extend_sequences(using_buttons, partial_solve);
    }
    return partial_solve;
}

long solve_problem_part_1(std::string line)
{
    Problem problem(line);

    long button_presses = 0;

    // Generate all combinations of button presses
    int options = problem.buttons.size();
    std::set<std::vector<int>> sequences;
    for (int i = 0; i < options; i++)
    {
        sequences.insert(std::vector<int>{i});
    }

    while (!problem.is_done())
    {
        for (const std::vector<int> &sequence : sequences)
        {
            if (problem.check_sequence(sequence))
            {
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

    // Possible optimization: focus on lights that are connected to the fewest buttons
    // In case only 1 button is connected, this button press amount is known
    // In case 2-3 buttons are connected, add all possible combinations as sequences
    int least_connected_light = problem.light_with_least_buttons();
    auto sequences = initial_partial_solve(problem, least_connected_light);
    auto options = problem.buttons_not_connected_to(least_connected_light);

    int presses = sequences.begin()->size();
    while (!problem.is_done())
    {
        std::cout << "Presses: " << presses << ", num sequences: " << sequences.size() << std::endl;
        if (std::execution::par, std::any_of(sequences.begin(), sequences.end(), [&problem](const std::vector<int> &sequence)
                                             { return problem.check_sequence_part_2(sequence) == Result::Succeeded; }))
        {
            // Sequences should all have the same length
            // Just return the length of the first one if we have a hit
            return presses;
        }

        sequences = prune_sequences(problem, sequences);
        sequences = extend_sequences(options, sequences);
        presses++;
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
    return sum;
}

void execute_part(std::function<long(std::string)> part_x, std::string file, int part_number)
{
    long answer_part_1 = part_x(file);
    std::cout << "Answer of part " << part_number << ": " << answer_part_1 << std::endl;
}

int main()
{
    execute_part(part1, "day10test.txt", 1);
    // Solution [42, 46, ]
    execute_part(part2, "day10test.txt", 2);
    return 0;
}
