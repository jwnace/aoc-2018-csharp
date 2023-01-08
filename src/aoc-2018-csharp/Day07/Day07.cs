using System.Text;

namespace aoc_2018_csharp.Day07;

public static class Day07
{
    private static readonly string[] Input = File.ReadAllLines("Day07/day07.txt");

    public static string Part1()
    {
        var result = DoWork(1);
        return string.Join(null, result.Select(x => x.StepId));
    }

    public static int Part2()
    {
        var result = DoWork(5);
        return result.Max(x => x.CompletionTime) + 1;
    }

    private static List<(char StepId, int CompletionTime)> DoWork(int workerCount)
    {
        var result = new List<(char StepId, int CompletionTime)>();
        var steps = GetSteps();
        var workers = GetWorkers(workerCount);

        for (var time = 0; time < int.MaxValue; time++)
        {
            if (steps.All(step => step.IsComplete(time)))
            {
                break;
            }

            var availableWorkers = workers.Where(worker => worker.IsAvailable(time)).ToList();

            foreach (var worker in availableWorkers)
            {
                var nextStep = steps.Where(step => step.IsAvailable(time)).MinBy(step => step.Id);

                if (nextStep is null)
                {
                    break;
                }

                var completionTime = time + 60 + nextStep.Id - 'A';
                nextStep.StartProgress(completionTime);
                worker.StartWork(completionTime);
                result.Add((nextStep.Id, completionTime));
            }
        }

        return result;
    }

    private static List<Worker> GetWorkers(int workerCount)
    {
        var workers = new List<Worker>();

        for (var i = 0; i < workerCount; i++)
        {
            workers.Add(new Worker());
        }

        return workers;
    }

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
        private bool _isInProgress;
        private int _completionTime = int.MaxValue;

        public char Id { get; }
        public List<Step> Prerequisites { get; } = new();

        public Step(char id)
        {
            Id = id;
        }

        public void StartProgress(int completionTime)
        {
            _isInProgress = true;
            _completionTime = completionTime;
        }

        public bool IsComplete(int time) => time > _completionTime;

        public bool IsAvailable(int time) =>
            !_isInProgress && !IsComplete(time) && Prerequisites.All(p => p.IsComplete(time));
    }

    private class Worker
    {
        private int _busyUntilTime = -1;

        public void StartWork(int time)
        {
            _busyUntilTime = time;
        }

        public bool IsAvailable(int time) => time > _busyUntilTime;
    }
}
