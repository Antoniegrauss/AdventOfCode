def setup_disk(input):
    disk = []
    is_free_space = False
    current_id = 0
    for character in input:
        if is_free_space:
            for _ in range(int(character)):
                disk.append(-1)
        else:
            for _ in range(int(character)):
                disk.append(current_id)
            current_id += 1
        is_free_space = not is_free_space

    return disk

def sort_disk_part_1(disk):
    try:
        while True:
            first_free_space_id = disk.index(-1)
            disk[first_free_space_id] = disk[-1]
            disk.pop(-1)
    except ValueError:
        return disk

def setup_disk_linked_list(input):
    disk = []
    is_free_space = False
    current_id = 0
    for character in input:
        if not disk:
            disk.append(Block(free=is_free_space, size=int(character), file_id=current_id))
        elif is_free_space:
            # Skip free spaces with size 0
            if int(character) > 0:
                disk.append(Block(free=is_free_space, size=int(character)))
                disk[-1].previous = disk[-2]
                disk[-2].next = disk[-1]
        else:
            disk.append(Block(free=is_free_space, size=int(character), file_id=current_id))
            disk[-1].previous = disk[-2]
            disk[-2].next = disk[-1]
        
        if not is_free_space:
            current_id += 1
        is_free_space = not is_free_space

    return disk

class Block():
    def __init__(self, free:bool, size:int, file_id=-1) -> None:
        self.free=free
        self.size=size
        self.file_id=file_id

        self.previous = None
        self.next = None

    def __str__(self) -> str:
        return str((self.file_id, self.size))
    
    def checksum(self, start_id) -> int:
        if self.file_id <= 0:
            return 0
        
        sum = 0
        for i in range(self.size):
            sum += self.file_id * (i + start_id)

        return sum

def print_disk_list(disk):
    text = ",".join([str(x) for x in disk])
    print(text)

def print_linked_list(current):
    text = ""
    while current is not None:
        text += str(current)
        current = current.next
    print(text)

def first_free_space_with_size(disk, min_size, max_file_id):
    current = disk[0]
    while current is not None:
        # Free space must be to the left of current file to move
        if current.file_id == max_file_id:
            return None
        if current.free and current.size >= min_size:
            return current
        current = current.next
        
    return None

def find_file_id(current, file_id):
    while current is not None:
        if current.file_id == file_id:
            return current
        current = current.next
        
    return None

def swap_file_for_new_free(file):
    # Create new free block
    new_free_space = Block(free=True, size=file.size)

    # Insert new free space where file was
    file.previous.next = new_free_space
    new_free_space.previous = file.previous
    if file.next:
        new_free_space.next = file.next
        file.next.previous = new_free_space

def move_file_into_free_space(file, free_space):
    swap_file_for_new_free(file)

    # Insert file before free space
    file.previous = free_space.previous
    if file.previous:
        file.previous.next = file
    
    # Check whether exact same size, then free space is gone
    if file.size == free_space.size:
        if free_space.next:
            free_space.next.previous = file
        file.next = free_space.next
    else:
        free_space.size -= file.size
        file.next = free_space
        free_space.previous = file


def main():
    input = open("aoc_24/input/Day9.txt")
    assignment = input.readline().strip("\n")   
    # input_test = "2333133121414131402"

    disk = setup_disk_linked_list(assignment)
        
    print_disk_list(disk)

    # Loop backwards over the files (once per each file)
    # Move them to the first free space
    current_file = disk[-1]
    current_file_id = current_file.file_id

    while current_file_id >= 0:
        free_space_large_enough = first_free_space_with_size(disk=disk, min_size=current_file.size, max_file_id=current_file_id)
        if free_space_large_enough is not None:
            # print(f"Moving file id {current_file_id}")
            move_file_into_free_space(file=current_file, free_space=free_space_large_enough)
            # print_linked_list(disk[0]) 
            
        current_file_id -= 1
        current_file = find_file_id(current=disk[0], file_id=current_file_id)
            
    
    file_position = 0
    checksum = 0
    current_file = disk[0]
    while current_file is not None:
        if current_file.file_id > 0:
            checksum += current_file.checksum(file_position)
        file_position += current_file.size
        current_file = current_file.next

    print(f"Checksum: {checksum}")

if __name__=="__main__":
    main()