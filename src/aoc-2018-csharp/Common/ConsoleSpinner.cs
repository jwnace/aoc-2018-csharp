using System.Diagnostics;

namespace aoc_2018_csharp.Common;

internal class ConsoleSpinner
{
    private static readonly string[] Sequence = { "   ", ".  ", ".. ", "..." };
    private readonly string _message;
    private readonly Stopwatch? _stopwatch;
    private readonly int _left;
    private readonly int _top;
    private readonly int _delay;
    private readonly Timer _timer;
    private int _counter;
    private bool _stopped;

    public ConsoleSpinner(
        string message = "",
        Stopwatch? stopwatch = null,
        int? left = null,
        int? top = null,
        int delay = 200)
    {
        _message = message;
        _stopwatch = stopwatch;
        _left = left ?? Console.CursorLeft;
        _top = top ?? Console.CursorTop;
        _delay = delay;
        _timer = new Timer(_ => Spin());
    }

    public void Start()
    {
        // HACK: this needs to be here, otherwise we end up overwriting previous lines
        Console.WriteLine();
        _timer.Change(dueTime: TimeSpan.Zero, period: TimeSpan.FromMilliseconds(_delay));
    }

    public void Stop()
    {
        _stopped = true;
        _timer.Change(dueTime: -1, period: -1);

        var top = _top == Console.BufferHeight - 1 ? _top - 1 : _top;

        Console.SetCursorPosition(_left, top);
    }

    private void Spin()
    {
        var elapsed = _stopwatch?.Elapsed.ToString() ?? "";
        var sequence = _stopwatch is null ? Sequence[++_counter % Sequence.Length] : "...";

        var top = _top == Console.BufferHeight - 1 ? _top - 1 : _top;

        if (_stopped)
        {
            return;
        }

        Console.SetCursorPosition(_left, top);
        Console.WriteLine($"{_message}{elapsed}{sequence}");
    }
}
