import numpy as np
import re


class Claw():
    def __init__(self, a, b, prize) -> None:
        self.a = a
        self.b = b
        self.prize = prize
        
    def parse_claw(input, line):
        a = re.findall("\d+", line)
        a = (int(a[0]), int(a[1]))
        line = input.readline()
        b = re.findall("\d+", line)
        b = (int(b[0]), int(b[1]))
        line = input.readline()
        prize = re.findall("\d+", line)
        prize = (int(prize[0]), int(prize[1]))
        
        return Claw(a, b, prize)
    
    def correct_solution(self, solution):
        button_a = round(solution[0])
        button_b = round(solution[1])
        return button_a * self.a[0] + button_b * self.b[0] == self.prize[0] \
            and button_a * self.a[1] + button_b * self.b[1] == self.prize[1]


def main():
    print("day 13")

    input = open("aoc_24/input/Day13.txt")
    # print(part1(input.read()))
    claws = []
    while True:
        line = input.readline()
        if line == "":
            break
        if line == "\n":
            continue
        line = line.strip("\n")
        
        claws.append(Claw.parse_claw(input, line))
        
        
    cost = [3, 1]
    sum = 0
    prizes = 0
    tolerance = 0.001
    for claw in claws:
        a = np.array(([claw.a[0], claw.b[0]],
                     [claw.a[1], claw.b[1]]))
        b = np.array(([claw.prize[0] + 10000000000000, claw.prize[1] + 10000000000000]))
        solution = np.linalg.solve(a, b)
        if abs(solution[0] - round(solution[0])) < tolerance and \
            abs(solution[1] - round(solution[1])) < tolerance:
                
            new_cost = round(solution[0]) * cost[0] + \
                round(solution[1]) * cost[1]               
            sum += new_cost
            prizes += 1
            # print(f"{a}, {b}, solution = {solution}, new cost: {new_cost}")
        # else:
            # print(f"{a}, {b}, solution = {solution}")
            # claw.correct_solution(solution)
                
    print(f"{sum}, prizes {prizes}")
    
if __name__=="__main__":
    main()