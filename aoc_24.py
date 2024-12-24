from itertools import product

class Operation():
    def __init__(self, input_1: str, operation: str, input_2: str, output: str, values_dict) -> None:
        self.input_1 = input_1 
        self.input_2 = input_2 
        self.operation = operation 
        self.output = output
        self.values_dict = values_dict
        
        self.output_value = None
        
    def can_do_operation(self):
        if self.output_value:
            return False
        return self.input_1 in self.values_dict and self.input_2 in self.values_dict
    
    def do_operation(self):
        if not self.can_do_operation():
            return False
        
        input_1_value = self.values_dict[self.input_1]
        input_2_value = self.values_dict[self.input_2]
        
        if self.operation == 'XOR':
            self.output_value = input_1_value != input_2_value
        elif self.operation == 'AND':
            self.output_value = input_1_value and input_2_value
        elif self.operation == 'OR':
            self.output_value = input_1_value or input_2_value
            
        assert(self.output_value is not None)
        self.values_dict[self.output] = self.output_value
        
        return True

def get_bitstring_for(character, values_dict):
    bit_string = ""
    for key in sorted(list(values_dict.keys()), reverse=True):
        if character in key:
            bit_string += str(int(values_dict[key]))
    return bit_string

def do_all_operations(operations):
    not_done = operations
    while not_done:
        not_done = [operation for operation in not_done if not operation.do_operation()]

def parse_values_dict(variables):
    values_dict = {}
    variables = variables.strip(" ").split("\n")
    
    values_dict = {}
    for variable in variables:
        key, value = variable.split(":")
        values_dict[key] = bool(int(value.strip(" ")))
        
    return values_dict

def parse_operations(operations, values_dict):
    operations = operations.strip(" ").split("\n")
    operations_list = []
    for operation in operations:
        parts = operation.split(" ")
        assert(len(parts) == 5)
        operations_list.append(Operation(parts[0], parts[1], parts[2], parts[4], values_dict))
    return operations_list

def part_1(input):
    variables, operations = input.split("\n\n")
    values_dict = parse_values_dict(variables)
        
    operation_list = parse_operations(operations, values_dict)
    do_all_operations(operation_list)
        
    bit_string = get_bitstring_for('z', values_dict)
        
    return int(bit_string, 2)

def bit_similarity_score(a, b):
    return sum([int(a_x == b_x) for a_x, b_x in zip(a, b)])

def swap_outputs()

def part_2(input):
    variables, operations = input.split("\n\n")
    values_dict = parse_values_dict(variables)
        
    operation_list = parse_operations(operations, values_dict)
    do_all_operations(operation_list)
    
    combinations = product(operation_list, operation_list)
    print(f"Amount of pairs: {len(list(combinations))}")
    
    x_bits = get_bitstring_for('x', values_dict)
    y_bits = get_bitstring_for('x', values_dict)
    print(f"X number:\n {x_bits}, {int(x_bits, 2)}")
    print(f"Y number:\n {y_bits}, {int(y_bits, 2)}")
    good_result_string = str(bin(int(x_bits, 2) + int(y_bits, 2)))[2:]
    print(f"Good result:\n {good_result_string}, {int(good_result_string, 2)}")
    actual_result_string = get_bitstring_for('z', values_dict)
    print(f"Actual result:\n {actual_result_string}, {int(actual_result_string, 2)}")
    
    print(f"Good bits: {bit_similarity_score(actual_result_string, good_result_string)} / {len(good_result_string)}")

def main():
    print("day 24")
    test_input = open("aoc_24/input/Day24_test.txt").read()
    assert(part_1(test_input) == 2024)
    
    real_input = open("aoc_24/input/Day24.txt").read()
    assert(part_1(real_input) == 53755311654662)
    # print(values_dict)
    
    part_2(real_input)

if __name__ == "__main__":
    main()