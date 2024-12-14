sum = 0
left_list = []
right_list = []
with open("InputDay1.txt") as file:
    while line := file.readline():
        split = line.split()
        if len(split) != 2:
            continue
        left_list.append(int(split[0]))
        right_list.append(int(split[1]))

left_list.sort()
right_list.sort()

for left, right in zip(left_list, right_list):
    sum += left * right_list.count(left)

print(sum)
