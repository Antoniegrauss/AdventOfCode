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

struct Coordinate;
using Network = std::set<Coordinate>;
using Networks = std::vector<Network>;
std::vector<int> network_sizes(const Networks &networks);
std::vector<int> network_ids(const Network &network);
struct Coordinate
{
    int x, y, z;
    int id;
    std::vector<int> connections;

    long distance_squared_to(const Coordinate &other) const
    {
        return ((long)other.x - (long)x) * ((long)other.x - (long)x) +
               ((long)other.y - (long)y) * ((long)other.y - (long)y) +
               ((long)other.z - (long)z) * ((long)other.z - (long)z);
    }

    Coordinate(std::string line)
    {
        static int id_counter = 0;
        id = id_counter;
        id_counter++;
        auto parts = AoC::utils::split_by_delimiter(line, ',');
        x = std::stoi(parts[0]);
        y = std::stoi(parts[1]);
        z = std::stoi(parts[2]);
    }

    void add_connection(int connection) {
        connections.emplace_back(connection);
    }

    friend bool operator==(const Coordinate &one, const Coordinate &other) noexcept
    {
        return one.x == other.x &&
               one.y == other.y &&
               one.z == other.z;
    }

    bool operator<(Coordinate other) const
    {
        if (x != other.x)
        {
            return x < other.x;
        }
        if (y != other.y)
        {
            return y < other.y;
        }
        if (z != other.z)
        {
            return z < other.z;
        }
        return false;
    }
};

struct Pair
{
    int one, other;
    long distance_sq;

    Pair(Coordinate one, Coordinate other) : one(one.id), other(other.id)
    {
        distance_sq = one.distance_squared_to(other);
    }

    friend auto operator<=>(const Pair &one, const Pair &other) noexcept
    {
        return one.distance_sq <=> other.distance_sq;
    }

    friend bool operator==(const Pair &one, const Pair &other) noexcept
    {
        return one.one == other.one &&
               one.other == other.other;
    }
};

std::string ids_of_network(const Network &network)
{
    std::stringstream ss;
    auto ids = network_ids(network);
    for (int id : ids)
    {
        ss << id << ",";
    }
    return ss.str();
}

std::vector<int> network_sizes(const Networks &networks)
{
    std::vector<int> network_sizes;
    std::transform(networks.begin(), networks.end(),
                   std::back_inserter(network_sizes),
                   [](const Network &net)
                   {
                       return net.size();
                   });
    return network_sizes;
}

std::vector<int> network_ids(const Network &network)
{
    std::vector<int> network_ids;
    std::transform(network.begin(), network.end(),
                   std::back_inserter(network_ids),
                   [](const Coordinate &coord)
                   {
                       return coord.id;
                   });
    std::sort(network_ids.begin(), network_ids.end());
    return network_ids;
}

int multiply_largest_3_network_sizes(std::vector<int> &sizes)
{
    std::sort(sizes.rbegin(), sizes.rend());
    return std::accumulate(sizes.begin(), sizes.begin() + 3,
                           1, std::multiplies<int>());
}

void connect_pair(const Pair &pair, std::vector<Coordinate>& coords)
{
    coords[pair.one].add_connection(pair.other);
    coords[pair.other].add_connection(pair.one);
}

std::vector<Pair> generate_pairs(std::vector<Coordinate> &coords)
{
    std::vector<Pair> pairs;
    for (int i = 0; i < coords.size(); i++)
    {
        for (int j = i + 1; j < coords.size(); j++)
        {
            pairs.emplace_back(Pair(coords[i], coords[j]));
        }
    }
    return pairs;
}

std::vector<int> bfs_find_network_sizes(const std::vector<Coordinate>& coords) {
    std::set<int> visited;
    std::vector<int> network_sizes;

    for (int i = 0; i < coords.size(); i++) {
        if (visited.find(i) != visited.end()) continue;

        visited.insert(i);
        int network_size_counter = 1;
        std::queue<int> queue;
        queue.push(coords[i].id);
        while (!queue.empty()) {
            int next = queue.front();
            queue.pop();
            for (int connection : coords[next].connections) {
                if (visited.find(connection) == visited.end()) {
                    queue.push(connection);
                    network_size_counter ++;
                    visited.insert(connection);
                }
            }
        }
        network_sizes.emplace_back(network_size_counter);
    }
    return network_sizes;
}

long part1(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    std::vector<Coordinate> coords = {};
    for (std::string line : lines)
    {
        coords.emplace_back(Coordinate(line));
    }

    // Generate all pairs
    std::vector<Pair> pairs = generate_pairs(coords);

    // Sort by distance squared
    std::sort(pairs.begin(), pairs.end());

    // Connect the first 1000 pairs
    for (int i = 0; i < 1000; i++)
    {
        connect_pair(pairs[i], coords);
    }

    // BFS to find network sizes
    auto network_sizes = bfs_find_network_sizes(coords);

    // Find the 3 larges network sizes combined
    return multiply_largest_3_network_sizes(network_sizes);
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
    // 33348 is too low
    // 79560 should be correct
    execute_part(part1, "day8.txt", 1);
    execute_part(part2, "day8.txt", 2);
    return 0;
}
