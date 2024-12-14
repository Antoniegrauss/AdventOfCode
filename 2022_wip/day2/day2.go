package main

import (
	"fmt"
	"os"
	"strings"
)

type RockPaperScissors int

const (
	Rock = 1
	Paper = 2
	Scissors = 3
)

func (move RockPaperScissors) WinningMove() RockPaperScissors {
	switch move {
	case Rock:
		return Paper
	case Paper:
		return Scissors
	case Scissors:
		return Rock
	default:
		return Rock
	}
}

func (move RockPaperScissors) LosingMove() RockPaperScissors {
	switch move {
	case Rock:
		return Scissors
	case Paper:
		return Rock
	case Scissors:
		return Paper
	default:
		return Rock
	}
}

type Game struct {
	us RockPaperScissors
	them RockPaperScissors
}

func (game Game) gameScore() int {
	return int(game.us) + game.result();
}

func parseMove(move string) RockPaperScissors {
	switch move{
	case "A":
		return Rock
	case "B":
		return Paper
	case "C":
		return Scissors
	case "X":
		return Rock
	case "Y":
		return Paper
	case "Z":
		return Scissors
	default:
		return Rock
	}
}

func parseGame(game string) Game {
	parts := strings.Split(game, " ")
	return Game{them:parseMove(parts[0]), us:parseMove(parts[1])}
}

func parseGamePart2(game string) Game {
	parts := strings.Split(game, " ")
	them := parseMove(parts[0])
	
	us := them
	switch parts[1] {
	case "X":
		us = them.LosingMove()
	case "Y":
		us = them
	case "Z":
		us = them.WinningMove()
	}
	return Game{them:them, us:us}
}

func parseGames(input string) []Game {
	gamesStr := strings.Split(input, "\n")
	games := make([]Game, 0)

	for _, gameStr := range gamesStr {
		games = append(games, parseGame(gameStr))
	}
	return games
}

func parseGamesPart2(input string) []Game {
	gamesStr := strings.Split(input, "\n")
	games := make([]Game, 0)

	for _, gameStr := range gamesStr {
		games = append(games, parseGamePart2(gameStr))
	}
	return games
}

func (game Game) result() int {
	// 0 for loss, 3 for draw, 6 for win
	if game.us == game.them {
		return 3
	}
	if game.us == Rock {
		if game.them == Paper {
			return 0
		}
		return 6
	}
	if game.us == Paper {
		if game.them == Scissors {
			return 0
		}
		return 6
	}
	if game.us == Scissors {
		if game.them == Rock {
			return 0
		}
		return 6
	}
	fmt.Println("Should not get here?")
	return -1
}

func part1(input string) int {
	games := parseGames(input)
	total := 0
	for _, game := range games {
		total += game.gameScore();
	}
	return total
}

func part2(input string) int {
	games := parseGamesPart2(input)
	total := 0
	for _, game := range games {
		total += game.gameScore();
	}
	return total
}

func main() {
	input, err := os.ReadFile("../input/day2.txt")
	if err != nil {
		fmt.Println("Reading file went wrong: ", err)
	}
	fmt.Println("Result of part 1: ", part1(string(input)))
	fmt.Println("Result of part 2: ", part2(string(input)))
}