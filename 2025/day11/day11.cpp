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
#include <execution>

#include "read_file.hpp"

struct Node
{
    std::string name;
    std::vector<std::string> connections;

    Node(const std::string &line)
    {
        for (char c : line)
        {
            if (c == ':')
                break;
            name += c;
        }

        // Name size is 5
        int connection_start = 5;
        auto parts = AoC::utils::split_by_delimiter(
            line.substr(5, line.size() - 5), ' ');
        for (std::string part : parts)
        {
            std::string connection = "";
            for (char c : part)
            {
                if (c != ' ')
                    connection += c;
            }
            connections.emplace_back(connection);
        }
    }
};

struct Graph
{
    std::unordered_map<std::string, Node> nodes;

    Graph(const std::vector<std::string> &lines)
    {
        for (const std::string &line : lines)
        {
            Node node(line);
            nodes.insert({node.name, node});
        }
    }
};

struct Path
{
    std::string current_node;
    int multiplicity = 1;

    bool operator==(const std::string& node_name) const {
        return current_node == node_name;
    }

    bool operator<(const Path& other) const {
        return current_node < other.current_node;
    }
};

std::vector<Path> extend_paths(const std::vector<Path> &paths, const Graph &graph, long &paths_to_end, const std::string &end)
{
    std::vector<Path> new_paths;
    for (const Path &path : paths)
    {
        for (const std::string &connection : graph.nodes.at(path.current_node).connections)
        {
            if (connection == end)
            {
                paths_to_end += path.multiplicity;
                continue;
            }
            if (connection == "out") continue;

            // If we already have a path to this node, add 1 to the path multiplicity
            auto path_already_exists = std::find(new_paths.begin(), new_paths.end(), connection);
            if (path_already_exists != new_paths.end()) {
                path_already_exists->multiplicity += path.multiplicity;
                continue;
            }
            new_paths.emplace_back(Path{connection, path.multiplicity});
        }
    }
    return new_paths;
}

long count_paths_from_to(std::string start_node, const Graph &graph, const std::string &end_node)
{
    std::cout << "Counting paths from " << start_node << " to " << end_node << std::endl;
    std::vector<Path> paths{Path{start_node, 1}};
    long found_end_counter = 0;

    int step_counter = 0;
    while (!paths.empty())
    {
        paths = extend_paths(paths, graph, found_end_counter, end_node);
        step_counter++;
    }

    return found_end_counter;
}

long part1(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    Graph graph(lines);
    std::string start_node = "you";
    std::string end_node = "out";
    return count_paths_from_to(start_node, graph, end_node);
}

long part2(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    Graph graph(lines);
    std::string start_node = "svr";

    // Path from 1 svr -> dac, 2 svr -> fft
    // From 3 dac -> fft, 4 fft -> dac
    // From 5 dac -> out, 6 fft -> out
    // Total = 1 * 3 * 5 + 2 * 4 * 6
    return (count_paths_from_to("svr", graph, "dac") *
            count_paths_from_to("dac", graph, "fft") *
            count_paths_from_to("fft", graph, "out")) +

           (count_paths_from_to("svr", graph, "fft") *
            count_paths_from_to("fft", graph, "dac") *
            count_paths_from_to("dac", graph, "out"));
}

void execute_part(std::function<long(std::string)> part_x, std::string file, int part_number)
{
    long answer_part_1 = part_x(file);
    std::cout << "Answer of part " << part_number << ": " << answer_part_1 << std::endl;
}

int main()
{
    // 649 is correct
    execute_part(part1, "day11.txt", 1);
    // 22500 is too low
    execute_part(part2, "day11.txt", 2);
    return 0;
}
