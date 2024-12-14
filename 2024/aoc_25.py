import time

def parse_locks_and_keys(lock_and_key):
    locks = []
    keys = []
    for lock_or_key in lock_and_key:
        rows = lock_or_key.split("\n")
        if "#" in rows[0]:
            locks.append(parse_lock(rows))
        else:
            assert("#" not in rows[0])
            keys.append(parse_key(rows))
    
    return locks, keys

def parse_lock(rows):
    heights = [0, 0, 0, 0, 0]
    for row in rows[1:]:
        if "#" not in row:
            break
        for id, value in enumerate(row):
            if value == "#":
                heights[id] += 1
    return tuple(heights)
    
def parse_key(rows):
    return parse_lock(rows[::-1])

def does_lock_fit_key(lock, key) -> bool:
    for tooth, hole in zip(lock, key):
        if tooth + hole > 5:
            return False
    return True

def main():
    print("day 25")
    
    start = time.time()
    input = open("aoc_24/input/Day25.txt").read()
    lock_and_key = input.split("\n\n")
    locks, keys = parse_locks_and_keys(lock_and_key)
    # print(f"parsed locks and keys: {locks}, {keys}")
    
    total_fits = 0
    for lock in locks:
        for key in keys:
            if does_lock_fit_key(lock, key):
                total_fits += 1
                # print(f"Key {key} fits in lock {lock}")
                
    print(f"Total time taken: {time.time() - start}")
    print(f"Result day 1: {total_fits}")

if __name__ == "__main__":
    main()