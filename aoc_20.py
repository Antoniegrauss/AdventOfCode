import grid as g
import networkx as nx
import copy

def check_cheat_possible_with_wall(pathfinder, start, end, wall, is_valid):
    pathfinder.grid.set(wall, ".")
    path = pathfinder.from_a_t_b_depth_first(start, end, is_valid, shortest_only=True, bfs=True)
    pathfinder.grid.set(wall, "#")
    return len(path)-1

def try_cheat(step, id, path, grid, cheat_time_allowed, min_timesave):
    # Do a BFS to see what parts of the path further we can reach within the cheat_time_allowed
    cheat_times = []
    
    visited = set()
    visited.add(step)
    
    steps = [step]
    cheat_times = 0
    for i in range(cheat_time_allowed):
        new_steps = []
        for step in steps:
            for neighbour in g.get_neighbours(step):
                if not grid.is_in_bounds(neighbour):
                    continue
                if neighbour in visited:
                    continue
                visited.add(neighbour)
                
                # If we find a neighbour that is in the path, we can add the cheat time
                if neighbour in path:
                    new_steps.append(neighbour)
                    time_win = path.index(neighbour) - id - (i + 1)
                    if time_win >= min_timesave:
                        cheat_times += 1
                # Add only walls to the search
                elif grid.at(neighbour) == "#":
                    new_steps.append(neighbour)
        # Reset the steps to check
        steps = copy.deepcopy(new_steps)
    return cheat_times

def solution_bfs(grid, start, end, is_valid, cheat_time_allowed, threshold):
    graph = g.network_x_graph(grid, is_valid)
    normal_path = nx.shortest_path(graph, start, end)

    cheat_times = 0
    counter = 0
    for id, step in enumerate(normal_path):
        if counter % 10 == 0:
            print(f"{counter} of {len(normal_path)}, shortcuts: {cheat_times}")
        cheat_times += try_cheat(step, id, normal_path, grid, cheat_time_allowed, min_timesave=threshold)
        counter += 1

    return cheat_times
    
def part1_nx(grid, start, end, is_valid):
    graph = g.network_x_graph(grid, is_valid)
    normal_path = nx.shortest_path(graph, start, end)
    
    grid.print_path(normal_path)
    
    cheat_paths = []
    counter = 0
    total_walls = len(grid.find_character("#"))
    walls = [wall_x for wall_x in grid.find_character("#") if not grid.is_border_position(wall_x)]
    print(f"total walls: {total_walls}, without border: {len(walls)}")
          
    for wall in walls:
        g.network_x_insert_node(graph, grid, wall[0], wall[1], is_valid)
        cheat_path = nx.shortest_path(graph, start, end)
        if len(cheat_path) < len(normal_path):
            cheat_len = len(normal_path) - len(cheat_path)
            cheat_paths.append(cheat_len)
        graph.remove_node(wall)
        counter += 1
        if counter % 100 == 0:
            print(f"{counter} of {len(walls)}, shortcuts: {len(cheat_paths)}")
        
    print(len([cheat for cheat in cheat_paths if cheat >= 100]))

def main():
    print("day 20")
    
    grid = g.grid_factory_str(open("aoc_24/input/Day20.txt").read())
    start = grid.find_character("S")[0]
    end = grid.find_character("E")[0]
    
    is_valid = lambda x: x == "." or x == "E" or x == "S"
    
    # part1_nx(grid, start, end, is_valid)
    # assert(solution_bfs(grid, start, end, is_valid, cheat_time_allowed=2, threshold=100)==1311)
    
    # Test for part 2 should be 32+31+29+39+25+23+20+19+12+14+22+4+3
    # cheat_times = solution_bfs(grid, start, end, is_valid, cheat_time_allowed=20, threshold=50)
    # assert(cheat_times == 285)
    
    print(solution_bfs(grid, start, end, is_valid, cheat_time_allowed=20, threshold=100))

if __name__ == "__main__":
    main()