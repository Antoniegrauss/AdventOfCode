import networkx as nx

def read_connections(connections):
    nodes = {}
    for connection in connections:
        start = connection[0]
        end = connection[1]
        if start not in nodes:
            nodes[start] = [end]
        elif end not in nodes[start]:
            nodes[start] += [end]
        
        if end not in nodes:
            nodes[end] = [start]
        elif start not in nodes[end]:
            nodes[end] += [start]
    return nodes

def part_1(nodes):
    cycle_set = set()
    for key in nodes:
        first_edges = nodes[key]
        for first_edge in first_edges:
            second_edges = nodes[first_edge]
            for second_edge in second_edges:
                third_edges = nodes[second_edge]
                if key in third_edges:
                    if 't' == key[0] or \
                        't' == first_edge[0] or \
                        't' == second_edge[0]:
                        cycle_set.add(tuple(sorted([key, first_edge, second_edge])))

    return cycle_set

def new_node_fits_in_cycle(new_node, cycle, nodes):
    if new_node in cycle:
        return False
    for cycle_node in cycle:
        # If any of the cycle nodes is not connected to this on
        # Skip it
        if new_node not in nodes[cycle_node]:
            return False
    return True

def grow_cycle(cycle, nodes):
    for new_node in nodes:
        if new_node_fits_in_cycle(new_node, cycle, nodes):
            return cycle + (new_node,)
    return None

def main():
    print("day 23")
    input = open("aoc_24/input/Day23.txt")
    
    connections = [line.strip("\n").split("-") for line in input.readlines()]
    nodes = read_connections(connections)

    cycle_set = part_1(nodes) 
    assert(len(cycle_set) == 1163)
    
    # Start a new cycle set for each node
    cycle_set = set()
    for key in nodes:
        cycle_set.add((key,))
        
    # Grow cycles until there is only 1 left
    while True:
        cycles = [grow_cycle(cycle, nodes) for cycle in cycle_set]
        cycle_set = set()
        for cycle in cycles:
            if cycle:
                cycle_set.add(tuple(sorted(cycle)))
        if len(cycle_set) == 1:
            print(list(cycle_set)[0])
            print(",".join(sorted(list(cycle_set)[0])))
            break
        if len(cycle_set) == 0:
            print("Oopsie, we emptied the set")
            break
        if list(cycle_set)[0] and len(list(cycle_set)[0]) >= 13:
            print(cycle_set[0])
            print(",".join(sorted(list(cycle_set)[0])))
            print("Max possible cycle length reached")
            break

if __name__ == "__main__":
    main()