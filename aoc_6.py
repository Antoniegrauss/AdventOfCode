def parse_obstacles(lines: "list[str]"):
    obstacles = []
    for y, line in enumerate(lines):
        for x, character in enumerate(line):
            if character == "#":
                obstacles.append((x, y))

    return obstacles

def find_start(lines):
    for y, line in enumerate(lines):
        for x, character in enumerate(line):
            if character == '^':
                return (x, y)
            
def step(position, obstacles, direction):
    next_position = next_step(position, direction)
    if next_position in obstacles:
        direction = change_direction_clockwise(direction=direction)
        # print(f"Hit obstacle: {next_position} at position: {position}, new direction: {direction}")
        return position, direction
    else:
        return next_position, direction

def next_step(position, direction):
    if direction == "up":
        return (position[0], position[1] - 1)
    elif direction == "right":
        return (position[0] + 1, position[1])
    elif direction == "down":
        return (position[0], position[1] + 1)
    elif direction == "left":
        return (position[0] - 1, position[1])

def change_direction_clockwise(direction):
    if direction == "up":
        return "right"
    elif direction == "right":
        return "down"
    elif direction == "down":
        return "left"
    elif direction == "left":
        return "up"

def is_in_bounds(position, width, height):
    return position[0] >= 0 and position[1] >= 0 and position[0] < width and position[1] < height

def new_obstacle_creates_loop(start_position, direction, new_obstacles, map_width, map_height):
    max_steps_before_loop = 25000
    steps = 0
    position = start_position
    direction = direction
    visited = {position}
    while steps < max_steps_before_loop:
        position, direction = step(position, new_obstacles, direction)
        visited.add(position)
        if not is_in_bounds(position, map_width, map_height):
            return False
        if steps > (len(visited) * 3):
            return True
        steps += 1
    return True

def main():
    print("day 6")

    # input = "#.\n^."
    # lines = input.split("\n")
    input = open("aoc_24/input/Day6.txt")
    lines = input.readlines()
    
    map_width = len(lines[0].strip("\n"))
    map_height = len(lines)
    direction = "up"

    print(f"width: {map_width}, height: {map_height}")

    obstacles = parse_obstacles(lines)
    print(f"{len(obstacles)}")
    start_position = find_start(lines)
    print(f"Start position: {start_position}")

    visited = {start_position}
    position = start_position

    correct_new_obstacles = set()
    while True:
        previous_position = position
        position, direction = step(position, obstacles, direction)
        if not is_in_bounds(position, map_width, map_height):
            print(f"Left map area at: {position} in direction: {direction}")
            break

        # For each new position check whether we could create a loop
        if position not in visited and position is not start_position:
            # print(f"Vars before checking loop: {len(obstacles)}, direction: {direction}, posiiont: {position}")
            new_obstacles = obstacles
            new_obstacles.append(position)
            direction_copy = direction

            # Simulate path from previous position with extra obstacle
            # If it runs for more than max_steps_before_loop then it is a loop
            if new_obstacle_creates_loop(previous_position, direction_copy, new_obstacles, map_width, map_height):
                print(f"Found a new loop obstacle at {position}, visited: {len(visited)}")
                correct_new_obstacles.add(position)
                # print(f"Vars after checking loop: {len(obstacles)}, direction: {direction}, posiiont: {position}")
            
            # Stupid python shallow copying list
            obstacles.pop(-1)

        visited.add(position)
        # print(f"total steps: {len(visited)}")

    print(f"Amount of visited squares: {len(visited)}, possibilities for loop: {len(correct_new_obstacles)}, \
          total squares: {map_width*map_height}")

if __name__ == "__main__":
    main()