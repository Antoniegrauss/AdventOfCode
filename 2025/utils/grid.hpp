#include <vector>
#include <string>

namespace AoC::utils {
    enum class NeighbourType
    {
        Adjacent,
        AdjacentAndDiagonal
    };

    struct Coordinate2D
    {
        int x;
        int y;

        std::vector<Coordinate2D> get_neighbours(NeighbourType neighbour_type);
        Coordinate2D(int x, int y) : x(x), y(y) {}

    private:
        std::vector<Coordinate2D> get_neighbours_adjacent();

        std::vector<Coordinate2D> get_neighbours_all_8();
    };

struct Cell {
    char content;
    Coordinate2D coord;

    Cell(char content, Coordinate2D coord) : content(content), coord(coord) {}
};

class Grid
    {

    public:
        explicit Grid(std::vector<std::string> lines);

        bool is_inside_grid(Coordinate2D coord) const {
            return coord.x > 0 && coord.x < get_width() &&
                coord.y > 0 && coord.y < get_height();
        }

        Cell get_cell(Coordinate2D coord) const
        {
            // Note the y coord is indexed first
            return cells[coord.y][coord.x];
        }

        std::vector<Cell> get_neighbours(Coordinate2D coord, NeighbourType neighbour_type) const
        {
            std::vector<Cell> neighbour_cells = {};
            for (Coordinate2D coord : coord.get_neighbours(neighbour_type)) {
                neighbour_cells.emplace_back(get_cell(coord));
            }
            return neighbour_cells;
        }

        std::vector<Cell> find_all(char match) const;

        int get_width() const
        {
            return cells[0].size();
        }

        int get_height() const
        {
            return cells.size();
        }

    private:
        std::vector<std::vector<Cell>> cells;
    };
}
