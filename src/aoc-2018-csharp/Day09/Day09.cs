namespace aoc_2018_csharp.Day09;

public static class Day09
{
    public static long Part1() => GetMaxScore(403, 71_920);

    public static long Part2() => GetMaxScore(403, 100 * 71_920);

    public static long GetMaxScore(int playerCount, int lastMarbleValue)
    {
        var scores = new long[playerCount];
        Array.Fill(scores, 0);

        var root = new Node(0);
        root.Next = root;
        root.Previous = root;

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
