#include <iostream>
#include <string>
#include <vector>
#include <stdexcept>
#include <functional>

#include "read_file.hpp"

std::string parse_next_token(std::string& line);

long perform_math_problem(std::vector<std::string>& lines) {
    std::string op = parse_next_token(lines.back());

    std::vector<long> numbers = {};
    for (auto it = lines.begin(); it != lines.end() - 1; it++) {
        numbers.emplace_back(std::stol(parse_next_token(*it)));
    }

    if (op == "+") {
        long sum = 0;
        for (long number : numbers) {
            sum += number;
        }
        return sum;
    }

    if (op == "*") {
        long product = 1;
        for (long number : numbers) {
            product *= number;
        }
        return product;
    }

    std::cout << "Operator invalid: " << op << std::endl;

    return -1;
}

std::string parse_next_token(std::string& line) {
    const char* SPACE = " ";
    auto pos = line.find_first_not_of(SPACE);
    if (pos == std::string::npos) {
        line = "";
        return "";
    }
    std::string token = line.substr(0, pos);
    // Erase this part of the string
    line = line.substr(pos + 1, line.size() - pos);
    return token;
}

long part1(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    long sum = 0;
    while (true) {
        sum += perform_math_problem(lines);
        if (lines[0] == "") {
            break;
        }
    }
    return -1;
}

long part2(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    return -1;
}

void execute_part(std::function<long(std::string)> part_x, std::string file, int part_number)
{
    long answer_part_1 = part_x(file);
    std::cout << "Answer of part " << part_number << ": " << answer_part_1 << std::endl;
}

int main()
{
    execute_part(part1, "day5.txt", 1);
    execute_part(part2, "day5.txt", 2);
    return 0;
}
