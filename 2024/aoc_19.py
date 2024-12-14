from itertools import product

def memoize(func):
    cache = {}
    def inner(*args):
        if args not in cache:
            cache[args] = func(*args)
        return cache[args]
    return inner

@memoize
def can_create_pattern(pattern) -> bool:
    for part in globals()['parts']:
        if pattern == part:
            return True
        if pattern[:len(part)] == part:
            if can_create_pattern(pattern[len(part):]):
                return True
    return False

def part_1(patterns):
    total = sum([can_create_pattern(pattern) for pattern in patterns])
    print(total)
 
@memoize
def can_create_pattern(pattern) -> bool:
    for part in globals()['parts']:
        if pattern == part:
            return True
        if pattern[:len(part)] == part:
            if can_create_pattern(pattern[len(part):]):
                return True
    return False
    
@memoize
def how_many_options_for_pattern(pattern) -> int:
    counter = 0
    for part in globals()['parts']:
        if pattern == part:
            counter += 1
        if len(part) > len(pattern):
            continue
        if pattern[:len(part)] == part:
            counter += how_many_options_for_pattern(pattern[len(part):])
    return counter
    
def part_2(patterns):
    solutions = []
    for pattern in patterns:
        solutions.append(how_many_options_for_pattern(pattern))
        
    print(f"{solutions}, {sum(solutions)}")

def main():
    print("day 19")
    
    input = open("aoc_24/input/Day19.txt").read()
    global parts
    parts, patterns = input.split("\n\n")
    parts = parts.strip("\n").split(", ")
    
    patterns = patterns.strip().split("\n")
    part_1(patterns)

    part_2(patterns)

if __name__ == "__main__":
    main()