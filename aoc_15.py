import grid as g
import time

def is_valid(character):
    return character == "."

def is_box(character):
    return character == "O"

def is_box_part_2(character):
    return character in "[]"

def is_wall(character):
    return character == "#"

def do_move(grid, position, move):
    if move == "^":
        offset = (0, -1)
    elif move == "v":
        offset = (0, 1)
    elif move == "<":
        offset = (-1, 0)
    elif move == ">":
        offset = (1, 0)
    else:
        assert(False)
        
    return resolve_move(grid, position, offset)
    
def resolve_move(grid, position, offset):
    assert(offset is not None)
    check_position = add_positions(position, offset)
    # If nothing there, just move
    if is_valid(grid.at(check_position)):
        grid.set(position, ".")
        grid.set(check_position, "@")
        return check_position
        
    # If box, keep incrementing untill not a box anymore
    # If free space, move 1 step, set the free space to box
    elif is_box(grid.at(check_position)):
        return resolve_box_push(grid, position, offset)
    
    elif is_box_part_2(grid.at(check_position)):
        return resolve_box_push_part_2(grid, position, offset)
    
    return position

def add_positions(position, offset):
    return (position[0] + offset[0], position[1] + offset[1])

def resolve_box_push(grid, position, offset):
    first_step = add_positions(position, offset)
    check_position = first_step
    while is_box(grid.at(check_position)):
        check_position = add_positions(check_position, offset)
    if is_valid(grid.at(check_position)):
        grid.set(position, ".")
        grid.set(first_step, "@")
        grid.set(check_position, "O")
        return first_step
    return position

def find_both_boxes(grid, position):
    box_coords = []
    box_coords.append(position)
    if grid.at(position) == "[":
        box_coords.append(add_positions(position, (1, 0)))
    elif grid.at(position) == "]":
        box_coords.append(add_positions(position, (-1, 0)))
    return box_coords

def resolve_box_push_part_2(grid, position, offset):
    first_step = add_positions(position, offset)
    box_coords = set()
    first_box = find_both_boxes(grid, first_step)
    for box in first_box:
        box_coords.add(box)
    assert(len(box_coords) == 2)
    
    positions_to_check = [add_positions(box_half, offset) for box_half in box_coords]
        
    # Do a DFS over the boxes untill we find a wall
    while positions_to_check:
        check_position = positions_to_check.pop()
        if is_wall(grid.at(check_position)):
            return position
        if is_box_part_2(grid.at(check_position)):
            connected_box = find_both_boxes(grid, check_position)
            for box_half in connected_box:
                if box_half not in box_coords:
                    positions_to_check.append(add_positions(box_half, offset))
                    box_coords.add(box_half)
        if is_valid(grid.at(check_position)):
            continue
        
    # If exited loop without finding wall, move all boxes by offset
    # Store what characters were in the box locations
    box_characters = [grid.at(box) for box in box_coords]
    for box in box_coords:
        grid.set(box, ".")
        
    for box, character in zip(box_coords, box_characters):
        grid.set(add_positions(box, offset), character)
        
    grid.set(position, ".")
    grid.set(first_step, "@")
    
    return first_step

def widen_grid(grid):
    new_rows = []
    for row in grid.cells:  
        new_row = []
        for character  in row:
            if character == "#":
                new_row.append("#")
                new_row.append("#")
            elif character == "@":
                new_row.append("@")
                new_row.append(".")
            elif character == "O":
                new_row.append("[")
                new_row.append("]")
            elif character == ".":
                new_row.append(".")
                new_row.append(".")
        new_rows.append(new_row)
    grid.cells = new_rows
    grid.width = len(new_rows[0])
    grid.height = len(new_rows)

def do_part_1(grid, position, moves):
    for move in moves:
        position = do_move(grid, position, move)
        
    sum_box_coords = sum([(x[0] + 100 * x[1]) for x in grid.find_character("O")])
    print(sum_box_coords)

def do_part_2(grid, moves):
    widen_grid(grid)
    start = grid.find_character("@")[0]
    position = start
    
    for move in moves:
        position = do_move(grid, position, move)
        # print(f"Move: {move}")
        
    grid.print_grid()
    print()
    sum_box_coords = sum([(x[0] + 100 * x[1]) for x in grid.find_character("[")])
    print(sum_box_coords)

def main():
    print("day 15")
    input = open("aoc_24/input/Day15.txt").read()
    grid, moves = input.split("\n\n")
    
    grid = g.grid_factory_str(grid)
    moves = [move for move in moves if move != "\n"]
    
    # Sanity check
    for move in moves:
        assert(move in ["^", "v", "<", ">"])
    

    grid.print_grid()
    
    # do_part_1(grid, position, moves)
    
    do_part_2(grid, moves)

if __name__ == "__main__":
    main()