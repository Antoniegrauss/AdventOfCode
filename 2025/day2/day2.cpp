#include <iostream>
#include <string>
#include <vector>
#include <stdexcept>
#include <functional>

#include "read_file.hpp"

struct Range {
    int lower_bound;
    int upper_bound;
};

Range parse_range(std::string input) {
    Range range;
    auto delimiter_pos = input.find('-');
    range.lower_bound = std::stoi(input.substr(0,delimiter_pos));
    range.upper_bound = std::stoi(input.substr(delimiter_pos + 1, input.length() - delimiter_pos));

    return range;
}

std::vector<Range> parse_input(const std::string& line) {
    std::vector<Range> ranges = {};
    std::string unparsed_range = "";

    std::string current = "";
    for (const char c : line) {
        if (c == ',') {
            ranges.emplace_back(parse_range(current));
            current = "";
        } else {
            current += c;
        }
    }
    return ranges;
}

int part1(std::string filename)
{
    auto ranges = parse_input(AoC::utils::read_input_file(filename)[0]);
    return -1;
}

int part2(std::string filename)
{
    return -1;
}

void execute_part(std::function<int(std::string)> part_x, std::string file, int part_number)
{
    int answer_part_1 = part_x(file);
    std::cout << "Answer of part " << part_number << ": " << answer_part_1 << std::endl;
}

int main()
{
    execute_part(part1, "day2.txt", 1);
    execute_part(part2, "day2.txt", 2);
}
