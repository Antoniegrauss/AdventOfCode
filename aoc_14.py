import re
import time
from functools import reduce
from operator import mul

def move_timesteps(robots, timesteps, map_width, map_height):
    for robot in robots:
        robot[0] = (robot[0] + (timesteps * robot[2])) % (map_width)
        robot[1] = (robot[1] + (timesteps * robot[3])) % (map_height)

def get_quadrant(robot, width_line, height_line):
    if robot[0] < width_line and robot[1] < height_line:
        return 0
    if robot[0] > width_line and robot[1] < height_line:
        return 1
    if robot[0] < width_line and robot[1] > height_line:
        return 2
    if robot[0] > width_line and robot[1] > height_line:
        return 3

def safety_factor(robots, width_division_line, height_division_line):
    quadrants = [get_quadrant(robot, width_division_line, height_division_line) for robot in robots]       
    return reduce(mul, [quadrants.count(i) for i in range(4)])

def do_part_1():
    input = open("aoc_24/input/Day14.txt")
    robots = read_robots(input)
        
    map_width = 101
    map_height = 103
    move_timesteps(robots, 100, map_width, map_height)
    
    width_division_line = 50
    height_division_line = 51
    return safety_factor(robots, width_division_line, height_division_line)

def read_robots(input):
    robots = []
    for line in input.readlines():
        pos_x, pos_y, vel_x, vel_y = re.findall(r"-*\d+",  line)
        robots.append([int(pos_x), int(pos_y), int(vel_x), int(vel_y)])
        
    return robots

def do_test_input_part_1():
    input = open("aoc_24/input/Day14_test.txt")
    robots = read_robots(input)
        
    map_width = 11
    map_height = 7
    move_timesteps(robots, 100, map_width, map_height)
    
    width_division_line = 5
    height_division_line = 3
    return safety_factor(robots, width_division_line, height_division_line)
    
def do_part_2(start_timestep, show=False):
    input = open("aoc_24/input/Day14.txt")
    robots = read_robots(input)
        
    map_width = 101
    map_height = 103
    move_timesteps(robots, start_timestep, map_width, map_height)
    
    if show:
        print("Timestep", start_timestep)
        print_grid(map_width, map_height, [(robot[0], robot[1]) for robot in robots])
        time.sleep(1)
        return
    
    width_division_line = 50
    height_division_line = 51
    return safety_factor(robots, width_division_line, height_division_line), start_timestep

def print_grid(width, height, robot_positions):
    for pos_y in range(height):
        for pos_x in range(width):
            if (pos_x, pos_y) in robot_positions:
                print("#", end="")
            else:
                print(".", end="")
        print()

def main():
    print("day 14")
    
    # do_test_input_part_1()
    safety_factors = sorted([do_part_2(start_timestep=i) for i in range(10000)], key=lambda x: x[0])
    # print(sorted(safety_factors))
    
    for low_factor, start_time in safety_factors[:20]:
        do_part_2(start_timestep=start_time, show=True)
        
    print(safety_factors[:20])
    
if __name__== "__main__":
    main()