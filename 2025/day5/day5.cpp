#include <iostream>
#include <string>
#include <vector>
#include <stdexcept>
#include <functional>
#include <numeric>
#include <limits>
#include <algorithm>
#include <tuple>
#include <queue>

#include "read_file.hpp"
#include "grid.hpp"

struct Range;
Range merge_range_or_not(std::vector<Range> ranges, Range range);

struct Range
{
    long start;
    long end;

    Range(std::string line)
    {
        // Should be in the form "123-456"
        auto delimiter_pos = line.find('-');
        start = stol(line.substr(0, delimiter_pos));
        end = stol(line.substr(delimiter_pos + 1, line.size() - delimiter_pos));
    }

    bool within_range(long product) const
    {
        return product >= start && product <= end;
    }

    // Range is inclusive on both ends
    long size() const
    {
        return end - start + 1;
    }

    void merge_with_other(const Range &other)
    {
        start = std::min(start, other.start);
        end = std::max(end, other.end);
    }

    bool overlaps_with(const Range &other) const
    {
        return (start >= other.start && start <= other.end) ||
               (end >= other.start && end <= other.end);
    }

    bool operator==(Range &other) const
    {
        return start == other.start && end == other.end;
    }

    bool operator<(Range other) const
    {
        return start < other.start;
    }
};

bool check_produce_within_ranges(const std::vector<Range> &ranges, long product)
{
    for (const Range &range : ranges)
    {
        if (range.within_range(product))
        {
            return true;
        }
    }
    return false;
}

std::tuple<std::vector<Range>, std::vector<long>> parse_input(const std::vector<std::string> &lines)
{
    std::vector<Range> ranges = {};
    std::vector<long> produce = {};
    for (const std::string &line : lines)
    {
        if (line.size() == 0)
        {
            continue;
        }
        if (line.find('-') != std::string::npos)
        {
            ranges.emplace_back(Range(line));
        }
        else
        {
            produce.emplace_back(stol(line));
        }
    }
    return std::make_tuple(ranges, produce);
}

long part1(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    auto parsed_input = parse_input(lines);
    std::vector<long> produce = std::get<1>(parsed_input);
    std::vector<Range> ranges = std::get<0>(parsed_input);
    auto answer = std::count_if(produce.begin(), produce.end(), [&ranges](long product)
                                { return check_produce_within_ranges(ranges, product); });

    return answer;
}

std::vector<Range> merge_ranges(std::vector<Range> original_ranges)
{
    std::vector<Range> merged_ranges = {};
    for (Range range : original_ranges)
    {
        Range changed_range = merge_range_or_not(original_ranges, range);
        if (std::find(merged_ranges.begin(), merged_ranges.end(), changed_range) == merged_ranges.end()) {
            merged_ranges.emplace_back(changed_range);
        }
    }
    return merged_ranges;
}

Range merge_range_or_not(std::vector<Range> ranges, Range range)
{
    for (Range other_range : ranges)
    {
        if (other_range == range)
        {
            continue;
        }
        if (range.overlaps_with(other_range))
        {
            range.merge_with_other(other_range);
            break;
        }
    }
    return range;
}

long part2(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    auto parsed_input = parse_input(lines);
    std::vector<Range> ranges = std::get<0>(parsed_input);
    std::sort(ranges.begin(), ranges.end(), [](Range a, Range b) {
        return a.start > b.start;
    });

    // While loop runs until no more ranges are merged
    int prev_range_num = ranges.size();
    int new_ranges_num = 0;
    do
    {
        if (new_ranges_num != 0)
        {
            prev_range_num = new_ranges_num;
        }
        auto ranges_as_set = merge_ranges(ranges);
        ranges = std::vector(ranges_as_set.begin(), ranges_as_set.end());

        new_ranges_num = ranges.size();

    } while (new_ranges_num < prev_range_num);

    long sum = 0;
    for (const Range &range : ranges)
    {
        sum += range.size();
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
    execute_part(part1, "day5.txt", 1);
    execute_part(part2, "day5.txt", 2);
    return 0;
}
