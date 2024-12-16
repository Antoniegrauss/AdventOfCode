import copy

# Gets adjacent neighbours for a position
def get_neighbours(position):
    return [
        (position[0] + 1, position[1]),
        (position[0] - 1, position[1]),
        (position[0], position[1] + 1),
        (position[0], position[1] - 1),
    ]
    
def get_direction(position_from, position_to):
    # Should be adjacent tiles
    assert(abs(position_from[0] - position_to[0]) + \
        abs(position_from[1] - position_to[1]) == 1)
    
    if position_from[0] > position_to[0]:
        return 0 #"west"
    elif position_from[0] < position_to[0]:
        return 1 #"east"
    elif position_from[1] > position_to[1]:
        return 2 #"north"
    elif position_from[1] < position_to[1]:
        return 3 #"south"

def position_distance(position_1, position_2):
    return abs(position_1[0] - position_2[0]) + \
        abs(position_1[1] - position_2[1])

class Grid():
    def __init__(self, input: str) -> None:
        self.cells = [line.strip("\n") for line in input.split("\n")]
        self.width = len(self.cells[0])
        self.height = len(self.cells)
    
    def is_in_bounds(self, position):
        return position[0] >= 0 and \
            position[1] >= 0 and \
            position[0] < self.width and \
            position[1] < self.height
            
    def find_character(self, character):
        correct = []
        for y, row in enumerate(self.cells):
            for x, cell in enumerate(row):
                if cell == character:
                    correct.append((x, y))
        return correct
    
    def at(self, position):
        return self.cells[position[1]][position[0]]
        
    def print_path(self, path):
        grid_copy = self.cells
        for y, row in enumerate(grid_copy):
            print_row = ""
            for x, cell in enumerate(row):
                if (x, y) in path:
                    print_row += "X"
                else:
                    print_row += cell
            print(print_row)

class Path:
    def __init__(self, positions) -> None:
        self.positions = positions
        assert(len(positions) >= 1)
        self.cost = 0
        self.direction = 1
        
    def add_step(self, position):
        distance = position_distance(self.positions[-1], position)
        assert(distance == 1)
            
        self.positions.append(position)
        new_direction = get_direction(self.positions[-2], self.positions[-1])
        if new_direction == self.direction:
            self.cost += 1
        else:
            self.cost += 1001
            
        return self

class Pathfinding():
    def __init__(self, grid: Grid) -> None:
        self.grid = grid
        
    # Pass a function to check whether locations are valid
    def from_a_t_b_depth_first(self, start, end, is_valid_function, shortest_only=False):
        self.paths = []
        self.paths.append(Path([start]))
        solutions = []
        self.visited = set()
        
        while self.paths:
            new_paths = []
            for path in self.paths:
                new_steps = self.add_new_steps(path.positions[-1])
                
                # Found end, add to solutions or return path
                if end in new_steps:
                    if shortest_only:
                        return path.positions + [new_step]
                    solutions.append(copy.deepcopy(path).add_step(end))
                
                # Add new valid steps    
                for new_step in new_steps:
                    if not is_valid_function(self.grid.at(new_step)):
                        continue
                    new_paths.append(copy.deepcopy(path).add_step(new_step))
            self.paths = new_paths
            
        return solutions
    
    def path_cost_dict(self, path):
        return self.lowest_costs[(path.positions[-1][0], 
                                  path.positions[-1][1], 
                                  path.direction)]
    
    def dijkstra_with_cost(self, start, end, is_valid_function):
        self.paths = []
        self.paths.append(Path([start]))
        self.visited = set()
        east_direction = 1
        self.lowest_costs = {(start[0], start[1], east_direction) : 0}
        self.lowest_cost_end = None
        
        while self.paths:           
            # Select path with lowest cost always                
            self.paths = sorted(self.paths, key=lambda path : \
                path.cost + position_distance(path.positions[-1], end))
            path = self.paths[0]
            self.paths.pop(0)
            
            # print(f"Dist to end: {position_distance(path.positions[-1], end)}")
            
            current_step = path.positions[-1]
            new_steps = self.add_new_steps(current_step, allow_duplicates=True)
            
            # Add new valid steps    
            for new_step in new_steps:
                new_direction = get_direction(current_step, new_step)
                if new_step in path.positions:
                    continue
                if not is_valid_function(self.grid.at(new_step)):
                    continue
                
                new_path = copy.deepcopy(path)
                new_path.add_step(new_step)
                if not (new_step[0], new_step[1], new_direction) in \
                    self.lowest_costs or \
                    self.lowest_costs[(new_step[0], new_step[1], new_direction)] > \
                        new_path.cost:
                        
                    self.lowest_costs[(new_step[0], new_step[1], new_direction)] = \
                        new_path.cost
                    self.paths.append(new_path)

                    # Found end with better cost, store solution
                if new_step == end:
                    solution = new_path
                    if not self.lowest_cost_end:
                        self.lowest_cost_end = new_path.cost
                    elif new_path.cost < self.lowest_cost_end:
                        self.lowest_cost_end = new_path.cost
                    
        return solution, self.lowest_cost_end

    # Grows patch around new_position
    def add_new_steps(self, position, allow_duplicates=False):
        new_positions = []
        for neighbour in get_neighbours(position):
            if not self.grid.is_in_bounds(neighbour):
                continue
            if not allow_duplicates and neighbour in self.visited:
                continue
            elif not allow_duplicates:
                self.visited.add(neighbour)
                
            new_positions.append(neighbour)
        return new_positions