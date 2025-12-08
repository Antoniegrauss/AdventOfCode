#include <iostream>
#include <string>
#include <vector>
#include <stdexcept>
#include <functional>
#include <cctype>
#include <set>
#include <cassert>
#include <algorithm>
#include <cmath>
#include <numeric>

#include "read_file.hpp"

struct Coordinate;
using Network = std::set<Coordinate>;
using Networks = std::vector<Network>;
struct Coordinate
{
    int x, y, z;

    long distance_squared_to(const Coordinate &other) const
    {
        return (other.x - x) * (other.x - x) +
               (other.y - y) * (other.y - y) +
               (other.z - z) * (other.z - z);
    }

    Coordinate(std::string line)
    {
        auto parts = AoC::utils::split_by_delimiter(line, ',');
        x = std::stoi(parts[0]);
        y = std::stoi(parts[1]);
        z = std::stoi(parts[2]);
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
    Coordinate one, other;
    long distance_sq;

    Pair(Coordinate one, Coordinate other) : one(one), other(other)
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

Networks merge_networks(const Networks &networks)
{
    Networks all_merged = {};
    std::vector<int> to_skip = {};

    for (int i = 0; i < networks.size(); i++)
    {
        Network current = networks[i];
        // If this network is already swallowed up, skip it
        auto should_skip = std::find(to_skip.begin(), to_skip.end(), i);
        if (should_skip != to_skip.end())
            continue;
        for (int j = i + 1; j < networks.size(); j++)
        {
            Network next = networks[j];
            Network intersection;
            std::set_intersection(
                current.begin(), current.end(),
                next.begin(), next.end(),
                std::inserter(intersection, intersection.begin()));
            if (!intersection.empty())
            {
                // Insert coord from next into current
                for (const Coordinate &coord : next)
                {
                    current.insert(coord);
                }
                to_skip.emplace_back(j);
            }
        }
        all_merged.emplace_back(current);
    }
    return all_merged;
}

int multiply_largest_3_network_sizes(const Networks &networks)
{
    std::vector<int> network_sizes;
    std::transform(networks.begin(), networks.end(),
                   std::back_inserter(network_sizes),
                   [](const Network &net)
                   {
                       return net.size();
                   });

    std::sort(network_sizes.rbegin(), network_sizes.rend());
    return std::accumulate(network_sizes.begin(), network_sizes.begin() + 3,
                           1, std::multiplies<int>());
}

Networks connect_pair(Networks &networks, const Pair &pair)
{
    bool found = false;
    for (Network &network : networks)
    {
        if (found)
            break;
        if (network.find(pair.one) != network.end())
        {
            network.insert(pair.other);
            found = true;
            continue;
        }
        if (network.find(pair.other) != network.end())
        {
            network.insert(pair.one);
            found = true;
            continue;
        }
    }
    // If both coordinates are not in a network yet, add them
    if (!found)
    {
        networks.emplace_back(Network{pair.one, pair.other});
    }

    networks = merge_networks(networks);
    return networks;
}

std::vector<Pair> generate_pairs(const std::vector<Coordinate> &coords)
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
    Networks networks;
    for (int i = 0; i < 1000; i++)
    {
        networks = connect_pair(networks, pairs[i]);
    }

    for (const Network &network : networks)
    {
        std::cout << "Size of network: " << network.size() << std::endl;
    }

    // Find the 3 larges network sizes combined
    return multiply_largest_3_network_sizes(networks);
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
    execute_part(part1, "day8.txt", 1);
    execute_part(part2, "day8.txt", 2);
    return 0;
}
