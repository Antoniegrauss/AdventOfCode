#include <iostream>
#include <fstream>
#include <sstream>
#include <filesystem>

#include "read_file.hpp"

std::vector<std::string> read_lines(std::istream& file);

std::ifstream open_file(const std::string& filename) {
    auto path = std::filesystem::current_path();
    path = path.parent_path();
    path.append("input");
    path.append(filename);

    std::cout << "Reading file: " << path.c_str() << std::endl;

    std::ifstream input_file(path.c_str());
    return input_file;
}

std::vector<std::string> read_lines(std::istream& file) {
    std::vector<std::string> lines = {};

    std::string line;
    while (std::getline(file, line))
    {
        // std::cout << line << std::endl;
        if (line.length() == 0) {
            continue;
        }
        lines.emplace_back(line);
    }
    return lines;
}

std::vector<std::string> read_input_file(const std::string& filename) {
    std::ifstream file(open_file(filename));
    return read_lines(file);
}
