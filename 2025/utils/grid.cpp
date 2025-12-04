#include <vector>
#include <string>

#include "grid.hpp"

namespace AoC::utils {
    std::vector<Coordinate2D> Coordinate2D::get_neighbours(NeighbourType neighbour_type) {
        if (neighbour_type == NeighbourType::Adjacent) {
            return get_neighbours_adjacent();
        }
        return get_neighbours_all_8();
    }

    std::vector<Coordinate2D> Coordinate2D::get_neighbours_adjacent() {
        std::vector<Coordinate2D> neighbours = {};
        std::vector<int> offsets = {-1, 1};
        for (int x_offset : offsets) {
            neighbours.push_back(Coordinate2D(x + x_offset, y));
        }
        for (int y_offset : offsets) {
            neighbours.push_back(Coordinate2D(x, y + y_offset));
        }
        return neighbours;
    }

    std::vector<Coordinate2D> Coordinate2D::get_neighbours_all_8() {
        std::vector<Coordinate2D> neighbours = {};
        // Loop over 3x3 grid centered on this coord
        std::vector<int> offsets = {-1, 0, 1};
        for (int x_offset : offsets) {
            for (int y_offset : offsets) {
                // Skip the center tile
                if (x_offset == 0 && y_offset == 0) {
                    continue;
                }
                neighbours.push_back(Coordinate2D(x + x_offset, y + y_offset));
            }
        }
        return neighbours;
    }

    Grid::Grid(std::vector<std::string> lines) {
        for (int y = 0; y < lines.size(); y++) {
            std::vector<Cell> row = {};
            for (int x = 0; x < lines[y].size(); x++) {
                row.emplace_back(Cell(lines[y][x], Coordinate2D(x, y)));
            }
            cells.emplace_back(row);
        }
    }

    std::vector<Cell> Grid::find_all(char match) const {
        std::vector<Cell> matches = {};
        for (const std::vector<Cell> row : cells) {
            for (Cell cell : row) {
                if (cell.content == match) {
                    matches.emplace_back(cell);
                }
            }
        }
        return matches;
    }
}
