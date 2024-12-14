from itertools import product
import graphviz
class Operation():
    def __init__(self, input_1: str, operation: str, input_2: str, output: str, values_dict) -> None:
        self.input_1 = input_1 
        self.input_2 = input_2 
        self.operation = operation 
        self.output = output
        self.values_dict = values_dict
        
        self.output_value = None
        
    def reset(self):
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

def swap_outputs(one, other):
    temp = one.output
    one.output = other.output
    other.output = temp
    
def swap_by_name(operations, a, b):
    operation_a = [operation for operation in operations if operation.output == a[2]
                   and (operation.input_1 == a[0] or operation.input_2 == a[0]) 
                   and (operation.input_2 == a[1] or operation.input_1 == a[1])][0]
    operation_b = [operation for operation in operations if operation.output == b[2]
                   and (operation.input_1 == b[0] or operation.input_2 == b[0]) 
                   and (operation.input_2 == b[1] or operation.input_1 == b[1])][0]
    swap_outputs(operation_a, operation_b)

def create_graph(operation_list):
    graph = graphviz.Digraph()
    # Make sure the z nodes line up
    with graph.subgraph() as sub:
        # sub.attr(rank='same')
        for i in range(46):
            if i < 10:
                i = "0" + str(i)
            sub.node('z' + str(i), style='filled', fillcolor='#40e0d0')
    with graph.subgraph() as sub:
        # sub.attr(rank='same')
        for i in range(45):
            if i < 10:
                i = "0" + str(i)
            sub.node('x' + str(i), style='filled', fillcolor='#ff000042') 
    with graph.subgraph() as sub:
        # sub.attr(rank='same')
        for i in range(45):
            if i < 10:
                i = "0" + str(i)
            sub.node('y' + str(i), style='filled', fillcolor='deeppink')
    for operation in operation_list:
        graph.node(operation.input_1)
        graph.node(operation.input_2)
        graph.edge(operation.input_1, operation.output, label=operation.operation)
        graph.edge(operation.input_2, operation.output, label=operation.operation)
    return graph

def print_diagnostics(values_dict):
    x_bits = get_bitstring_for('x', values_dict)
    y_bits = get_bitstring_for('y', values_dict)
    print(f"X number:\n  {x_bits}, {int(x_bits, 2)}")
    print(f"Y number:\n  {y_bits}, {int(y_bits, 2)}")
    good_result_string = str(bin(int(x_bits, 2) + int(y_bits, 2)))[2:]
    print(f"Good result:\n {good_result_string}, {int(good_result_string, 2)}")
    actual_result_string = get_bitstring_for('z', values_dict)
    print(f"Actual result:\n {actual_result_string}, {int(actual_result_string, 2)}")
    
    print(f"Good bits: {bit_similarity_score(actual_result_string, good_result_string)} / {len(good_result_string)}")

def reset_all_operations(operation_list):
    for operation in operation_list:
        operation.reset()
    
def part_2(input):
    variables, operations = input.split("\n\n")
    values_dict = parse_values_dict(variables)
    
    # Debugging
    for i in range(45):
        num_str = str(i)
        if i < 10:
            num_str = '0' + num_str
        values_dict['x' + num_str] = False
        values_dict['y' + num_str] = True
        
    operation_list = parse_operations(operations, values_dict)
    # do_all_operations(operation_list)
    
    combinations = product(operation_list, operation_list)
    print(f"Amount of pairs: {len(list(combinations))}")
    
    # print_diagnostics(values_dict)
    
    # wrong = ['wtf', 'fjk', 'fck']
    # wrong_output = ['z39', 'z41', 'z42']
    swap_by_name(operations=operation_list, a=('sth', 'bhw', 'z15'), b=('sth', 'bhw', 'htp'))
    swap_by_name(operations=operation_list, a=('qfj', 'mqg', 'z20'), b=('fvm', 'mvv', 'hhh'))
    swap_by_name(operations=operation_list, a=('x05', 'y05', 'z05'), b=('hdc', 'gcs', 'dkr'))
    swap_by_name(operations=operation_list, a=('x36', 'y36', 'ggk'), b=('x36', 'y36', 'rhv'))
    print(",".join(sorted(["z15","z20","z05","ggk","htp","hhh","dkr","rhv"])))

    reset_all_operations(operation_list)
    do_all_operations(operation_list)

    graph = create_graph(operation_list)
    graph.render(directory='doctest-output', engine='dot').replace('\\', '/')
    print_diagnostics(values_dict)
    
    print(f"carry 15 {values_dict['sth']}, x: {values_dict['x15']}, y: {values_dict['y15']}")
    print(f"z: {values_dict['z16']}, x: {values_dict['x16']}, y: {values_dict['y16']}, carry: {values_dict['mqr']}")
    print(f"bhw: {values_dict['bhw']}, htp: {values_dict['htp']}")
    
    for i in range(13,17):
        num = str(i)
        print(f"{num} \nx {int(values_dict['x' + num])}, \ny {int(values_dict['y' + num])}, \nz: {int(values_dict['z' + num])}")
    
    # print(values_dict['z02'], values_dict['x02'], values_dict['y02'], values_dict['z01'])

def main():
    print("day 24")
    # test_input = open("aoc_24/input/Day24_test.txt").read()
    # assert(part_1(test_input) == 2024)
    
    # real_input = open("aoc_24/input/Day24.txt").read()
    # assert(part_1(real_input) == 53755311654662)
    # print(values_dict)
    
    mini_input = open("aoc_24/input/Day24.txt").read()
    part_2(mini_input)

if __name__ == "__main__":
    main()