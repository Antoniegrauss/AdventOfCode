def parse_antennas(lines: "list[str]"):
    obstacles = []
    for y, line in enumerate(lines):
        for x, character in enumerate(line.strip("\n")):
            if character != ".":
                obstacles.append((character, x, y))

    return obstacles

def pair_antennas(antennas):
    matches = []
    for id, antenna in enumerate(antennas):
        for id_2, antenna_2 in enumerate(antennas):
            if id == id_2:
                continue
            elif antenna[0] == antenna_2[0]:
                matches.append((antenna, antenna_2))

    return matches

def antinodes(antenna_pair, map_width, map_height):
    pos_1 = (antenna_pair[0][1], antenna_pair[0][2])
    pos_2 = (antenna_pair[1][1], antenna_pair[1][2])

    diff_x = pos_1[0] - pos_2[0]
    diff_y = pos_1[1] - pos_2[1]

    nodes = [[pos_1[0], pos_1[1]]]

    i = 1
    while True:
        new_node = [
            pos_1[0] + (i * diff_x),
            pos_1[1] + (i * diff_y)
        ]
        if is_in_bounds(new_node, map_width, map_height):
            nodes.append(new_node)
        else:
            break
        i += 1

    i = -1
    while True:
        new_node = [
            pos_1[0] + (i * diff_x),
            pos_1[1] + (i * diff_y)
        ]
        if is_in_bounds(new_node, map_width, map_height):
            nodes.append(new_node)
        else:
            break
        i -= 1
    
    return nodes


def is_in_bounds(position, width, height):
    return position[0] >= 0 and position[1] >= 0 and position[0] < width and position[1] < height


def main():
    print("day 8")

    # input = "#.\n^."
    # lines = input.split("\n")
    input = open("aoc_24/input/Day8.txt")
    lines = input.readlines()
    
    map_width = len(lines[0].strip("\n"))
    map_height = len(lines)

    antennas = parse_antennas(lines)
    print(antennas)

    matches = pair_antennas(antennas)

    total_antinodes = set()
    for match in matches:
        print(f"Match: {match}")
        new_antinodes = (antinodes(match, map_width, map_height))
        print(f"New antinodes {new_antinodes}")

        for node in new_antinodes:
            total_antinodes.add((node[0], node[1]))

    print(f"nodes: {len(total_antinodes)}")

if __name__ == "__main__":
    main()