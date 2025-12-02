#include <iostream>
#include <string>
#include <vector>
#include <stdexcept>
#include <functional>
#include <numeric>
#include <limits>

#include "read_file.hpp"

bool is_id_valid(long id) {
    std::string id_str = std::to_string(id);
    if (id_str.size() % 2 != 0) {
        return true;
    }

    int i = 0;
    for (int j = id_str.size() / 2; j < id_str.size(); j++) {
        if (id_str[i] != id_str[j]) {
            return true;
        }
        i++;
    }

    return false;
}

bool is_repeating_with_length(const std::string& input, int pattern_length) {
    if (input.size() % pattern_length != 0) {
        return false;
    }
    
    std::string first_chunk = input.substr(0, pattern_length);
    for (int chunk_id = 1; chunk_id < input.size() / pattern_length; chunk_id++) {
        if (first_chunk != input.substr(chunk_id * pattern_length, pattern_length)) {
            // This chunk not same as other chunk
            return false;
        }
    }
    return true;
}

bool is_id_valid_part2(long id) {
    std::string id_str = std::to_string(id);
    if (id_str.size() % 2 != 0) {
        return true;
    }

    // Check repeating patterns
    for (int pattern_length = 1; pattern_length < id_str.size() / 2; pattern_length++) {
        if (is_repeating_with_length(id_str, pattern_length)) {
            return false;
        }
    }

    return false;
}

struct Range {
    long lower_bound;
    long upper_bound;

    long sum_of_invalid_ids() const {
        long sum = 0;
        for (long i = lower_bound; i <= upper_bound; i++) {
            if (! is_id_valid(i)) {
                sum += i;
            }
        }
        return sum;
    }

    long sum_of_invalid_ids_part2() const {
        long sum = 0;
        for (long i = lower_bound; i <= upper_bound; i++) {
            if (! is_id_valid_part2(i)) {
                sum += i;
            }
        }
        return sum;
    }
};

Range parse_range(std::string input) {
    Range range;
    auto delimiter_pos = input.find('-');
    range.lower_bound = std::stol(input.substr(0,delimiter_pos));
    range.upper_bound = std::stol(input.substr(delimiter_pos + 1, input.length() - delimiter_pos));

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
    ranges.emplace_back(parse_range(current));

    return ranges;
}

long part1(std::string filename)
{
    auto ranges = parse_input(AoC::utils::read_input_file(filename)[0]);
    long total_invalid_ids = 0;
    for (const Range& range : ranges) {
        total_invalid_ids += range.sum_of_invalid_ids();
    }
    return total_invalid_ids;
}

long part2(std::string filename)
{
    auto ranges = parse_input(AoC::utils::read_input_file(filename)[0]);
    long total_invalid_ids = 0;
    for (const Range& range : ranges) {
        total_invalid_ids += range.sum_of_invalid_ids_part2();
    }
    return total_invalid_ids;
}

void execute_part(std::function<long(std::string)> part_x, std::string file, int part_number)
{
    long answer_part_1 = part_x(file);
    std::cout << "Answer of part " << part_number << ": " << answer_part_1 << std::endl;
}

int main()
{
    execute_part(part1, "day2.txt", 1);
    // 1043802083128342 is too high
    execute_part(part2, "day2test.txt", 2);
    return 0;
}
