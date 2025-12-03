#include <iostream>
#include <string>
#include <vector>
#include <stdexcept>
#include <functional>
#include <numeric>
#include <limits>
#include <algorithm>

#include "read_file.hpp"

struct BatteryBank
{
    std::vector<int> digits;

    BatteryBank(const std::string &line)
    {
        for (const char &c : line)
        {
            digits.emplace_back(c - '0');
        }
    }

    int max_jolts() const
    {
        auto largest_digit_pos = std::max_element(digits.begin(), digits.end() - 1);
        int first_digit = *largest_digit_pos;

        int second_digit = *std::max_element(largest_digit_pos + 1, digits.end());
        return first_digit * 10 + second_digit;
    }

    long max_jolts_with_override() const
    {
        std::vector<int>::const_iterator last_digit_index = digits.begin();
        long total_jolts = 0;

        for (int digits_to_find = 11; digits_to_find >= 0; digits_to_find--)
        {
            auto largest_digit_pos =
                std::max_element(last_digit_index, digits.end() - digits_to_find);
            last_digit_index = largest_digit_pos + 1;
            total_jolts = total_jolts * 10 +  *largest_digit_pos;
        }
        return total_jolts;
    }
};

std::vector<BatteryBank> parse_input(const std::vector<std::string> &lines)
{
    std::vector<BatteryBank> batteries = {};
    std::transform(lines.begin(), lines.end(), std::back_inserter(batteries), [](const std::string &line)
                   { return BatteryBank(line); });
    return batteries;
}

long part1(std::string filename)
{
    auto battery_banks = parse_input(AoC::utils::read_input_file(filename));
    long total_jolts = 0;
    for (const BatteryBank &battery_bank : battery_banks)
    {
        total_jolts += battery_bank.max_jolts();
    }
    return total_jolts;
}

long part2(std::string filename)
{
    auto battery_banks = parse_input(AoC::utils::read_input_file(filename));
    long total_jolts = 0;
    for (const BatteryBank &battery_bank : battery_banks)
    {
        total_jolts += battery_bank.max_jolts_with_override();
    }
    return total_jolts;
}

void execute_part(std::function<long(std::string)> part_x, std::string file, int part_number)
{
    long answer_part_1 = part_x(file);
    std::cout << "Answer of part " << part_number << ": " << answer_part_1 << std::endl;
}

int main()
{
    execute_part(part1, "day3.txt", 1);
    execute_part(part2, "day3.txt", 2);
    return 0;
}
