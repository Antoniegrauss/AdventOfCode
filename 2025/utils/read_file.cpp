#include <iostream>
#include <fstream>
#include <sstream>
#include <filesystem>

#include "read_file.hpp"

std::vector<std::string> read_lines_from_file(std::istream& file);

std::ifstream open_input_file(const std::string& filename) {
    auto path = std::filesystem::current_path();
    path.append("..");
    path.append("input");
    path.append(filename);

    std::ifstream input_file(path.c_str());
    auto lines = read_lines_from_file(input_file);
    return input_file;
}

std::vector<std::string> read_lines_from_file(std::istream& file) {
    std::vector<std::string> lines = {};

    std::string line;
    while (std::getline(file, line))
    {
        if (line.length() == 0) {
            continue;
        }
        lines.emplace_back(line);
    }
    return lines;
}

std::vector<std::string> read_input_file(const std::string& filename) {
    std::ifstream file(open_input_file(filename));
    return read_lines_from_file(file);
}
