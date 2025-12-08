#include <vector>
#include <string>

namespace AoC::utils {
    std::vector<std::string> read_input_file(const std::string& filename);

    std::vector<std::string> split_by_delimiter(const std::string& original, char delimiter);
}

