import copy

def is_in_bounds(position, width, height):
    return position[0] >= 0 and position[1] >= 0 and position[0] < width and position[1] < height

# Gets adjacent neighbours for a position
def get_neighbours(position):
    return [
        (position[0] + 1, position[1]),
        (position[0] - 1, position[1]),
        (position[0], position[1] + 1),
        (position[0], position[1] - 1),
    ]

# Grows patch around new_position
def add_new_steps(patch, lines, new_position, map_width, map_height):
    new_positions = []
    for neighbour in get_neighbours(new_position):
        if is_in_bounds(neighbour, map_width, map_height):
            if lines[neighbour[1]][neighbour[0]] == patch.character and \
                neighbour not in patch.positions:

                patch.add_position(neighbour)
                new_positions.append(neighbour)

    return new_positions


def discover_patch(start_position, lines, map_width, map_height):
    patch = Patch(character=lines[start_position[1]][start_position[0]])
    patch.add_position(start_position)

    positions_to_check = [start_position]
    while positions_to_check:
        new_positions = add_new_steps(patch, lines, positions_to_check[0], map_width, map_height)
        positions_to_check.pop(0)
        positions_to_check += new_positions

    return patch

class Patch():
    def __init__(self, character: str) -> None:
        assert(len(character) == 1)
        self.character = character
        self.positions = set()
        self.fences = set()

    def add_position(self, position):
        self.positions.add(position)

    def size(self):
        return len(self.positions)
    
    def perimiter(self):
        total = 0

        for position in self.positions:
            this_perimiter = 4
            for neighbour in get_neighbours(position):
                if neighbour in self.positions:
                    this_perimiter -= 1
            total += this_perimiter

        return total
    
    def create_fences(self):
        for position in self.positions:
            for neighbour in get_neighbours(position):
                if neighbour not in self.positions:
                    self.fences.add(Fence(position_in_area=position, position_outside_area=neighbour))
    
    def cost(self):
        return self.size() * self.sides()
    
    def sides(self):
        visited = set()
        
        sides = 0
        for fence in self.fences:
            if fence in visited:
                continue
            
            visited.add(fence)
            
            # Get fences that could continue on the same side
            fences_on_same_side = fence.connecting_positions()
            while fences_on_same_side:
                next_fence = fences_on_same_side[-1]
                if next_fence not in self.fences or \
                    next_fence in visited:
                        
                    fences_on_same_side.remove(next_fence)
                    continue
                
                # fence.print()
                # print(f"is connected to:")
                # next_fence.print()
                
                visited.add(next_fence)
                
                # Keep adding connections of connections
                new_fences = next_fence.connecting_positions()
                for new_fence in new_fences:
                    if new_fence not in visited:
                        fences_on_same_side.append(new_fence)
                fences_on_same_side.remove(next_fence)
                
            # We have reached completed 1 fence side
            sides += 1
        return sides
                

class Fence():
    # Contains 2 positions between which there is a fence
    def __init__(self, position_in_area, position_outside_area) -> None:
        self.position_in_area = position_in_area
        self.position_outside_area = position_outside_area

        # Exact coordinate difference should be 1
        assert(abs(position_in_area[0] - position_outside_area[0]) + \
               abs(position_in_area[1] - position_outside_area[1]) == 1)
        
        self._determine_orientation()
        
    def __eq__(self, other: object) -> bool:
        inside_same = \
            self.position_in_area[0] == other.position_in_area[0] and \
            self.position_in_area[1] == other.position_in_area[1]
        outside_same = \
            self.position_outside_area[0] == other.position_outside_area[0] and \
            self.position_outside_area[1] == other.position_outside_area[1]
        return inside_same and outside_same

    def __hash__(self) -> int:
        return hash((self.position_in_area[0], self.position_in_area[1],
                     self.position_outside_area[0], self.position_outside_area[1]))

    def _determine_orientation(self):
        if self.position_in_area[0] == self.position_outside_area[0]:
            self.direction = "horizontal"
        else:
            self.direction = "vertical"

    def print(self):
        print(f"Fence, position inside: {self.position_in_area}, position outside: {self.position_outside_area}")

    def connecting_positions(self):
        if self.direction == "vertical":
            return [
                Fence(
                    (self.position_in_area[0], self.position_in_area[1] - 1),
                    (self.position_outside_area[0], self.position_outside_area[1] - 1)
                ),
                Fence(
                    (self.position_in_area[0], self.position_in_area[1] + 1),
                    (self.position_outside_area[0], self.position_outside_area[1] + 1),
                )
            ]
        else:
            return [
                Fence(
                    (self.position_in_area[0]  - 1, self.position_in_area[1]),
                    (self.position_outside_area[0]  - 1, self.position_outside_area[1])
                ),
                Fence(
                    (self.position_in_area[0]  + 1, self.position_in_area[1]),
                    (self.position_outside_area[0]  + 1, self.position_outside_area[1]),
                )
            ]

def main():
    print("day 12")

    input = open("aoc_24/input/Day12.txt")
    lines = input.readlines()
    lines = [line.strip("\n") for line in lines]
    
    map_width = len(lines[0])
    map_height = len(lines)
    
    # Test Fence comparison
    assert(
        Fence(position_in_area=(0, 0), position_outside_area=(0, 1)) == \
        Fence(position_in_area=(0, 0), position_outside_area=(0, 1))
    )
    assert(
        Fence(position_in_area=(0, 0), position_outside_area=(0, 1)) != \
        Fence(position_in_area=(0, 0), position_outside_area=(0, -1))
    )

    patches = []
    visited = set()
    for y, line in enumerate(lines):
        for x, _ in enumerate(line):
            if (x, y) in visited:
                continue
            patches.append(discover_patch((x, y), lines, map_width, map_height))
            patches[-1].create_fences()

            # Add all positions to visited
            for position in patches[-1].positions:
                visited.add(position)
            
                
    total_cost = sum([patch.cost() for patch in patches])

    print(f"Total {total_cost}")

if __name__ == "__main__":
    main()