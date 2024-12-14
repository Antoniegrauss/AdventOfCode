from grid import Grid
from grid import Pathfinding
from grid import unique_positions, grid_factory_str

def main():
    print("Day 16")
    is_valid = lambda char : char == "." or char == "E"

    test_pathfinder_2 = Pathfinding(grid_factory_str(open("aoc_24/input/Day16.txt").read()))
    start = test_pathfinder_2.grid.find_character("S")[0]
    end = test_pathfinder_2.grid.find_character("E")[0]
    path, cost = test_pathfinder_2.dijkstra_with_cost(start=start, end=end, 
                                                 is_valid_function=is_valid)
    print(path, cost)
    test_pathfinder_2.grid.print_multiple_paths(path)
    print(unique_positions(path))

if __name__== "__main__":
    main()