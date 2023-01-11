namespace aoc_2018_csharp.Day09;

public static class Day09
{
    private static readonly string[] Input = File.ReadAllLines("Day09/day09.txt");

    public static int Part1() => GetMaxScore(403, 71_920);

    public static int Part2() => 2;
    
    public static int GetMaxScore(int playerCount, int lastMarbleValue)
    {
        var scores = new int[playerCount];
        Array.Fill(scores, 0);

        var nodes = new List<Node>();

        var root = new Node(0);
        root.Next = root;
        root.Previous = root;

        nodes.Add(root);

        var currentNode = root;

        for (var i = 1; i <= lastMarbleValue; i++)
        {
            var player = (i - 1) % playerCount;

            if (i % 23 == 0)
            {
                var nodeToRemove = currentNode.Previous.Previous.Previous.Previous.Previous.Previous.Previous;

                var left = nodeToRemove.Previous;
                var right = nodeToRemove.Next;

                left.Next = right;
                right.Previous = left;

                currentNode = right;

                scores[player] += i + nodeToRemove.Value;
            }
            else
            {
                var left = currentNode.Next;
                var right = currentNode.Next.Next;
                var node = new Node(i);

                left.Next = node;
                node.Next = right;

                right.Previous = node;
                node.Previous = left;

                nodes.Add(node);

                currentNode = node;
            }
        }

        return scores.Max();
    }
    
    private class Node
    {
        public int Value { get; }
        public Node? Next { get; set; }
        public Node? Previous { get; set; }

        public Node(int value)
        {
            Value = value;
        }
    }
}
