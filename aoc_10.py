import copy

def is_in_bounds(position, width, height):
    return position[0] >= 0 and position[1] >= 0 and position[0] < width and position[1] < height

def add_new_steps(path, lines, map_width, map_height):
    new_paths = []

    neighbours = [
        [path[-1][0] + 1, path[-1][1]],
        [path[-1][0] - 1, path[-1][1]],
        [path[-1][0], path[-1][1] + 1],
        [path[-1][0], path[-1][1] - 1],
    ]

    current_number = int(lines[path[-1][1]][path[-1][0]])
    for neighbour in neighbours:
        if is_in_bounds(neighbour, map_width, map_height):
            try:
                neighbour_number = int(lines[neighbour[1]][neighbour[0]])
                if  neighbour_number == current_number + 1:

                    new_paths.append(copy.copy(path))
                    new_paths[-1].append(tuple(neighbour))
            except:
                continue

    return new_paths


def step_once(paths, finished, lines, map_width, map_height):
    new_paths = []
    for path in paths:
        if lines[path[-1][1]][path[-1][0]] == "9":
            finished.append(path)
            continue
        
        new_paths += add_new_steps(path, lines, map_width, map_height)

    return new_paths, finished

def possible_ends_for_trailhead(trail_head, lines, map_width, map_height):
    finished = []
    paths = [[trail_head]]

    while paths:
        paths, finished = step_once(paths, finished, lines, map_width, map_height)

    return finished


def main():
    print("day 10")

    # input = "#.\n^."
    # lines = input.split("\n")
    input = open("aoc_24/input/Day10_test.txt")
    lines = input.readlines()
    lines = [line.strip("\n") for line in lines]
    
    map_width = len(lines[0].strip("\n"))
    map_height = len(lines)

    trailheads = []
    trail_head = 0
    for y, line in enumerate(lines):
        for x, char in enumerate(line):
            try:
                if int(char) == trail_head:
                 trailheads.append((x, y))
            except:
                continue
                
    sum = 0
    for head in trailheads:
        print(head)
        finshed_paths = possible_ends_for_trailhead(head, lines, map_width, map_height)
        print(f"ends: {len(finshed_paths)}")
        sum += len(finshed_paths)

    print(f"Total {sum}")

if __name__ == "__main__":
    main()