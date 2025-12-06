#include <iostream>
#include <string>
#include <vector>
#include <stdexcept>
#include <functional>
#include <cctype>

#include "read_file.hpp"

std::string parse_next_token(std::string &line);
long apply_operator(const std::string &op, const std::vector<long> &numbers);

long perform_math_problem(std::vector<std::string> &lines)
{
    std::string op = parse_next_token(lines.back());

    std::vector<long> numbers = {};
    for (auto it = lines.begin(); it != lines.end() - 1; it++)
    {
        numbers.emplace_back(std::stol(parse_next_token(*it)));
    }

    return apply_operator(op, numbers);
}

long perform_math_problem_part_2(std::vector<std::string> &lines)
{
    // Reverse all lines
    for (std::string &line : lines)
    {
        std::reverse(line.begin(), line.end());
    }

    long total_math = 0;
    long current_number = 0;
    std::vector<long> numbers = {};
    std::string op = "";
    for (int i = 0; i < lines[0].size(); i++)
    {
        long current_number = 0;
        for (auto it = lines.begin(); it != lines.end(); it++)
        {
            char symbol = (*it)[i];
            if (symbol == ' ') {
                continue;
            }
            if (std::isdigit(symbol)) {
                current_number *= 10;
                current_number += symbol - '0';
                continue;
            }
            op += symbol;
        }
        if (current_number != 0) {
            numbers.emplace_back(current_number);
            current_number = 0;
        }
        if (op != "") {        
            // We encountered an operator
            // This means we parsed the entire math problem
            // Add it to the sum and reset the variables
            total_math += apply_operator(op, numbers);
            op = "";
            numbers.clear();
        }
    }
    return total_math;
}

long apply_operator(const std::string &op, const std::vector<long> &numbers)
{
    if (op == "+")
    {
        long sum = 0;
        for (long number : numbers)
        {
            sum += number;
        }
        return sum;
    }

    if (op == "*")
    {
        long product = 1;
        for (long number : numbers)
        {
            product *= number;
        }
        return product;
    }

    std::cout << "Operator invalid: " << op << std::endl;

    return -1;
}

std::string parse_next_token(std::string &line)
{
    std::string token;

    bool started_token = false;
    int erase_counter = 0;
    for (char c : line)
    {
        erase_counter += 1;
        if (c == ' ')
        {
            if (started_token)
            {
                break;
            }
            continue;
        }
        started_token = true;
        token += c;
    }

    line = line.substr(erase_counter, line.size() - erase_counter);
    return token;
}

long part1(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    long sum = 0;
    while (true)
    {
        sum += perform_math_problem(lines);
        if (lines[0] == "")
        {
            break;
        }
    }
    return sum;
}

long part2(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    return perform_math_problem_part_2(lines);
}

void execute_part(std::function<long(std::string)> part_x, std::string file, int part_number)
{
    long answer_part_1 = part_x(file);
    std::cout << "Answer of part " << part_number << ": " << answer_part_1 << std::endl;
}

int main()
{
    execute_part(part1, "day6.txt", 1);
    execute_part(part2, "day6.txt", 2);
    return 0;
}
