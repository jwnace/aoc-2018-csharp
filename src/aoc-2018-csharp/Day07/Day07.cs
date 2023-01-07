using System.Text;

namespace aoc_2018_csharp.Day07;

public static class Day07
{
    private static readonly string[] Input = File.ReadAllLines("Day07/day07.txt");

    public static string Part1()
    {
        var steps = GetSteps();
        var queue = new PriorityQueue<Step, char>();
        var result = new StringBuilder();

        if (steps.Any(step => step.IsAvailable))
        {
            var availableSteps = steps.Where(step => step.IsAvailable).ToList();

            foreach (var step in availableSteps)
            {
                queue.Enqueue(step, step.Id);
            }
        }

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            node.IsComplete = true;

            result.Append(node.Id);

            var newAvailableSteps = steps.Where(step => step.Prerequisites.Contains(node) && step.IsAvailable).ToList();

            foreach (var step in newAvailableSteps)
            {
                queue.Enqueue(step, step.Id);
            }
        }

        return result.ToString();
    }

    public static int Part2() => 2;

    private static List<Step> GetSteps()
    {
        var steps = new List<Step>();

        foreach (var line in Input)
        {
            var values = line.Split(' ');
            var prerequisiteId = values[1][0];
            var stepId = values[7][0];

            var prerequisite = steps.FirstOrDefault(s => s.Id == prerequisiteId);

            if (prerequisite is null)
            {
                prerequisite = new Step(prerequisiteId);
                steps.Add(prerequisite);
            }

            var step = steps.FirstOrDefault(s => s.Id == stepId);

            if (step is null)
            {
                step = new Step(stepId);
                steps.Add(step);
            }

            step.Prerequisites.Add(prerequisite);
        }

        return steps;
    }

    private class Step
    {
        public char Id { get; }
        public bool IsComplete { get; set; }
        public bool IsAvailable => IsComplete == false && Prerequisites.All(p => p.IsComplete);
        public List<Step> Prerequisites { get; } = new();

        public Step(char id)
        {
            Id = id;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"Id: {Id}");

            if (Prerequisites.Any())
            {
                builder.Append($", Prerequisites: {string.Join(',', Prerequisites.Select(p => p.Id))}");
            }

            return builder.ToString();
        }
    }
}
