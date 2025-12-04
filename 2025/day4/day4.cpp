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

using Coordinate2D = AoC::utils::Coordinate2D;
using Grid = AoC::utils::Grid;

bool valid_paper_roll_part_1(const Coordinate2D &coord, const Grid &grid);
int remove_valid_paper_rolls(Grid& grid);

long part1(std::string filename)
{
    auto grid = Grid(AoC::utils::read_input_file(filename));
    auto all_coords = grid.find_all('@');

    int valid_counter = 0;
    for (const AoC::utils::Cell &cell : all_coords)
    {
        if (valid_paper_roll_part_1(cell.coord, grid))
        {
            valid_counter++;
        }
    }
    return valid_counter;
}

bool valid_paper_roll_part_1(const Coordinate2D &coord, const Grid &grid)
{
    char invalid_char = '@';
    auto neighbours = grid.get_neighbours(coord, AoC::utils::NeighbourType::AdjacentAndDiagonal);
    auto count = std::count_if(neighbours.begin(), neighbours.end(), [invalid_char](const AoC::utils::Cell &cell)
                               { return cell.content == invalid_char; });
    return count < 4;
}

long part2(std::string filename)
{
    auto grid = Grid(AoC::utils::read_input_file(filename));
    
    int total_counter = 0;
    int valid_counter = remove_valid_paper_rolls(grid);
    while (valid_counter > 0) {
        total_counter += valid_counter;
        valid_counter = remove_valid_paper_rolls(grid);
    }
    
    return total_counter;
}

int remove_valid_paper_rolls(Grid& grid)
{
    auto all_coords = grid.find_all('@');
    int valid_counter = 0;
    std::vector<Coordinate2D> valid_coords;
    for (const AoC::utils::Cell &cell : all_coords)
    {
        if (valid_paper_roll_part_1(cell.coord, grid))
        {
            valid_coords.emplace_back(cell.coord);
            valid_counter++;
        }
    }

    for (const Coordinate2D &coord : valid_coords)
    {
        grid.set_cell(coord, '.');
    }

    return valid_counter;
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
