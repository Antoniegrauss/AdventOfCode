from grid import position_distance, add_positions
from itertools import product
import time

def memoize(func):
    cache = {}
    def inner(*args):
        if args not in cache:
            cache[args] = func(*args)
        return cache[args]
    return inner

class KeypadSolver():
    def __init__(self) -> None:
        # numpad: 
        # NOTE: forbidden position at 1, 3
        # +---+---+---+
        # | 7 | 8 | 9 |
        # +---+---+---+
        # | 4 | 5 | 6 |
        # +---+---+---+
        # | 1 | 2 | 3 |
        # +---+---+---+
        #     | 0 | A |
        #     +---+---+
        numpad_forbidden_position = (0, 3)
        self.numpad_positions = {
            '7': (0, 0),
            '8': (1, 0),
            '9': (2, 0),
            '4': (0, 1),
            '5': (1, 1),
            '6': (2, 1),
            '1': (0, 2),
            '2': (1, 2),
            '3': (2, 2),
            '0': (1, 3),
            'A': (2, 3)
        }
        
        self.numpad_paths = setup_numpad_paths(self.numpad_positions, numpad_forbidden_position)
        
        # Movepad
        # NOTE: forbidden position = 0, 0
        #     +---+---+
        #     | ^ | A |
        # +---+---+---+
        # | < | v | > |
        # +---+---+---+
        movepad_forbidden_position = (0, 0)
        self.movepad_positions = {
            '^': (1, 0),
            'A': (2, 0),
            '<': (0, 1),
            'v': (1, 1),
            '>': (2, 1),
        }
        # Convert moves to symbols on the movepad
        # NOTE: A is encoded as standing still (0, 0)
        self.movepad_lookup = {
            (0, 1): 'v',
            (0, -1): '^',
            (1, 0): '>',
            (-1, 0): '<',
            (0, 0): 'A'
        }
        self.movepad_lookup_inverse = {
            'v': (0, 1),
            '^': (0, -1),
            '>': (1, 0),
            '<': (-1, 0),
            'A': (0, 0)
        }
        self.movepad_paths = setup_movepad_paths(movepad_positions, movepad_forbidden_position)
        
    def part_2(self, patterns, depth):
        self.patterns = patterns
        
        results = []
        for pattern in patterns:
            input_length = self.handle_pattern(pattern, depth)
            results.append(input_length * pattern_score(pattern))
            
        return sum(results)
            
    def handle_pattern(self, pattern, depth):
        # First iteration with keypad
        depth -= 1
        numpad_paths = self.get_numpad_paths(pattern=pattern)
        string_paths = [self.nested_path_to_symbols(path) for path in numpad_paths]
        
        # For each character give the total sum
        total = sum([self.recursive_score(string_path, depth) for string_path in string_paths])
        return total
    
    @memoize
    def recursive_score(self, path, depth):
        if depth == 0:
            return self.total_cost(path)
        
        # Expand paths, return min score
        total = []
        path_options = path.split(",")
        for path_option in path_options:
            start_position = 'A'
            expanded_paths = self.expand_paths(path_option, start_position)
            current_costs = []
            for expanded_path in expanded_paths:
                current_costs.append(self.recursive_score(expanded_path, depth-1))
            total.append(sum(current_costs))
        return min(total)
        
    def total_cost(self, path):
        press_a_cost = 1
        options = path.split(",")
        return len(min(options, key=len)) + press_a_cost
        
    def get_numpad_paths(self, pattern):
        paths = []
        numpad_current = 'A'
        for symbol in pattern:
            path_options = self.numpad_paths[numpad_current, symbol]
            paths.append(path_options)
            numpad_current = symbol
            
        return paths
    
    def move_path_to_symbols(self, positions):
        return "".join([self.movepad_lookup[position] for position in positions])  

    def nested_path_to_symbols(self, nested_path):
        return ",".join([self.move_path_to_symbols(path) for path in nested_path])

    def nested_nested_path_to_symbols(self, move_paths):
        string_paths = ""
        for move_path_inner in move_paths:
            string_paths += self.nested_path_to_symbols(move_path_inner)
            string_paths += "[A]"
        return string_paths
    
    def nested_path_to_string_paths(self, paths):
        string_paths = []
        for path in paths:
            string_path = self.nested_path_to_symbols(path)
            string = ""
            for move in string_path:
                if move != "":
                    string += move
            string_paths.append(string)
        return string_paths
    
    def expand_paths(self, symbol_path, start_position):
        path_options = []
        for move in symbol_path:
            path_options.append(find_paths_for_move(start_position, 
                                                    self.movepad_lookup_inverse[move], 
                                                    self.movepad_lookup, 
                                                    self.movepad_paths))
            start_position = move
            
        if start_position == 'A':
            path_options.append([[]])
        else:
            press_a_paths = self.movepad_paths[start_position, 'A']
            path_options.append(press_a_paths)
        
        return self.nested_path_to_string_paths(path_options)

def step(a, b):
    assert position_distance(a, b) == 1
    return (b[0] - a[0], b[1] - a[1])

def straight_line(a, b):
    assert(are_on_straight_line(a, b))
    
    if a[0] == b[0]:
        distance_vertical = abs(b[1] - a[1])
        return [(0, (b[1] - a[1]) // distance_vertical) \
            for i in range(distance_vertical)]
    
    assert(a[1] == b[1])
    distance_horizontal = abs(b[0] - a[0])
    return [((b[0] - a[0]) // distance_horizontal, 0) \
        for i in range(distance_horizontal)]
 
def are_on_straight_line(a, b):
    return a[0] == b[0] or a[1] == b[1]

def step_in_either_direction(a, b, forbidden_steps):
    distance_horizontal = abs(b[0] - a[0])
    distance_vertical = abs(b[1] - a[1])
    horizontal = ((b[0] - a[0]) // distance_horizontal, 0)
    vertical = (0, (b[1] - a[1]) // distance_vertical)
    assert(position_distance(horizontal, (0, 0)) == 1)
    assert(position_distance(vertical, (0, 0)) == 1)
    
    moves = []
    if add_positions(horizontal, a) not in forbidden_steps:
        moves.append(horizontal)
    if add_positions(vertical, a) not in forbidden_steps:
        moves.append(vertical)
    return moves

def all_paths(start, end, forbidden_steps):
    if position_distance(start, end) == 1:
        return [[step(start, end)]]
    
    # If on same row, move up or down
    if are_on_straight_line(start, end):
        return [straight_line(start, end)]
    
    # Else do a step in one of the directions and call recursively
    both_options = step_in_either_direction(start, end, forbidden_steps)
    paths = []
    for move in both_options:
        new_position = add_positions(start, move)
        recursive_paths = all_paths(new_position, end, forbidden_steps)
        for path in recursive_paths:
            paths.append([move] + path)
    return paths

def setup_numpad_paths(numpad_positions, forbidden_position):   
    all_keys = list(numpad_positions.keys())
    all_key_steps = list(product(all_keys, all_keys))
    
    key_steps_dict = {}
    for step in all_key_steps:
        if step[0] == step[1]:
            continue
        possible_paths = all_paths(
            numpad_positions[step[0]], 
            numpad_positions[step[1]], 
            [forbidden_position]
        )
        key_steps_dict[step] = possible_paths
        
    return key_steps_dict

def setup_movepad_paths(movepad_positions, forbidden_position):      
    all_keys = list(movepad_positions.keys())
    all_key_steps = list(product(all_keys, all_keys))
    
    key_steps_dict = {}
    for step in all_key_steps:
        if step[0] == step[1]:
            continue
        possible_paths = all_paths(
            movepad_positions[step[0]], 
            movepad_positions[step[1]], 
            [forbidden_position]
        )
        key_steps_dict[step] = possible_paths
        
    return key_steps_dict

def shortest_path(paths):
    return min(paths, key=len)

def find_paths_for_move(start_position, move, movepad_lookup, movepad_paths):
    if start_position == movepad_lookup[move]:
        # Add empty path
        return [[]]
    return movepad_paths[start_position, movepad_lookup[move]]

def get_path_options_movepad(symbol_path, movepad_lookup, movepad_paths, start_position):
    path_options = []
    for move in symbol_path:
        path_options.append(find_paths_for_move(start_position, move, movepad_lookup, movepad_paths))
        start_position = movepad_lookup[move]
        
    if start_position == 'A':
        path_options.append([[]])
    else:
        press_a_paths = movepad_paths[start_position, 'A']
        path_options.append(press_a_paths)
    
    return path_options

def get_numpad_paths(pattern, numpad_paths):
    paths = []
    numpad_current = 'A'
    for symbol in pattern:
        path_options = numpad_paths[numpad_current, symbol]
        paths.append(path_options)
        numpad_current = symbol
        
    return paths

def get_robot_paths(input_paths, movepad_lookup, movepad_paths):
    paths = []
    for symbol_paths in input_paths:
        start_position = 'A'
        options_for_this_symbol = [
            get_path_options_movepad(symbol_path, movepad_lookup, movepad_paths, start_position) \
                for symbol_path in symbol_paths
        ]
        paths.append(options_for_this_symbol)
    return paths

def best_option_cost(options):
    button_press_action = 1
    return sum(min([
        [len(min(move_option, key=len)) + button_press_action\
            for move_option in options]
    ]))

def pick_shortest_paths(paths):
    scores = []
    
    for path in paths:
        this_score = 0
        for i in path:
            best_options = []
            for j in i:
                best_option = best_option_cost(j)
                best_options.append(best_option)
            best = min(best_options)
            this_score += best
        scores.append(this_score)
        
    return min(scores)

def calculate_shortest_score_length(scores):
    total_length = 0
    for path in scores:
        total_length += pick_shortest_paths(path)
    return total_length

def pattern_score(pattern):
    if pattern[-1] == 'A':
        return int("".join(pattern[:-1]))
    else:
        return int("".join(pattern))

def part_1(pattern, movepad_paths, movepad_lookup, numpad_paths):
    numpad_paths = get_numpad_paths(pattern=pattern, numpad_paths=numpad_paths)      
        
    # Have the first robot execute the paths by pressing on the movepad
    # After each symbol press the A button
    robot_1_paths = get_robot_paths(numpad_paths, movepad_lookup, movepad_paths)
    
    robot_2_paths = []
    for path in robot_1_paths:
        new_robot_2_paths = [get_robot_paths(inner_path, movepad_lookup, movepad_paths)
             for inner_path in path
            ]
        robot_2_paths.append(new_robot_2_paths)
        
    shortest_score_length = calculate_shortest_score_length(robot_2_paths)
    
    return shortest_score_length * pattern_score(pattern)

def main():
    print("day 21")
        
    # numpad: 
    # NOTE: forbidden position at 1, 3
    # +---+---+---+
    # | 7 | 8 | 9 |
    # +---+---+---+
    # | 4 | 5 | 6 |
    # +---+---+---+
    # | 1 | 2 | 3 |
    # +---+---+---+
    #     | 0 | A |
    #     +---+---+
    numpad_forbidden_position = (0, 3)
    numpad_positions = {
        '7': (0, 0),
        '8': (1, 0),
        '9': (2, 0),
        '4': (0, 1),
        '5': (1, 1),
        '6': (2, 1),
        '1': (0, 2),
        '2': (1, 2),
        '3': (2, 2),
        '0': (1, 3),
        'A': (2, 3)
    }
    
    global numpad_paths
    numpad_paths = setup_numpad_paths(numpad_positions, numpad_forbidden_position)
    
    # Movepad
    # NOTE: forbidden position = 0, 0
    #     +---+---+
    #     | ^ | A |
    # +---+---+---+
    # | < | v | > |
    # +---+---+---+
    movepad_forbidden_position = (0, 0)
    global movepad_positions
    movepad_positions = {
        '^': (1, 0),
        'A': (2, 0),
        '<': (0, 1),
        'v': (1, 1),
        '>': (2, 1),
    }
    # Convert moves to symbols on the movepad
    # NOTE: A is encoded as standing still (0, 0)
    global movepad_lookup
    movepad_lookup = {
        (0, 1): 'v',
        (0, -1): '^',
        (1, 0): '>',
        (-1, 0): '<',
        (0, 0): 'A'
    }
    global movepad_paths
    movepad_paths = setup_movepad_paths(movepad_positions, movepad_forbidden_position)
    
    # You are pressing:
    # <vA<AA>>^AvAA<^A>A<v<A>>^AvA^A<vA>^A<v<A>^A>AAvA^A<v<A>A>^AAAvA<^A>A
    # Robot3 is pressing:
    # v<<A>>^A<A>AvA<^AA>A<vAAA>^A
    # Robot 2 is pressing:
    # <A^A>^^AvvvA
    # Robot 1 is pressing:
    # 029A
    
    test_input = open("aoc_24/input/Day21_test.txt").read()
    test_patterns = [line.strip("\n") for line in test_input.split("\n")]
    
    input = open("aoc_24/input/Day21.txt").read()
    patterns = [line.strip("\n") for line in input.split("\n")]
    
    assert(sum([part_1(pattern, movepad_paths, movepad_lookup, numpad_paths) 
               for pattern in patterns]) == 212488)
    
    # For part 2 we use 25 robots
    solver = KeypadSolver()
    # Should be 6 -> ^^A, A -> vvA
    assert(solver.part_2([['6', 'A']], 1) == 6 * 6)
    assert(solver.part_2([['9']], 1) == 4 * 9)
    assert(solver.part_2([['2']], 2) == 9 * 2)
    
    assert(solver.part_2([['3']], 2) == 4 * 3)
    assert(solver.part_2([['3']], 3) == 12 * 3)
    assert(solver.part_2([['6']], 2) == 5 * 6)
    assert(solver.part_2([['6']], 3) == 13 * 6)
    assert(solver.part_2([['0', '2', '9', 'A']], 3) == 68 * 29)
    
    assert(solver.part_2(patterns, 3) == 212488)
    
    # Somehow part answer must be 1 robot higher than in my implementation
    start = time.time()
    assert(solver.part_2(patterns, 26) == 258263972600402)
    print(time.time() - start)
    
    # Still runs within 100ms with 500 robots
    # Before 1000 the stack will overflow
    start = time.time()
    solver.part_2(patterns, 500)
    print(time.time() - start)

if __name__ == "__main__":
    main()