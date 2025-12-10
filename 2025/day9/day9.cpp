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

enum class Direction
{
    Up,
    Down,
    Left,
    Right
};

Direction opposite(Direction direction)
{
    switch (direction)
    {
    case Direction::Up:
        return Direction::Down;
    case Direction::Down:
        return Direction::Up;
    case Direction::Left:
        return Direction::Right;
    case Direction::Right:
        return Direction::Left;
    }
}

Direction relative_right(Direction direction)
{
    switch (direction)
    {
    case Direction::Up:
        return Direction::Right;
    case Direction::Down:
        return Direction::Left;
    case Direction::Left:
        return Direction::Up;
    case Direction::Right:
        return Direction::Down;
    }
}

Direction relative_left(Direction direction)
{
    return opposite(relative_right(direction));
}

struct Coordinate
{
    long x, y;

    Coordinate(std::string line)
    {
        auto parts = AoC::utils::split_by_delimiter(line, ',');
        x = std::stoi(parts[0]);
        y = std::stoi(parts[1]);
    }

    Coordinate(long x, long y) : x(x), y(y) {}

    Coordinate operator+(const Coordinate &other) const
    {
        return Coordinate{x + other.x, y + other.y};
    }

    Coordinate operator-(const Coordinate &other)
    {
        return Coordinate{x - other.x, y - other.y};
    }

    Coordinate move_in_direction(Direction direction) const;
};

static Coordinate UP{0, -1};
static Coordinate DOWN{0, 1};
static Coordinate LEFT{-1, 0};
static Coordinate RIGHT{1, 0};

Coordinate Coordinate::move_in_direction(Direction direction) const
{
    switch (direction)
    {
    case Direction::Up:
        return *this + UP;
    case Direction::Down:
        return *this + DOWN;
    case Direction::Left:
        return *this + LEFT;
    case Direction::Right:
        return *this + RIGHT;
    }
}

Direction find_direction(const Coordinate &start, const Coordinate &end)
{
    if (start.x == end.x)
    {
        if (end.y > start.y)
            return Direction::Up;
        if (start.y > end.y)
            return Direction::Down;
        // Is same coord!
        assert(false);
    }
    if (start.y == end.y)
    {
        if (end.x > start.x)
            return Direction::Right;
        if (end.x < start.x)
            return Direction::Left;
        // Is same coord
        assert(false);
    }
    // Is not on same x or same y
    assert(false);
}

// If it is not on the right, left, up or down of rectangle it must be inside
bool is_inside_rectange(const Coordinate &to_check, const Coordinate &corner_one, const Coordinate &corner_two)
{
    if (to_check.x > corner_one.x && to_check.x > corner_two.x)
        return false;
    if (to_check.x < corner_one.x && to_check.x < corner_two.x)
        return false;
    if (to_check.y > corner_one.y && to_check.y > corner_two.y)
        return false;
    if (to_check.y < corner_one.y && to_check.y < corner_two.y)
        return false;
    return true;
}

long calculate_rectangle_area(const Coordinate &one, const Coordinate &other)
{
    // Note the rectangle is inclusive of the border on all sides
    return static_cast<long>(abs(one.x - other.x) + 1) *
           static_cast<long>(abs(one.y - other.y) + 1);
}

std::vector<Coordinate> parse_input(const std::vector<std::string> &input)
{
    std::vector<Coordinate> parsed = {};
    for (const std::string &line : input)
    {
        parsed.emplace_back(Coordinate(line));
    }
    return parsed;
}

long part1(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    auto coordinates = parse_input(lines);
    long max_area = 0;
    for (int i = 0; i < coordinates.size(); i++)
    {
        for (int j = i + 1; j < coordinates.size(); j++)
        {
            long rectangle_area = calculate_rectangle_area(coordinates[i], coordinates[j]);
            if (rectangle_area > max_area)
            {
                max_area = rectangle_area;
            }
        }
    }

    return max_area;
}

std::vector<Coordinate> find_outside_coords(const std::vector<Coordinate> &coords, bool right_side, bool left_side)
{
    std::vector<Coordinate> outside_coords = {};
    for (int i = 0; i < coords.size() - 1; i++)
    {
        Direction line_direction = find_direction(coords[i], coords[i + 1]);
        Coordinate first_of_line = coords[i].move_in_direction(line_direction);
        if (right_side)
        {
            outside_coords.emplace_back(first_of_line.move_in_direction(relative_right(line_direction)));
        }
        if (left_side)
        {
            outside_coords.emplace_back(first_of_line.move_in_direction(relative_left(line_direction)));
        }
    }
    return outside_coords;
}

long part2(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    auto coordinates = parse_input(lines);

    // Run along the lines,                   v
    // for each line, take the first step -> #XXXXXX#
    // Add the coord to the left as inside and to the right as outside
    //     O (outside)
    //    #XXXXXXXX#
    //     I (inside)
    bool right_is_outside = true;
    bool left_is_outside = false;
    auto outside_coords = find_outside_coords(coordinates, right_is_outside, left_is_outside);

    // For each rectangle coordinate pair check whether any O coords are inside
    // if so, reject this one
    long max_area = 0;
    for (int i = 0; i < coordinates.size(); i++)
    {
        for (int j = i + 1; j < coordinates.size(); j++)
        {
            bool valid_rectangle = true;
            for (const Coordinate& outside_coord : outside_coords) {
                if (is_inside_rectange(outside_coord, coordinates[i], coordinates[j])) {
                    valid_rectangle = false;
                    break;
                }
            }
            if (!valid_rectangle) continue;

            long rectangle_area = calculate_rectangle_area(coordinates[i], coordinates[j]);
            if (rectangle_area > max_area)
            {
                max_area = rectangle_area;
            }
        }
    }

    return 0;
}

void execute_part(std::function<long(std::string)> part_x, std::string file, int part_number)
{
    long answer_part_1 = part_x(file);
    std::cout << "Answer of part " << part_number << ": " << answer_part_1 << std::endl;
}

int main()
{
    // 4777678192 is too low
    execute_part(part1, "day9.txt", 1);
    execute_part(part2, "day9test.txt", 2);
    return 0;
}
