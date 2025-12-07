#include <iostream>
#include <string>
#include <vector>
#include <stdexcept>
#include <functional>
#include <cctype>
#include <set>
#include <cassert>

#include "read_file.hpp"

long part1(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    std::set<int> beams;

    int first_beam = lines[0].find("S");
    beams.insert(first_beam);

    std::set<int> new_beams;
    int split_counter = 0;
    for (int line_id = 1; line_id < lines.size(); line_id++)
    {
        new_beams.clear();
        std::string current_line = lines[line_id];
        for (int beam : beams)
        {
            if (current_line[beam] == '.')
            {
                new_beams.insert(beam);
                continue;
            }
            if (current_line[beam] == '^')
            {
                new_beams.insert(beam + 1);
                new_beams.insert(beam - 1);
                split_counter += 1;
                continue;
            }
            // Lines should consist only of . or ^
            assert(false);
        }
        beams = new_beams;
    }

    return split_counter;
}

struct Beam
{
    int position;
    int amount;
};

long part2(std::string filename)
{
    auto lines = AoC::utils::read_input_file(filename);
    std::vector<Beam> beams = {};

    int first_beam = lines[0].find("S");
    beams.emplace_back(Beam{first_beam, 1});

    std::vector<Beam> new_beams = {};
    for (int line_id = 1; line_id < lines.size(); line_id++)
    {
        new_beams.clear();
        std::string current_line = lines[line_id];
        for (const Beam &beam : beams)
        {
            if (current_line[beam.position] == '.')
            {
                new_beams.emplace_back(beam);
                continue;
            }
            if (current_line[beam.position] == '^')
            {
                bool found_right = false;
                bool found_left = false;
                for (Beam &new_beam : new_beams)
                {
                    if (found_left && found_right)
                    {
                        break;
                    }
                    if (new_beam.position == beam.position + 1)
                    {
                        found_right = true;
                        new_beam.amount += 1;
                    }
                    if (new_beam.position == beam.position - 1)
                    {
                        found_left = true;
                        new_beam.amount += 1;
                    }
                }
                if (!found_right)
                {
                    new_beams.emplace_back(Beam{beam.position + 1, beam.amount});
                }
                if (!found_left)
                {
                    new_beams.emplace_back(Beam{beam.position - 1, beam.amount});
                }
                continue;
            }
            // Lines should consist only of . or ^
            assert(false);
        }
        beams = new_beams;

        long sum = 0;
        for (const Beam &beam : beams)
        {
            sum += beam.amount;
        }
        std::cout << "Sum of row " << line_id << " : " << sum << std::endl;
    }

    long sum = 0;
    for (const Beam &beam : beams)
    {
        sum += beam.amount;
    }

    return sum;
}

void execute_part(std::function<long(std::string)> part_x, std::string file, int part_number)
{
    long answer_part_1 = part_x(file);
    std::cout << "Answer of part " << part_number << ": " << answer_part_1 << std::endl;
}

int main()
{
    execute_part(part1, "day7.txt", 1);
    execute_part(part2, "day7test.txt", 2);
    return 0;
}
