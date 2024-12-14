from grid import Grid, Pathfinding, grid_factory_sizes, grid_factory_str

def rain_bits(num_bytes, grid, falling_bytes):
    for byte in falling_bytes[:num_bytes]:
        grid.set((byte[0], byte[1]), "#")

def find_shortest_path(grid):
    start = (0, 0)
    end = (70, 70)
    is_valid = lambda char : char == "."
    return Pathfinding(grid).from_a_t_b_depth_first(start, end, is_valid, shortest_only=True)

def create_grid():
    grid_width = 71
    grid_height = 71
    grid = grid_factory_sizes(grid_width, grid_height, ".")
    
    return grid

def create_bytes():
    return [[int(x) for x in line.strip("\n").split(",")] for line in open("aoc_24/input/Day18.txt").readlines()]

def do_part_1():
    grid = create_grid()
    falling_bytes = create_bytes()
    rain_bits(2988, grid, falling_bytes)
        
    shortest_path = find_shortest_path(grid)
    print(shortest_path)
    grid.print_path(shortest_path)
    print(len(shortest_path) - 1)
    
def do_part_2():
    # Do divide and conquer to find the time when no path is possible anymore
    falling_bytes = create_bytes()
    
    step = int(len(falling_bytes) / 2)
    rain_num = step
    while step > 0:
        grid = create_grid()
        rain_bits(rain_num, grid, falling_bytes)
        if find_shortest_path(grid):
            rain_num += step
            print(f"Found path at num {rain_num}")
        else:
            rain_num -= step
            print(f"Found no path at num {rain_num}")
        step = int(step / 2)
        print(f"Step is {step}")
        
    return rain_num
    
def test_part_1():
    is_valid = lambda char : char == "."
    pathfinder = Pathfinding(grid_factory_str(open("aoc_24/input/Day18_test.txt").read()))
    start = (0, 0)
    end = (6, 6)
    shortest_path = pathfinder.from_a_t_b_depth_first(start, end, is_valid, shortest_only=True, bfs=True)
    
    pathfinder.grid.print_path(shortest_path)
    return (len(shortest_path) - 1)

def main():
    print("day 18")
    
    assert(test_part_1() == 22)
    
    do_part_1()
    print(f"Part 2 answer: {do_part_2()}")

if __name__== "__main__":
    main()