class Program():
    def __init__(self, a, b, c, instructions) -> None:
        self.a = a
        self.b = b
        self.c = c
        self.instructions = instructions
        self.instruction_pointer = 0
        
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
        # print(f"{instruction}, {self.instructions[self.instruction_pointer + 1]}" +
        #       f", a: {self.a}, b: {self.b}, c: {self.c}")
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
        self.a = a
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

def opcode(reg, prog, verbose=False, part=1):
    i = 0
    output = []

    while True:    
        op = prog[i]
        li = prog[i+1]
        
        # combo from literal
        co = li 
        if co>3 and co<7:
            co=reg[co-4]        
        
        if verbose:
            print(f"{i} | {op} : {co} | ",end="")
        
        if op==0: # adv
            reg[0] = reg[0] // 2**co  
        elif op==1: # bxl
            reg[1] = reg[1] ^ li 
        elif op==2: # bst
            reg[1] = co % 8
        elif op==3: # jnz
            if reg[0]!=0: 
                i = li - 2
        elif op==4: # bxc
            reg[1] = reg[1] ^ reg[2]
        elif op==5: # out
            output += [ co % 8 ]
            #if part==2 and output != prog[:len(output)]:
            #    return None, None
        elif op==6: # bdv
            reg[1] = reg[0] // 2**co  
        elif op==7: # cdv
            reg[2] = reg[0] // 2**co  

        if verbose:
            print(reg)

        i+=2
        if i>=len(prog):
            break
    return output,reg

def findA(program, a=0, b=0, c=0, ip=-1):
    if abs(ip) > len(program.instructions): 
      return a
    for i in range(8):
        aa = a * 8 + i
        program.set_registers(aa, b, c)
        output = program.perform_operations_part_1()
        if output[0]==program.instructions[ip]:
            print(ip)
            aa = findA(program, aa, program.b, program.c, ip-1)
            if aa:
                return aa
    return None

def main():
    print("Day 16")
    input = (open("aoc_24/input/Day17.txt").readlines())
    a = int(input[0].split(" ")[-1])
    b = int(input[1].split(" ")[-1])
    c = int(input[2].split(" ")[-1])
    program = [int(x) for x in input[-1].split(" ")[1].split(",")]
    print(program)
    
    program = Program(a, b, c, program)
    
    program.set_registers(0, 0, 0)
    print(findA(program))
    # print(findA_copy(program.instructions))
        
        
if __name__== "__main__":
    main()