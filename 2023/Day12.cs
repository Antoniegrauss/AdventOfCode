using System.Collections;
using System.Collections.Generic;

namespace AoC23.Day12;

public class SpringConditionRecords
{
    private List<RecordRow> records = new();


    public void Unfold()
    {
        foreach (RecordRow record in records)
        {
            record.Unfold();
        }
    }

    private class RecordRow
    {
        List<int> row = new();
        List<int> contiguousGroups = new();

        sealed class ListComparer : EqualityComparer<(List<int>, List<int>)>
        {
            public override bool Equals((List<int>, List<int>) a, (List<int>, List<int>) b)
            {
                return
                StructuralComparisons.StructuralEqualityComparer.Equals(a.Item1?.ToArray(), b.Item1?.ToArray()) &&
                StructuralComparisons.StructuralEqualityComparer.Equals(a.Item2?.ToArray(), b.Item2?.ToArray());
            }

            public override int GetHashCode((List<int>, List<int>) a)
            {
                return
                HashCode.Combine(StructuralComparisons.StructuralEqualityComparer.GetHashCode(a.Item1?.ToArray()),
                StructuralComparisons.StructuralEqualityComparer.GetHashCode(a.Item2?.ToArray()));
            }
        }

        public static Dictionary<(List<int>, List<int>), long> possiblePlacesCache =
            new Dictionary<(List<int>, List<int>), long>(new ListComparer());

        public void Unfold()
        {
            List<int> newRow = new();
            List<int> tempContiguous = new();
            // Duplicate the row 4 times, separated by a ?
            for (int i = 0; i < 5; i++)
            {
                tempContiguous.AddRange(contiguousGroups);
                newRow.AddRange(row);
                if (i != 4)
                {
                    newRow.Add(-1);
                }
            }
            row = newRow;
            contiguousGroups = tempContiguous;
        }

        public static RecordRow ParseRow(string line)
        {
            RecordRow rr = new RecordRow();

            string[] parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            string springs = parts[0];
            string contiguousGroups = parts[1];

            foreach (char c in springs)
            {
                rr.row.Add(ParseChar(c));
            }

            foreach (string c in contiguousGroups.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                rr.contiguousGroups.Add(int.Parse(c));
            }

            return rr;
        }

        private static int ParseChar(char c)
        {
            return c switch
            {
                '.' => 0,
                '?' => -1,
                '#' => 1,
                _ => throw new ArgumentException("Invalid character in record row"),
            };
        }

        public long CountPossibleArrangements()
        {
            return CountPossibleArrangements(ref row, ref contiguousGroups);
        }

        private static long CountPossibleArrangements(
            ref List<int> row, ref List<int> contiguousGroups)
        {
            if (possiblePlacesCache.ContainsKey((row, contiguousGroups)))
            {
                long answer = possiblePlacesCache[(row, contiguousGroups)];
                return answer;
            }

            if (contiguousGroups.Count == 1)
            {
                int temp = PossiblePlacesForLastGroup(contiguousGroups[0], row).Count;

                possiblePlacesCache[(row, contiguousGroups)] = temp;
                return temp;
            }

            // Check first contiguous group and remove it from the row
            // in all possible combinations
            // Then recurse over all remaining options
            long sum = 0;
            foreach (int place in PossiblePlacesForGroup(contiguousGroups[0], row))
            {
                // Create a new row without the first group
                int gapLocation = contiguousGroups[0] + place;

                // If we placed the group at the end, check whether we have
                // any more groups
                if (gapLocation == row.Count)
                {
                    if (contiguousGroups.Count == 1)
                    {
                        // No more groups, one valid arrangement
                        long temp = sum + 1;
                        possiblePlacesCache[(row, contiguousGroups)] = temp;
                        return temp;
                    }
                    else
                    {
                        // More groups but no space left
                        possiblePlacesCache[(row, contiguousGroups)] = sum;
                        return sum;
                    }
                }

                // Slice off the part where we placed the first group
                // Also slice off 1 for the gap between groups
                List<int> newRow = [.. row.Skip(gapLocation + 1)];
                List<int> newContiguousGroups = [.. contiguousGroups.Skip(1)];

                sum += CountPossibleArrangements(ref newRow, ref newContiguousGroups);
            }
            possiblePlacesCache[(row, contiguousGroups)] = sum;
            return sum;
        }

        private static List<int> PossiblePlacesForGroup(int groupSize, List<int> row)
        {
            List<int> possiblePlaces = new();
            for (int i = 0; i < row.Count; i++)
            {
                if (row.Take(i).Any(s => s == 1))
                {
                    // Not allowed to skip filled spots
                    break;
                }
                if (CanPlaceGroupAt(i, groupSize, row))
                {
                    possiblePlaces.Add(i);
                }
            }

            return possiblePlaces;
        }

        private static List<int> PossiblePlacesForLastGroup(int groupSize, List<int> row)
        {
            List<int> possiblePlaces = new();
            for (int i = 0; i < row.Count; i++)
            {
                if (row.Take(i).Any(s => s == 1))
                {
                    // Not allowed to skip filled spots
                    break;
                }
                if (CanPlaceGroupAt(i, groupSize, row))
                {
                    // If there is a trailing 1 (must place) this is not an option
                    if (row.Skip(i + groupSize).Contains(1)) continue;
                    possiblePlaces.Add(i);
                }
            }

            return possiblePlaces;
        }

        // Spots for group must be -1 or 1, cannot be 0
        // Also spot before and after group must be 0 or out of bounds
        private static bool CanPlaceGroupAt(int index, int groupSize, List<int> row)
        {
            if (index + groupSize > row.Count)
            {
                return false; // Out of bounds
            }

            if (index > 0 && row[index - 1] == 1)
            {
                return false; // Spot before group is occupied
            }

            if (index + groupSize < row.Count && row[index + groupSize] == 1)
            {
                return false; // Spot after group is occupied
            }

            for (int i = 0; i < groupSize; i++)
            {
                if (row[index + i] == 0)
                {
                    return false; // Empty space is impossible
                }
            }

            return true;
        }
    }

    public static SpringConditionRecords Parse(string input)
    {
        SpringConditionRecords scr = new SpringConditionRecords();

        string[] lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            scr.records.Add(RecordRow.ParseRow(line));
        }

        return scr;
    }

    public long SumPossibleArrangements()
    {
        long total = 0;
        foreach (RecordRow row in records)
        {
            total += row.CountPossibleArrangements();
        }
        return total;
    }
}

public class Day12
{
    public static long Part1(string input)
    {
        SpringConditionRecords scr = SpringConditionRecords.Parse(input);
        // Placeholder implementation
        return scr.SumPossibleArrangements();
    }

    public static long Part2(string input)
    {
        SpringConditionRecords scr = SpringConditionRecords.Parse(input);
        scr.Unfold();
        return scr.SumPossibleArrangements();
    }
}

