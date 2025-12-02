def read_input_file(file_path: str) -> list[str]:
    with open(file=file_path, mode="r") as input_file:
        line = input_file.readline()
        return line.strip().split(',')


def solution(lines: list[str]):
    invalid_sum = 0

    for line in lines:
        start, end = line.split('-')

        n_start, n_end = len(start), len(end)
        digit_range = list(range(n_start, n_end+1))

        if len(digit_range) == 1:
            if digit_range[0] % 2 != 0:
                continue

            n_start = len(start)
            n = n_start // 2

            left_start = int(start[:n])
            left_end = int(end[:n])
        else:
            for i in range(len(digit_range)):
                digits = digit_range[i]

                if digits % 2 != 0:
                    continue

                if i==0:
                    left_start = int(start[:digits//2])
                    left_end = int('9'*(digits//2))
                
                elif i==len(digit_range)-1:
                    left_start = 1
                    left_end = int(end[:digits//2])
                
                else:
                    left_start = 1
                    left_end = int('9'*(digits//2))
        
        while left_start <= left_end:
            invalid_number = int(f'{left_start}{left_start}')

            if int(start) <= invalid_number <= int(end):
                invalid_sum += invalid_number
            
            left_start += 1
        print(invalid_sum)
        
    print(invalid_sum)


lines = read_input_file(file_path="input/day2.txt")
solution(lines)
