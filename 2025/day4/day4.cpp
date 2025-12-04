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

long part1(std::string filename)
{
    auto grid = AoC::utils::Grid(AoC::utils::read_input_file(filename));
    return -1;
}

long part2(std::string filename)
{
    return -1;
}

void execute_part(std::function<long(std::string)> part_x, std::string file, int part_number)
{
    long answer_part_1 = part_x(file);
    std::cout << "Answer of part " << part_number << ": " << answer_part_1 << std::endl;
}

int main()
{
    execute_part(part1, "day4.txt", 1);
    execute_part(part2, "day4.txt", 2);
    return 0;
}
