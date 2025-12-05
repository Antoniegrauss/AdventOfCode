#include <iostream>
#include <string>
#include <vector>
#include <stdexcept>
#include <functional>
#include <numeric>
#include <limits>
#include <algorithm>

#include "read_file.hpp"
#include "grid.hpp"

struct Range {
    long start;
    long end;

    Range(std::string line) {
        // Should be in the form "123-456"
        auto delimiter_pos = line.find('-');
        start = stol(line.substr(0, delimiter_pos));
        end = stol(line.substr(delimiter_pos + 1, line.size() - delimiter_pos));
    }

    bool within_range(long product) const {
        return product >= start && product <= end;
    }
};

bool check_produce_within_ranges(const std::vector<Range>& ranges, long product) {
    for (const Range& range : ranges) {
        if (! range.within_range(product)) {
            return false;
        }
    }
    return true;
}

long part1(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    std::vector<Range> ranges = {};
    std::vector<long> produce = {};
    for (const std::string& line : lines) {
        if (line.size() == 0) {
            continue;
        }
        if (line.find('-') != std::string::npos) {
            ranges.emplace_back(Range(line));
        } else {
            produce.emplace_back(stol(line));
        }
    }

    auto answer = std::count_if(produce.begin(), produce.end(), [&ranges](long product) {
        return check_produce_within_ranges(ranges, product);
    });

    return answer;
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
    execute_part(part1, "day5.txt", 1);
    execute_part(part2, "day5.txt", 2);
    return 0;
}
