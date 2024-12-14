def parse_input(input):
    rules = []
    updates = []

    while line := input.readline():
        if line == "\n":
            continue
        if line.find("|") != -1:
            rules.append(line.strip("\n").split("|"))
        else:
            updates.append(line.strip("\n").split(","))

    return rules, updates

def update_satiesfies_rule(update, rule):
    if rule[0] not in update or rule[1] not in update:
        return True
    
    # First number must be present before second one
    return update.index(rule[0]) < update.index(rule[1])

def satiesfies_all_rules(update, rules):
    for rule in rules:
        if not update_satiesfies_rule(update, rule):
            return False
    return True

def fix_ordering(update, rule):
    update.pop(update.index(rule[0]))
    update.insert(update.index(rule[1]), rule[0])
    return update

def make_update_satisfy_rules(update, rules):
    for rule in rules:
        if not update_satiesfies_rule(update, rule):
            print(f"Before reordering: {update}, {rule}")
            update = fix_ordering(update, rule)
            print(f"Reordered update: {update}, {rule}")
            update = make_update_satisfy_rules(update, rules)

    return update

def get_middle(update):
    return int(update[len(update) // 2])

def main():
    print("day 5")
    input = open("aoc_24/input/Day5.txt")

    rules, updates = parse_input(input)

    correct_updates_middle_pages_sum = 0
    for update in updates:
        if not satiesfies_all_rules(update, rules):
            # Add middle element value to sum
            update_ordered = make_update_satisfy_rules(update, rules)
            correct_updates_middle_pages_sum += get_middle(update_ordered)
            print(f"New sum: {correct_updates_middle_pages_sum}, added {get_middle(update_ordered)}, from list {update}")
        else:
            print("Satisfied all rules, doing nothing")

if __name__ == "__main__":
    main()
