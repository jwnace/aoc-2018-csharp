namespace aoc_2018_csharp.Day08;

public static class Day08
{
    private static readonly int[] Input = File.ReadAllText("Day08/day08.txt").Split(' ').Select(int.Parse).ToArray();

    public static int Part1() => BuildTree().GetMetadataRecursive().Sum();

    public static int Part2() => BuildTree().GetValueRecursive();

    private static Node BuildTree()
    {
        var index = 0;
        return BuildTree(ref index);
    }

    private static Node BuildTree(ref int index)
    {
        var node = new Node();
        var childNodeCount = Input[index];
        var metadataCount = Input[++index];

        while (node.Children.Count < childNodeCount)
        {
            index++;
            node.Children.Add(BuildTree(ref index));
        }

        while (node.Metadata.Count < metadataCount)
        {
            node.Metadata.Add(Input[++index]);
        }

        return node;
    }

    private class Node
    {
        public List<Node> Children { get; } = new();
        public List<int> Metadata { get; } = new();

        public List<int> GetMetadataRecursive()
        {
            var result = new List<int>();

            result.AddRange(Metadata);

            foreach (var child in Children)
            {
                result.AddRange(child.GetMetadataRecursive());
            }

            return result;
        }

        public int GetValueRecursive()
        {
            if (Children.Count == 0)
            {
                return Metadata.Sum();
            }

            var result = 0;

            foreach (var metadata in Metadata)
            {
                var index = metadata - 1;
                var child = Children.ElementAtOrDefault(index);

                result += child?.GetValueRecursive() ?? 0;
            }

            return result;
        }
    }
}
