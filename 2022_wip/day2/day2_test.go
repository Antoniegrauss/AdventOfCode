package main

import "testing"

func Test_part1(t *testing.T) {
	tests := []struct {
		name string // description of this test case
		// Named input parameters for target function.
		input string
		want  int
	}{
		// TODO: Add test cases.
		{"Test input",
		"A Y\nB X\nC Z",
		15},
		{"Draws",
		"A X\nB Y\nC Z",
		1  + 2 + 3 + 3 * 3},
		{"Losses",
		"A Z\nB X\nC Y",
		1  + 2 + 3 + 3 * 0},
		{"Wins",
		"A Y\nB Z\nC X",
		1  + 2 + 3 + 3 * 6},
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			got := part1(tt.input)

			if got != tt.want {
				t.Errorf("part1() = %v, want %v", got, tt.want)
			} else {
				t.Log(tt.input, " correct result = ", tt.want)
			}
		})
	}
}

func Test_part2(t *testing.T) {
	tests := []struct {
		name string // description of this test case
		// Named input parameters for target function.
		input string
		want  int
	}{
		// TODO: Add test cases.
		{"Test input",
		"A Y\nB X\nC Z",
		12},
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			got := part2(tt.input)

			if got != tt.want {
				t.Errorf("part1() = %v, want %v", got, tt.want)
			} else {
				t.Log(tt.input, " correct result = ", tt.want)
			}
		})
	}
}
