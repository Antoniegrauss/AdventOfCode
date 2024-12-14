from grid import Grid
from grid import Pathfinding
from grid import Path
from grid import unique_positions

def main():
    # Test BFS
    grids = open("test_grids.txt").read().split("\n\n")
    testgrid  = Grid(grids[0])
    start = testgrid.find_character("S")[0]
    end = testgrid.find_character("E")[0]
    assert(start == (1, 13))
    assert(end == (13, 1))
    
    
    pathfinder = Pathfinding(testgrid)
    is_valid = lambda char : char == "." or char == "E"
    
    assert(Path([(0, 0)]).add_step((1, 0)).add_step((2, 0)).cost == 2)
    assert(Path([(0, 0)]).add_step((1, 0)).add_step((1, 1)).cost == 1002)
    paths = pathfinder.from_a_t_b_depth_first(start=start, end=end, is_valid_function=is_valid)
    print(paths)

    # Test day 18 dijksta
    test_pathfinder_1 = Pathfinding(Grid(grids[0]))
    start = test_pathfinder_1.grid.find_character("S")[0]
    end = test_pathfinder_1.grid.find_character("E")[0]
    path, cost = test_pathfinder_1.dijkstra_with_cost(start=start, end=end, 
                                                 is_valid_function=is_valid)
    test_pathfinder_1.grid.print_path(path)
    assert(unique_positions(path)==45)
    print(unique_positions(path))
    assert(cost == 7036)
    
    test_pathfinder_2 = Pathfinding(Grid(grids[1]))
    start = test_pathfinder_2.grid.find_character("S")[0]
    end = test_pathfinder_2.grid.find_character("E")[0]
    path, cost = test_pathfinder_2.dijkstra_with_cost(start=start, end=end, 
                                                 is_valid_function=is_valid)
    test_pathfinder_2.grid.print_path(unique_positions(path))
    assert(unique_positions(path) == 64)
    print(unique_positions(path) )
    assert(cost == 11048)
        
    test_pathfinder_3 = Pathfinding(Grid(grids[2]))
    start = test_pathfinder_3.grid.find_character("S")[0]
    end = test_pathfinder_3.grid.find_character("E")[0]
    path, cost = test_pathfinder_3.dijkstra_with_cost(start=start, end=end, 
                                                 is_valid_function=is_valid)
    test_pathfinder_3.grid.print_path(unique_positions(path))
    assert(unique_positions(path)==149)
    print(unique_positions(path))
    assert(cost == 21148)
    
    test_pathfinder_5 = Pathfinding(Grid(grids[4]))
    start = test_pathfinder_5.grid.find_character("S")[0]
    end = test_pathfinder_5.grid.find_character("E")[0]
    path, cost = test_pathfinder_5.dijkstra_with_cost(start=start, end=end, 
                                                 is_valid_function=is_valid)
    # test_pathfinder_5.grid.print_path(unique_positions(path))
    print(unique_positions(path))
    print(cost)
    assert(cost == 4013)
    
    test_pathfinder_4 = Pathfinding(Grid(grids[3]))
    start = test_pathfinder_4.grid.find_character("S")[0]
    end = test_pathfinder_4.grid.find_character("E")[0]
    path, cost = test_pathfinder_4.dijkstra_with_cost(start=start, end=end, 
                                                 is_valid_function=is_valid)
    test_pathfinder_4.grid.print_path(unique_positions(path))
    assert(unique_positions(path)==264)
    print(unique_positions(path))
    assert(cost == 21110)
if __name__ == "__main__":
    main()