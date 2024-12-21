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

def main():
    print("day 21")
    
    input = open("aoc_24/input/Day21.txt").read()
    patterns = [line.strip("\n") for line in input.split("\n")]
    print(patterns)
        
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
        '>': (2, 2),
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
    robot_3_position = 'A'
    robot_2_position = 'A'
    robot_1_position = 'A'
    numpad_current = 'A'
    
    paths = []
    for symbol in patterns[0]:
        path_options = numpad_paths[numpad_current, symbol]
        paths.append(path_options)
        print(f"From {numpad_current} to {symbol}, possible via: \n {path_options}")
        numpad_current = symbol      
    
    print(f"Pathing options for first digit on numpad: \n {paths}")

if __name__ == "__main__":
    main()