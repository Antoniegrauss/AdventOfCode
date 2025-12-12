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
#include <array>
#include <bitset>

#include "read_file.hpp"

struct Point
{
    int x, y;

    int chessboard_distance_to_origin() const
    {
        return std::max(abs(x), abs(y));
    }

    int chessboard_distance_to(const Point &other) const
    {
        return std::max(abs(x - other.x), abs(y - other.y));
    }
};

enum class Direction
{
    Original,
    Right90,
    Left90,
    Opposite
};

enum class Side
{
    Top,
    Right,
    Bottom,
    Left,
    MiddleHorizontal,
    MiddleVertical
};

Side opposite(Side side)
{
    switch (side)
    {
    case Side::Top:
        return Side::Bottom;
    case Side::Bottom:
        return Side::Top;
    case Side::Right:
        return Side::Left;
    case Side::Left:
        return Side::Right;
    case Side::MiddleHorizontal:
        return Side::MiddleHorizontal;
    case Side::MiddleVertical:
        return Side::MiddleVertical;
    }
    assert(false);
}

Side rotate_right(Side side)
{
    switch (side)
    {
    case Side::Top:
        return Side::Right;
    case Side::Right:
        return Side::Bottom;
    case Side::Bottom:
        return Side::Left;
    case Side::Left:
        return Side::Top;
    case Side::MiddleHorizontal:
        return Side::MiddleVertical;
    case Side::MiddleVertical:
        return Side::MiddleHorizontal;
    }
    assert(false);
}

Side rotate_left(Side side)
{
    return opposite(rotate_right(side));
}

Side rotated_side(Side side, Direction rotation)
{
    switch (rotation)
    {
    case Direction::Original:
        return side;
    case Direction::Right90:
        return rotate_right(side);
    case Direction::Left90:
        return rotate_left(side);
    case Direction::Opposite:
        return opposite(side);
    }
    assert(false);
}

struct Block
{
    int id;
    int num_gaps;
    int num_blocks;

    std::bitset<3> top;
    std::bitset<3> right;
    std::bitset<3> bottom;
    std::bitset<3> left;
    std::bitset<3> middle_vertical;
    std::bitset<3> middle_horizontal;

    std::array<bool, 9> occupied_spaces;

    Block(const std::vector<std::string> &lines)
    {
        id = lines[0][0] - '0';
        // Store in the format:
        // 0 1 2
        // 3 4 5
        // 6 7 8
        // Store lines as 8 bit integer
        occupied_spaces[0] = lines[1][0] == '#';
        occupied_spaces[1] = lines[1][1] == '#';
        occupied_spaces[2] = lines[1][2] == '#';
        occupied_spaces[3] = lines[2][0] == '#';
        occupied_spaces[4] = lines[2][1] == '#';
        occupied_spaces[5] = lines[2][2] == '#';
        occupied_spaces[6] = lines[3][0] == '#';
        occupied_spaces[7] = lines[3][1] == '#';
        occupied_spaces[8] = lines[3][2] == '#';

        num_gaps = std::count(occupied_spaces.begin(), occupied_spaces.end(), false);
        num_blocks = 9 - num_gaps;

        calculate_sides();
    }

    void calculate_sides()
    {
        // Horizontals
        top = occupied_spaces[0] << 2 | occupied_spaces[1] << 1 | occupied_spaces[2];
        middle_horizontal = occupied_spaces[3] << 2 | occupied_spaces[4] << 1 | occupied_spaces[5];
        bottom = occupied_spaces[6] << 2 | occupied_spaces[7] << 1 | occupied_spaces[8];

        // Verticals
        left = occupied_spaces[0] << 2 | occupied_spaces[3] << 1 | occupied_spaces[6];
        middle_vertical = occupied_spaces[1] << 2 | occupied_spaces[4] << 1 | occupied_spaces[7];
        right = occupied_spaces[2] << 2 | occupied_spaces[5] << 1 | occupied_spaces[8];
    }
};

struct PlacedBlock
{
    int block_id;
    Direction orientation;
    Point position;
};

struct SpaceUnderTree
{
    int width;
    int height;

    std::vector<int> total_blocks;
    std::vector<PlacedBlock> placed_blocks;

    SpaceUnderTree(const std::string &line)
    {
        auto parts = AoC::utils::split_by_delimiter(line, ':');
        auto size = AoC::utils::split_by_delimiter(parts[0], 'x');
        width = stoi(size[0]);
        height = stoi(size[1]);

        auto blocks = AoC::utils::split_by_delimiter(parts[1], ' ');
        for (std::string block : blocks)
        {
            if (block.size() == 0)
                continue;
            total_blocks.emplace_back(stoi(block));
        }
    }

    // TODO: implement
    bool is_possible_to_place(const std::vector<int> &block_amounts)
    {
        return false;
    }
};

Block parse_block(const std::vector<std::string> &lines, int block_id)
{
    const int lines_per_block = 4;
    int start_line = lines_per_block * block_id;

    std::vector<std::string> block_lines;
    for (int i = start_line; i < start_line + lines_per_block; i++)
    {
        block_lines.emplace_back(lines[i]);
    }

    return Block(block_lines);
}

long part1(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    // We parse 6 blocks, then we parse each row as SpaceUnderTree
    std::vector<Block> blocks;
    for (int i = 0; i < 6; i++)
    {
        blocks.emplace_back(parse_block(lines, i));
    }

    std::vector<SpaceUnderTree> spaces_under_tree;
    const int tree_space_start = 24;
    for (int i = tree_space_start; i < lines.size(); i++)
    {
        spaces_under_tree.emplace_back(SpaceUnderTree(lines[i]));
    }

    // Pre-calculate how all blocks fit together

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
    // 500 is too high
    execute_part(part1, "day12test.txt", 1);
    execute_part(part2, "day12.txt", 2);
    return 0;
}
