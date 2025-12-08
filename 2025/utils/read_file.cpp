#include <iostream>
#include <fstream>
#include <sstream>
#include <filesystem>

#include "read_file.hpp"

namespace AoC::utils
{

    std::vector<std::string> read_lines(std::istream &file);

    std::ifstream open_file(const std::string &filename)
    {
        auto path = std::filesystem::current_path();
        path = path.parent_path();
        path.append("input");
        path.append(filename);

        std::cout << "Reading file: " << path.c_str() << std::endl;

        std::ifstream input_file(path.c_str());
        return input_file;
    }

    std::vector<std::string> read_lines(std::istream &file)
    {
        std::vector<std::string> lines = {};

        std::string line;
        while (std::getline(file, line))
        {
            // std::cout << line << std::endl;
            if (line.length() == 0)
            {
                continue;
            }
            lines.emplace_back(line);
        }
        return lines;
    }

    std::vector<std::string> read_input_file(const std::string &filename)
    {
        std::ifstream file(open_file(filename));
        return read_lines(file);
    }

    std::vector<std::string> split_by_delimiter(const std::string &original, char delimiter)
    {
        std::vector<std::string> parts = {};

        std::string copy = original;
        while (copy.find(delimiter) != std::string::npos) {
            auto delimiter_pos = copy.find(delimiter);
            parts.emplace_back(copy.substr(0, delimiter_pos));
            copy = copy.substr(delimiter_pos + 1, copy.size() - delimiter_pos);
        }
        // Add the final bit to the vector
        parts.emplace_back(copy);
        return parts;
    }
}
