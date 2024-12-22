import time
from multiprocessing import Pool
import numpy as np

def step_1(number):
    return prune(mix(number, number * 64))    

def step_2(number):
    return prune(mix(number, int(number / 32)))

def step_3(number):
    return prune(mix(number, number * 2048))

def prune(number):
    return number % 16777216

def mix(number, mix_number):
    return number ^ mix_number

def new_secret_number(number):
    return step_3(step_2(step_1(number)))

def update_secret_number_n_times(number, times):
    for _ in range(times):
        number = new_secret_number(number)
    
    return number

def part_1(numbers):
    numbers_2000 = ([update_secret_number_n_times(number, 2000) for number in numbers])
    print(numbers_2000)
    print(sum(numbers_2000))
    return sum(numbers_2000)

# ___________Part 2 ________________

def last_digit(number):
    return number % 10

def get_all_last_digits(number):
    last_digits_number_x = []
    for _ in range(2000):
        number = new_secret_number(number)
        last_digits_number_x.append(last_digit(number))
    return last_digits_number_x
        
def calculate_change(price_list):
    changes = []
    for price_prev, price_curr in zip(price_list[:-1], price_list[1:]):
        changes.append(price_curr - price_prev)
    return changes

def get_sequences(price_changes):
    sequences = np.zeros((4,), dtype=np.uint8)
    for id, _ in enumerate(price_changes):
        if id < 4:
            continue
        new_sequence = price_changes[id - 4:id]
        sequences = np.vstack((sequences, new_sequence))
    # Remove the first row with zeros before returning
    return sequences[1:, :]

# Sequences with length 4
def get_distinct_sequences(price_changes):
    uniques = np.unique(get_sequences(price_changes), axis=0)
    return uniques

def prune_sequences(sequences):
    # Check for rows where the last element is greater than 0
    pruned_indices = np.greater(sequences, np.array([-10, -10, -10, 0]))
    pruned_rows = np.all(pruned_indices, axis=1)
    return sequences[pruned_rows, :]
        
def generate_and_prune_sequences(price_changes):
    distinct_sequences = np.zeros((4,), dtype=np.uint8)
    for price_change_list in price_changes:
        distinct_sequences = \
        np.unique(
            np.vstack(
                (distinct_sequences,get_distinct_sequences(price_change_list))
            ), axis=0
        )
    print(f"Before pruning sequences: {len(distinct_sequences)}")
    
    pruned_sequences = prune_sequences(distinct_sequences)
    print(f"After pruning sequences: {len(pruned_sequences)}")
    return pruned_sequences

def first_id_sequence_in_list(sequence, price_change):
    price_changes_sequences = get_sequences(price_change)
    
    # Check exists for potential speed up
    if not any(np.equal(price_changes_sequences,sequence).all(1)):
        return -1
    
    for id, row in enumerate(price_changes_sequences):
        # print(f"Price changes: {price_change[id:id+4]} = sequence {row}")
        if np.all(row == sequence):
            # sequences start at id 0, which means element 4 is the last one
            # print(f"Fourn sequence: {sequence} in row: {row}")
            return id + 4
    return -1

def first_id_sequence_in_list_cached(sequence, price_change_sequences):   
    # Check exists for potential speed up
    if not any(np.equal(price_change_sequences,sequence).all(1)):
        return -1
    
    for id, row in enumerate(price_change_sequences):
        # print(f"Price changes: {price_change[id:id+4]} = sequence {row}")
        if np.all(row == sequence):
            # sequences start at id 0, which means element 4 is the last one
            # print(f"Fourn sequence: {sequence} in row: {row}")
            return id + 4
    return -1
  
# Sort sequences by the value each rows last element
def sort_sequences_by_last_element(sequences):
    sort_indices=np.argsort(sequences[:,-1])
    return sequences[sort_indices[::-1]]
  
def sequence_profit(sequence, price_lists, price_changes):
    sum = 0
    for price_list, price_change in zip(price_lists, price_changes):
        id = first_id_sequence_in_list(sequence, price_change)
        # Need to check for offset in this sequence finding function
        if id > 0:
            # Debug prints
            # print(f"Found {sequence} in list {price_change[id-4:id]}")
            # print(f"Price changes {price_change[id-4:id]}, corresponds to prices: {price_list[id-4:id+1]}")
            # Debug assert
            assert(price_list[id] - price_list[id-1] == price_change[id-1])
            sum += price_list[id]
    return sum

def find_best_sum(sequences, price_lists, price_changes):
    best_sequence = sequences[0]
    best_sum = 0
    for id, sequence in enumerate(sequences):
        # if id % 100 == 0:
        #     print(f"id: {id} / {len(sequences)}")
        sequence_sum = sequence_profit(sequence, price_lists, price_changes)
        if sequence_sum > best_sum:
            best_sum = sequence_sum
            best_sequence = sequence
            print(f"New best sequence {best_sequence}, sum: {best_sum}")
            
    print(f"Finished, new best sum {best_sum}, from sequence: {best_sequence}")
    return best_sum
  
def part_2(numbers):
    price_lists = np.array([get_all_last_digits(number) for number in numbers])
    price_changes = np.array([calculate_change(price_list) for price_list in price_lists])   
    sequences = sort_sequences_by_last_element(generate_and_prune_sequences(price_changes))
        
    best_sum = find_best_sum(sequences, price_lists, price_changes)
            
    return best_sum

def sequence_profit_multithread(sequence):
    sum = 0
    # print(f"Started sequence: {sequence}")
    for price_list, price_change_sequence in zip(globals()['price_lists'], globals()['price_change_sequences']):
        id = first_id_sequence_in_list_cached(sequence, price_change_sequence)
        # Need to check for offset in this sequence finding function
        if id > 0:
            # Debug prints
            # print(f"Found {sequence} in list {price_change[id-4:id]}")
            # print(f"Price changes {price_change[id-4:id]}, corresponds to prices: {price_list[id-4:id+1]}")
            # Debug assert
            # assert(price_list[id] - price_list[id-1] == price_change[id-1])
            sum += price_list[id]
    return sum

def find_best_sum_multithread(sequences):
    best_sum = 0
    with Pool(10) as pool:
        return max(pool.map(sequence_profit_multithread, sequences))
    
    # for id, sequence in enumerate(sequences):
    #     if id % 1 == 0:
    #         print(f"id: {id} / {len(sequences)}")
    #     sequence_sum = sequence_profit_multithread(sequence)
    #     if sequence_sum > best_sum:
    #         best_sum = sequence_sum
    #         best_sequence = sequence
    #         print(f"New best sequence {best_sequence}, sum: {best_sum}")
            
    # print(f"Finished, new best sum {best_sum}, from sequence: {best_sequence}")
    return best_sum
 
def part_2_multithread(numbers):
    global price_lists
    price_lists = np.array([get_all_last_digits(number) for number in numbers])
    global price_changes
    price_changes = np.array([calculate_change(price_list) for price_list in price_lists])  
    global price_change_sequences
    price_change_sequences = [get_sequences(price_change) for price_change in price_changes]
     
    sequences = sort_sequences_by_last_element(generate_and_prune_sequences(price_changes))
        
    best_sum = find_best_sum_multithread(sequences)
    print(best_sum)
    return best_sum

def test_part_2():
    test_numbers_part_2 = [1, 2, 3, 2024]
    start = time.time()
    assert(part_2(test_numbers_part_2) == 23)
    print(f"Time for test input 4 monkeys: {time.time() - start}")

def main():
    print("day 22")
    input = open("aoc_24/input/Day22.txt")
    numbers = [int(line) for line in input.readlines()]
    # print(numbers)
    
    assert(mix(42, 15) == 37)
    assert(prune(100000000) == 16113920)
    assert(update_secret_number_n_times(123, 1) == 15887950)
    
    assert(part_1([1, 10, 100, 2024]) == 37327623)
    # part_1(numbers)
    
    # test_part_2()
    
    test_numbers_part_2 = [1, 2, 3, 2024]
    assert(part_2_multithread(test_numbers_part_2) == 23)
    
    # start = time.time()
    # part_2_multithread(numbers[:10])
    # print(f"Time for multithreaded 10 monkeys: {time.time() - start}")
    
    # start = time.time()
    # part_2_multithread(numbers[:100])
    # print(f"Time for multithreaded 100 monkeys: {time.time() - start}")
      
    start = time.time()
    result = part_2_multithread(numbers)
    print(f"Time for full 2000 monkeys: {time.time() - start}")
    print(result)
    # start = time.time()
    # part_2(numbers[:10])
    # print(f"Time for 10 monkeys: {time.time() - start}")
    
if __name__ == "__main__":
    main()