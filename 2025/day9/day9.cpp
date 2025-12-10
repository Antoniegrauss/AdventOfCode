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

long part1(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    return 0;
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
    execute_part(part1, "day9.txt", 1);
    execute_part(part2, "day9.txt", 2);
    return 0;
}
