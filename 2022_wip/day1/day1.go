package main

import (
	"fmt"
	"os"
	"slices"
	"strconv"
	"strings"
)

func part1(input string) int {
	elvesStr := strings.Split(input, "\n\n")
	elves := divideSnacks(elvesStr)

	return slices.Max(elves)
}

func part2(input string) int {
	elvesStr := strings.Split(input, "\n\n")
	elves := divideSnacks(elvesStr)

	// Now return the top 3
	slices.Sort(elves)
	return sum(elves[len(elves) - 3 : len(elves)])
}

func sum(slice []int) int {
	total := 0
	for _, item := range slice {
		total += item
	}
	return total
}

func divideSnacks(elvesStr []string) []int {
	elves := make([]int, 0)

	for _, elveStr := range elvesStr {
		// Skip empty entries
		if elveStr == "" {
			continue
		}

		calories := strings.Split(elveStr, "\n")
		total := 0
		for _, calorie := range calories {
			plus, err := strconv.Atoi(calorie)
			if err == nil {
				total += plus
			}
		}
		elves = append(elves, total)
	}
	return elves
}

func check(e error) {
    if e != nil {
        panic(e)
    }
}

func main() {
	data, err := os.ReadFile("../input/day1.txt")
	check(err)

    str := string(data) // convert content to a 'string'

	resultPart1 := part1(str)
	fmt.Println("Result of part 1: ", resultPart1)

	resultPart2 := part2(str)
	fmt.Println("Result of part 2: ", resultPart2)
}
