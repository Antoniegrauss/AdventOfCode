def memoize(func):
    cache = {}
    def inner(stone, num):
        if (stone, num) not in cache:
            cache[stone, num] = func(stone, num)
        return cache[stone, num]
    return inner

@memoize
def blink(stone, num):
    if num == 0:
        return 1
    
    if stone == 0:
        return blink(1, num - 1)
    if len(str(stone)) % 2 == 0:
        return blink(int(str(stone)[0:int(len(str(stone)) / 2)]), num - 1) + \
            blink(int(str(stone)[int(len(str(stone)) / 2):]), num - 1)
    return blink(stone * 2024, num - 1)


def main():
    stones = [77, 515, 6779622, 6, 91370, 959685, 0, 9861]
    print(sum([blink(stone, 75) for stone in stones]))

if __name__=="__main__":
    main()