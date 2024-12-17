class Program():
    def __init__(self, a, b, c, instructions) -> None:
        self.a = a
        self.b = b
        self.c = c
        self.instructions = instructions
        self.instruction_pointer = 0
        self.start_a = 0
        
        self.output = []
        
    def perform_operations_part_1(self):
        self.output = []
        self.instruction_pointer = 0
        while self.instruction_pointer < len(self.instructions):
            should_jump = self.do_instruction()
            if should_jump:
                self.instruction_pointer += 2
        return self.output
        
    def perform_operations_part_2(self):
        self.output = []
        self.instruction_pointer = 0
        while self.instruction_pointer < len(self.instructions):
            should_jump = self.do_instruction()
            outputs = len(self.output)
            if outputs > 0:
                if self.output != self.instructions[0:outputs]:
                    return len(self.output) - 1
            if should_jump:
                self.instruction_pointer += 2
        return len(self.output)
        
    def combo_operand(self):
        num = self.instructions[self.instruction_pointer + 1]
        if num in [0, 1, 2, 3]:
            return num
        if num == 4:
            return self.a
        if num == 5:
            return self.b
        if num == 6:
            return self.c
        
    def literal_operand(self):
        return self.instructions[self.instruction_pointer + 1]
        
    def do_instruction(self) -> bool:
        should_jump = True
        instruction = self.instructions[self.instruction_pointer]
        print(f"{instruction}, {self.instructions[self.instruction_pointer]}")
        if instruction == 0:
            self.a = int(self.a / (2 ** self.combo_operand()))
        elif instruction == 1:
            self.b = int(self.b ^ self.literal_operand())
        elif instruction == 2:
            self.b = int(self.combo_operand() % 8)
        elif instruction == 3:
            if self.a == 0:
                pass
            else:
                self.instruction_pointer = self.literal_operand()
                should_jump = False
        elif instruction == 4:
            self.b = int(self.b ^ self.c)
        elif instruction == 5:
            self.output.append(int(self.combo_operand() % 8))
        elif instruction == 6:
            self.b = int(self.a / (2 ** self.combo_operand()))
        elif instruction == 7:
            self.c = int(self.a / (2 ** self.combo_operand()))
            
        return should_jump
        
    def advance_pointer(self):
        self.instruction_pointer += 2
        
    def set_registers(self, a, b, c):
        self.start_a = a
        self.a = self.start_a
        self.b = b
        self.c = c

def add_chunks(counter, sets):
    print(bin(counter))
    reversed = bin(counter)[::-1][:-2]
    for chunk_id, index in enumerate(range(0, len(reversed) - 2, 3)):
        # Chunk binary in 3-bit sizes
        chunk = reversed[index:index+3]
        if len(chunk) != 3:
            break
        if (chunk_id) > len(sets) - 1:
            sets.append(set())
        sets[chunk_id].add(chunk)

def main():
    print("Day 16")
    input = (open("aoc_24/input/Day17.txt").readlines())
    a = int(input[0].split(" ")[-1])
    b = int(input[1].split(" ")[-1])
    c = int(input[2].split(" ")[-1])
    program = [int(x) for x in input[-1].split(" ")[1].split(",")]
    print(program)
    
    program = Program(a, b, c, program)
    
    # Part 1
    output = program.perform_operations_part_1()
    print(output)
    
    # Part 2
    # Reverse the operations, given the outputs
    
    # counter = 0
    # chunk_counter = 0
    # increment = 2 ** (3*chunk_counter)
    
    # max_score = 1
    # last_bit_same_times = 0
    # last_bit_previous = 0
    
    # # Increments each time we find a chunk
    # looking_for_chunk = 1
    # while True:
    #     program.set_registers(counter, 0, 0)
    #     score = program.perform_operations_part_2()
    #     if score > 1:
    #         print(f"{score}, {bin(counter)}")
    #         max_score = score
            
    #         last_unknown_chunk = counter % (2**(3*looking_for_chunk))
    #         if last_unknown_chunk == last_bit_previous:
    #             last_bit_same_times += 1
    #         else:
    #             last_bit_same_times = 0
    #             last_bit_previous = last_unknown_chunk
                
    #         # If we cycled a whole chunk with no changes
    #         # Add the chunk to the part we keep the same
    #         if last_bit_same_times == 8:
    #             last_bit_same_times = 0
    #             chunk_counter += 1
    #             increment = 2 ** (3*chunk_counter)
        
    #     counter += increment
    #     if counter == 100:
    #         break
    # print(f"Found a solution: {counter}, score: {max_score}")
        
if __name__== "__main__":
    main()