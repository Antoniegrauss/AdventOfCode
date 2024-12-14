namespace AoC23.Day8;

public record NodePart1
{
    public int id;
    public int left;
    public int right;
}

public record NodePart2
{
    public int id;
    public int left;
    public int right;
    public bool isStart;
    public bool isEnd;
}

public record ParsedNode
{
    public string name;
    public string left;
    public string right;
}

public class Day8
{
    public static Dictionary<(int, char), int> _cache = new();
    public static List<NodePart2> _nodesPart2 = new();

    public static (List<NodePart1>, int) parseNodes(string input)
    {
        List<ParsedNode> parsedNodes = parseNodeStrings(input);
        Dictionary<string, int> nameToId = createNodeDictionary(parsedNodes);

        int goal = nameToId["ZZZ"];

        return (parsedNodes.Select(pn => new NodePart1
        {
            id = nameToId[pn.name],
            left = nameToId[pn.left],
            right = nameToId[pn.right]
        }).ToList(), goal);
    }

    private static Dictionary<string, int> createNodeDictionary(List<ParsedNode> parsedNodes)
    {
        Dictionary<string, int> nameToId = new();
        for (int i = 0; i < parsedNodes.Count; i++)
        {
            nameToId[parsedNodes[i].name] = i;
        }

        return nameToId;
    }

    private static List<ParsedNode> parseNodeStrings(string input)
    {
        return input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(line => line.Split(" = "))
            .Select(parts => new ParsedNode
            {
                name = parts[0],
                left = parts[1].Split(", ")[0].Trim('(', ')'),
                right = parts[1].Split(", ")[1].Trim('(', ')')
            })
            .ToList();
    }

    public static List<NodePart2> parsePart2(string input)
    {
        List<ParsedNode> parsedNodes = parseNodeStrings(input);
        Dictionary<string, int> nameToId = createNodeDictionary(parsedNodes);

        return parsedNodes.Select(pn => new NodePart2
        {
            id = nameToId[pn.name],
            left = nameToId[pn.left],
            right = nameToId[pn.right],
            isStart = pn.name.EndsWith("A"),
            isEnd = pn.name.EndsWith("Z")
        }).ToList();
    }

    public static int move(int currentID, char direction, List<NodePart1> nodes)
    {
        NodePart1 currentNode = nodes[currentID];
        return direction == 'L' ? currentNode.left : currentNode.right;
    }

    public static int movePart2(int currentID, char direction)
    {
        if (_cache.ContainsKey((currentID, direction)))
        {
            return _cache[(currentID, direction)];
        }
        int newNode = direction == 'L' ? _nodesPart2[currentID].left : _nodesPart2[currentID].right;
        _cache[(currentID, direction)] = newNode;
        return newNode;
    }

    public static int Part1(string input)
    {
        string[] instructionsAndNodes = input.Split(Environment.NewLine + Environment.NewLine);
        string instructions = instructionsAndNodes[0];

        (List<NodePart1> nodes, int goal) = parseNodes(instructionsAndNodes[1]);

        // Start at beginning
        int currentID = 0;
        int totalSteps = 0;
        while (currentID != goal)
        {
            foreach (char direction in instructions)
            {
                totalSteps++;
                currentID = move(currentID, direction, nodes);
                if (currentID == goal)
                {
                    break;
                }
            }
        }
        return totalSteps;
    }

    public static long Part2(string input)
    {
        string[] instructionsAndNodes = input.Split(Environment.NewLine + Environment.NewLine);
        string instructions = instructionsAndNodes[0];
        _nodesPart2 = parsePart2(instructionsAndNodes[1]);

        List<int> currentNodes = _nodesPart2.Where(n => n.isStart).Select(n => n.id).ToList();

        // long totalSteps = Part2NaiveSolution(instructions, ref currentNodes);

        List<CycleInfo> cycleLengths = new(currentNodes.Count);
        foreach (int nodeId in currentNodes)
        {
            // Calculate the cycle length for each node path
            cycleLengths.Add(CalculateCycleLength(nodeId, instructions));
        }

        // It seems the cycles are all from 0 to n
        // To get the least common multiple use:
        // lcm = abs(a * b) / gcd(a, b)
        long lcm = 1;
        foreach (var cycleInfo in cycleLengths)
        {
            lcm = LCM(lcm, cycleInfo._cycleLength);
        }

        return lcm;
    }

    static long LCM(long a, long b)
    {
        return Math.Abs(a * b) / GCD(a, b);
    }

    // Use eudlicean algorithm to calculate gcd
    static long GCD(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    public struct VisitedGoal : IEquatable<VisitedGoal>
    {
        public int nodeId;
        public int steps;

        public bool Equals(VisitedGoal other)
        {
            return nodeId == other.nodeId;
        }
    }

    public struct CycleInfo(int cycle_length, int steps_to_cycle)
    {
        public int _cycleLength = cycle_length;
        public int _stepsToCycle = steps_to_cycle;
    }

    // Returns a tuple of (cycle length, steps to reach cycle)
    private static CycleInfo CalculateCycleLength(int startNodeId, string instructions)
    {
        // If we visit a goal twice we have a cycle
        List<VisitedGoal> goalsVisited = new();

        int currentNodeId = startNodeId;
        int steps = 0;
        while (true)
        {
            foreach (char direction in instructions)
            {
                currentNodeId = movePart2(currentNodeId, direction);
                steps++;
            }
            if (_nodesPart2[currentNodeId].isEnd)
            {
                var newGoal = new VisitedGoal { nodeId = currentNodeId, steps = steps };
                if (goalsVisited.Contains(newGoal))
                {
                    int firstVisitSteps = goalsVisited.First(g => g.nodeId == currentNodeId).steps;
                    return new CycleInfo(steps - firstVisitSteps, firstVisitSteps);
                }
                goalsVisited.Add(newGoal);
            }
        }
    }

    private static long Part2NaiveSolution(string instructions, ref List<int> currentNodes)
    {
        long totalSteps = 0;
        while (currentNodes.Any(id => !_nodesPart2[id].isEnd))
        {
            foreach (char direction in instructions)
            {
                currentNodes = [.. currentNodes.Select(id => movePart2(id, direction))];
                totalSteps++;
            }
        }

        return totalSteps;
    }
}