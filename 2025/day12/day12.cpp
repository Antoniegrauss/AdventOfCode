#include <iostream>
#include <string>
#include <vector>
#include <stdexcept>
#include <functional>
#include <cctype>
#include <set>
#include <map>
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

    bool operator==(const Point &other) const
    {
        return x == other.x && y == other.y;
    }
};

bool collides(const std::bitset<3> &one, const std::bitset<3> &other)
{
    auto copy = one;
    copy &= other;
    return copy != 0;
}

enum class Rotation
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

Side rotated_side(Side side, Rotation rotation)
{
    switch (rotation)
    {
    case Rotation::Original:
        return side;
    case Rotation::Right90:
        return rotate_right(side);
    case Rotation::Left90:
        return rotate_left(side);
    case Rotation::Opposite:
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

    bool middle_occupied() const
    {
        return occupied_spaces[4];
    }

    std::bitset<3> get_side_rotated(Side side, Rotation rotation) const
    {
        switch (rotation)
        {
        case Rotation::Original:
            return get_side(side);
        case Rotation::Opposite:
            // If opposite always swap the order of the bits -> 123 to 321
            return mirror(get_side(opposite(side)));
        case Rotation::Right90:
            // With right rotation, the horizontal lines do not need to be mirrored
            if (side == Side::Top || side == Side::MiddleHorizontal || side == Side::Bottom)
            {
                return get_side(rotate_right(side));
            }
            else
            {
                return mirror(get_side(rotate_right(side)));
            }
        case Rotation::Left90:
            // With left rotation, the vertical lines do not need to be mirrored
            if (side == Side::Left || side == Side::MiddleVertical || side == Side::Right)
            {
                return get_side(rotate_right(side));
            }
            else
            {
                return mirror(get_side(rotate_right(side)));
            }
        }
    }

    std::bitset<3> get_side(Side side) const
    {
        switch (side)
        {
        case Side::Top:
            return top;
        case Side::MiddleHorizontal:
            return middle_horizontal;
        case Side::Bottom:
            return bottom;
        case Side::Left:
            return left;
        case Side::MiddleVertical:
            return middle_vertical;
        case Side::Right:
            return right;
        }
        assert(false);
    }

    std::bitset<3> mirror(std::bitset<3> original) const
    {
        static const int bitset_size = 3;
        std::bitset<bitset_size> reversed;
        for (int i = 0, j = bitset_size - 1; i < bitset_size; i++, j--)
        {
            reversed[j] = original[i];
        }
        return reversed;
    }
};

struct PlacedBlock
{
    int block_id;
    Rotation orientation;
    Point position;

    // For sorting disregard the position
    bool operator<(const PlacedBlock& other) const {
        if (block_id != other.block_id) {
            return block_id < other.block_id;
        }
        return orientation < other.orientation;
    }

    bool collides_with(const PlacedBlock &other, const std::vector<Block> &all_blocks) const
    {
        int distance = position.chessboard_distance_to(other.position);
        // If 3x3 squares do not overlap, do not have to check anything else
        if (distance > 2)
            return false;

        // No blocks can have same center point
        if (distance == 0)
            return true;

        Side touching_side = where_is_other_block(other.position);

        std::bitset<3> this_block_touching = all_blocks[block_id].get_side_rotated(touching_side, orientation);
        std::bitset<3> other_block_touching = all_blocks[other.block_id].get_side_rotated(opposite(touching_side), other.orientation);
        if (distance == 2)
        {
            return collides(this_block_touching, other_block_touching);
        }

        // If distance 1, also have to test the middle row/cols
        if (distance == 1)
        {
            // Overlapping on 2 rows/cols
            Side middle;
            if (touching_side == Side::Top || touching_side == Side::Bottom)
            {
                middle = Side::MiddleHorizontal;
            }
            middle = Side::MiddleVertical;

            std::bitset<3> this_block_middle = all_blocks[block_id].get_side_rotated(middle, orientation);
            std::bitset<3> other_block_middle = all_blocks[other.block_id].get_side_rotated(opposite(middle), other.orientation);

            // Touching side ---- other.middle
            if (collides(this_block_touching, other_block_middle))
                return true;

            // middle -----------other.touching side
            return collides(this_block_middle, other_block_touching);
        }

        // Distance is negative??
        assert(false);
    }

    Side where_is_other_block(const Point &other) const
    {
        assert(!(position == other));

        if (abs(position.x - other.x) >= abs(position.y - other.y))
        {
            // Horizontal distance is largest (or equal)
            if (other.x > position.x)
                return Side::Right;
            return Side::Left;
        }
        if (other.y > position.y)
            return Side::Bottom;
        return Side::Top;
    }
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

    bool is_possible_to_place(const std::vector<Block>& blocks)
    {
        // Simple start, check whether there is even enough space
        int total_space = width * height;
        int block_pins = 0;
        for (int i = 0; i < blocks.size(); i ++) {
            block_pins += blocks[i].num_blocks * total_blocks[i];
        }
        return total_space >= block_pins;
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

std::vector<PlacedBlock> generate_blocks(const std::vector<int> &x,
                                         const std::vector<int> &y,
                                         const std::vector<Rotation> &rotations,
                                         const std::vector<int> &block_ids)
{
    std::vector<PlacedBlock> blocks;
    for (int x : x)
    {
        for (int y : y)
        {
            for (Rotation rot : rotations)
            {
                for (int block_id : block_ids)
                {
                    blocks.emplace_back(PlacedBlock{block_id, rot, Point{x, y}});
                }
            }
        }
    }
    return blocks;
}

auto pre_calculate_possible_placements(const std::vector<Block>& blocks) {
    static const Point ORIGIN{0, 0};
    static const std::vector<int> ALL_BLOCK_IDS = {0, 1, 2, 3, 4, 5};
    static const std::vector<Rotation> ALL_ROTATIONS = {Rotation::Original, Rotation::Right90,
                                                        Rotation::Left90, Rotation::Opposite};
    static const std::vector<int> BLOCK_RELATIVE_OFFSETS = {-2, -1, 0, 1, 2};

    std::map<PlacedBlock, std::vector<PlacedBlock>> possible_neighbours_per_block;
    for (const PlacedBlock &this_block : generate_blocks({0},
                                                         {0},
                                                         ALL_ROTATIONS,
                                                         ALL_BLOCK_IDS))
    {
        std::vector<PlacedBlock> possible_neighbours;
        for (const PlacedBlock &other_block : generate_blocks(BLOCK_RELATIVE_OFFSETS,
                                                              BLOCK_RELATIVE_OFFSETS,
                                                              ALL_ROTATIONS,
                                                              ALL_BLOCK_IDS))
        {
            if (!this_block.collides_with(other_block, blocks))
            {
                possible_neighbours.emplace_back(other_block);
            }
        }
        possible_neighbours_per_block.insert({this_block, possible_neighbours});
    }

    return possible_neighbours_per_block;
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

    // _______ Turns out puzzle input is a joke, is triavially solvable______________
    // Pre-calculate how all blocks fit together
    // std::map<PlacedBlock, std::vector<PlacedBlock>> possible_neighbours_per_block;

    long total_possible = 0;
    for (SpaceUnderTree space_under_tree : spaces_under_tree) {
        if (space_under_tree.is_possible_to_place(blocks)) {
            total_possible += 1;
        }
    }

    return total_possible;
}

void execute_part(std::function<long(std::string)> part_x, std::string file, int part_number)
{
    long answer_part_1 = part_x(file);
    std::cout << "Answer of part " << part_number << ": " << answer_part_1 << std::endl;
}

int main()
{
    execute_part(part1, "day12.txt", 1);
    return 0;
}
