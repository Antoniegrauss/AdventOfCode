#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <vector>
#include <filesystem>

#include "read_file.hpp"

int main() {
    auto lines = AoC::utils::read_input_file("day1.txt");

    for (const std::string& line : lines) {
        std::cout << line << std::endl;
    }
}
