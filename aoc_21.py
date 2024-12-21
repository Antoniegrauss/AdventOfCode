from grid import position_distance, add_positions
from itertools import product

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

def move_path_to_symbols(positions, movepad_lookup):
    return "".join([movepad_lookup[position] for position in positions])  

def nested_path_to_symbols(nested_path, movepad_lookup):
    return "[" + \
        ",".join([move_path_to_symbols(path, movepad_lookup) for path in nested_path]) + \
     "]"

def nested_nested_path_to_symbols(move_paths, movepad_lookup):
    string_paths = ""
    for move_path_inner in move_paths:
        string_paths += nested_path_to_symbols(move_path_inner, movepad_lookup)
        string_paths += "[A]"
    return string_paths

def find_paths_for_move(start_position, move, movepad_lookup, movepad_paths):
    if start_position == movepad_lookup[move]:
        # Add empty path
        return [[]]
    return movepad_paths[start_position, movepad_lookup[move]]

def get_path_options_movepad(symbol_path, movepad_lookup, movepad_paths, start_position):
    path_options = []
    for move in symbol_path:
        path_options.append(find_paths_for_move(start_position, move, movepad_lookup, movepad_paths))
        # print(f"Move from {start_position} to {movepad_lookup[move]}, paths: \n {nested_nested_path_to_symbols(path_options, movepad_lookup)}")
        start_position = movepad_lookup[move]
        
    if start_position == 'A':
        path_options.append([[]])
    else:
        press_a_paths = movepad_paths[start_position, 'A']
        # print("Pressing A: ", nested_path_to_symbols(press_a_paths, movepad_lookup))
        path_options.append(press_a_paths)
    
    # print(f"Total paths: {nested_nested_path_to_symbols(path_options, movepad_lookup)} \n")    
    return path_options

def get_numpad_paths(pattern, numpad_paths):
    paths = []
    numpad_current = 'A'
    for symbol in pattern:
        path_options = numpad_paths[numpad_current, symbol]
        paths.append(path_options)
        # print(f"From {numpad_current} to {symbol}, possible via: \n {path_options}")
        numpad_current = symbol
        
    return paths

def get_robot_paths(input_paths, movepad_lookup, movepad_paths):
    paths = []
    for symbol_paths in input_paths:
        # print(f"\nRobot 1, moving to symbol {patterns[0][id]}")
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
    # print("Picking shortest path")
    scores = []
    
    for path in paths:
        this_score = 0
        for i in path:
            best_options = []
            for j in i:
                best_option = best_option_cost(j)
                best_options.append(best_option)
            best = min(best_options)
            # print(f"best option length: {best}")
            this_score += best
        scores.append(this_score)
        
    # print(f"Scores: {scores}")
    return min(scores)

def calculate_shortest_score_length(scores):
    # counter = 1
    total_length = 0
    for path in scores:
        total_length += pick_shortest_paths(path)
        # counter += 1
        # for id_1, i in enumerate(path):
        #     for id_2, j in enumerate(i):
        #         for k in j:
                    # print(f"{str(counter)}, {str(id_1)}, {str(id_2)}, {nested_nested_path_to_symbols(k, movepad_lookup)}")
    
    return total_length

def pattern_score(pattern):
    return int("".join(pattern[:-1]))

def part_1(pattern, movepad_paths, movepad_lookup, numpad_paths):
    numpad_paths = get_numpad_paths(pattern=pattern, numpad_paths=numpad_paths)      
    
    # print(f"Pathing options for first digit on numpad: \n {nested_nested_path_to_symbols(numpad_paths, movepad_lookup)}")
    
    # Have the first robot execute the paths by pressing on the movepad
    # After each symbol press the A button
    robot_1_paths = get_robot_paths(numpad_paths, movepad_lookup, movepad_paths)
    # print("Robot 1 paths for digit 1")
    # counter = 1
    # for path in robot_1_paths:
    #     for path_inner in path:
    #         print(str(counter) + nested_nested_path_to_symbols(path_inner, movepad_lookup))
    #     counter += 1
    
    robot_2_paths = []
    for path in robot_1_paths:
        new_robot_2_paths = [get_robot_paths(inner_path, movepad_lookup, movepad_paths)
             for inner_path in path
            ]
        robot_2_paths.append(new_robot_2_paths)
        
    shortest_score_length = calculate_shortest_score_length(robot_2_paths)
    print(shortest_score_length, pattern_score(pattern))
    
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
    
    numpad_paths = setup_numpad_paths(numpad_positions, numpad_forbidden_position)
    # print(numpad_paths)
    
    # Movepad
    # NOTE: forbidden position = 0, 0
    #     +---+---+
    #     | ^ | A |
    # +---+---+---+
    # | < | v | > |
    # +---+---+---+
    movepad_forbidden_position = (0, 0)
    movepad_positions = {
        '^': (1, 0),
        'A': (2, 0),
        '<': (0, 1),
        'v': (1, 1),
        '>': (2, 1),
    }
    # Convert moves to symbols on the movepad
    # NOTE: A is encoded as standing still (0, 0)
    movepad_lookup = {
        (0, 1): 'v',
        (0, -1): '^',
        (1, 0): '>',
        (-1, 0): '<',
        (0, 0): 'A'
    }
    movepad_paths = setup_movepad_paths(movepad_positions, movepad_forbidden_position)
    # print(movepad_paths)
    
    # You are pressing:
    # <vA<AA>>^AvAA<^A>A<v<A>>^AvA^A<vA>^A<v<A>^A>AAvA^A<v<A>A>^AAAvA<^A>A
    # Robot3 is pressing:
    # v<<A>>^A<A>AvA<^AA>A<vAAA>^A
    # Robot 2 is pressing:
    # <A^A>^^AvvvA
    # Robot 1 is pressing:
    # 029A
    
    input = open("aoc_24/input/Day21_test.txt").read()
    patterns = [line.strip("\n") for line in input.split("\n")]
    for pattern in patterns:
        print(part_1(pattern, movepad_paths, movepad_lookup, numpad_paths))
    
    input = open("aoc_24/input/Day21.txt").read()
    patterns = [line.strip("\n") for line in input.split("\n")]
    
    assert(sum([part_1(pattern, movepad_paths, movepad_lookup, numpad_paths) 
               for pattern in patterns]) == 212488)
    
    # For part 2 we use 25 robots

if __name__ == "__main__":
    main()