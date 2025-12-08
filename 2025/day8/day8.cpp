#include <iostream>
#include <string>
#include <vector>
#include <stdexcept>
#include <functional>
#include <cctype>
#include <set>
#include <cassert>
#include <cmath>

#include "read_file.hpp"

struct Coordinate
{
    int x, y, z;

    long distance_squared_to(const Coordinate &other) const
    {
        return (other.x - x) * (other.x - x) +
               (other.y - y) * (other.y - y) +
               (other.z - z) * (other.z - z);
    }

    Coordinate(std::string line) {
        auto parts = AoC::utils::split_by_delimiter(line, ',');
        x = std::stoi(parts[0]);
        y = std::stoi(parts[1]);
        z = std::stoi(parts[2]);
    }
};

long part1(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    std::vector<Coordinate> coords = {};
    for (std::string line : lines) {
        coords.emplace_back(Coordinate(line));
    }
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
    execute_part(part1, "day8.txt", 1);
    execute_part(part2, "day8.txt", 2);
    return 0;
}
