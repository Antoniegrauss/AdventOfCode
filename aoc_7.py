def check_possible(answer, numbers):
    results = [numbers[0]]
    numbers = numbers[1:]
    while numbers:
        new_results = []
        for id, result in enumerate(results):
            multiply = result * numbers[0]
            if multiply <= answer:
                new_results.append(multiply)
            results[id] += numbers[0]
            concat = int(str(result) + str(numbers[0]))
            if concat <= answer:
                new_results.append(concat)
        results += new_results
        numbers = numbers[1:]

    return answer in results

def main():
    print("day7")

    input = open("aoc_24/input/Day7.txt")
    lines = input.readlines()

    answers = []
    numbers = []

    for line in lines:
        parts = line.split(":")
        answers.append(int(parts[0]))
        numbers.append([int(x) for x in parts[1].split(" ")[1:]])

    sum = 0
    for answer, number_list in zip(answers, numbers):
        if check_possible(answer, number_list):
            sum += answer

    print(f"Possible equations: {sum}")

if __name__ == "__main__":
    main()