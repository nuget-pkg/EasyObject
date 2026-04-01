//css_nuget Spectre.Console;
//css_nuget EasyObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Progress;

public static class Program
{
    public static void Main(string[] args)
    {
        Global.EasyObject.Log(new { args });
        AnsiConsole.MarkupLine("[yellow]Initializing warp drive[/]...");

        // Show progress
        AnsiConsole.Progress()
            .AutoClear(false)
            .Columns(new ProgressColumn[]
            {
                    new TaskDescriptionColumn(),    // Task description
                    new ProgressBarColumn(),        // Progress bar
                    new PercentageColumn(),         // Percentage
                    new RemainingTimeColumn(),      // Remaining time
                    new SpinnerColumn(),            // Spinner
            })
            .UseRenderHook((renderable, tasks) => RenderHook(tasks, renderable))
            .Start(ctx =>
            {
                var random = new Random(DateTime.Now.Millisecond);

                // Create some tasks
                var tasks = CreateTasks(ctx, random);
                var warpTask = ctx.AddTask("Going to warp", autoStart: false).IsIndeterminate();

                // Wait for all tasks (except the indeterminate one) to complete
                while (!ctx.IsFinished)
                {
                    // Increment progress
                    foreach (var (task, increment) in tasks)
                    {
                        task.Increment(random.NextDouble() * increment);
                    }

                    // Write some random things to the terminal
                    if (random.NextDouble() < 0.1)
                    {
                        WriteLogMessage();
                    }

                    // Simulate some delay
                    Thread.Sleep(100);
                }

                // Now start the "warp" task
                warpTask.StartTask();
                warpTask.IsIndeterminate(false);
                while (!ctx.IsFinished)
                {
                    warpTask.Increment(12 * random.NextDouble());

                    // Simulate some delay
                    Thread.Sleep(100);
                }
            });

        // Done
        AnsiConsole.MarkupLine("[green]Done![/]");
    }

    private static IRenderable RenderHook(IReadOnlyList<ProgressTask> tasks, IRenderable renderable)
    {
        var header = new Panel("Going on a :rocket:, we're going to the :crescent_moon:").Expand().RoundedBorder();
        var footer = new Rows(
            new Rule(),
            new Markup(
                $"[blue]{tasks.Count}[/] total tasks. [green]{tasks.Count(i => i.IsFinished)}[/] complete.")
        );

        const string ESC = "\u001b";
        string escapeSequence;
        if (tasks.All(i => i.IsFinished))
        {
            escapeSequence = $"{ESC}]]9;4;0;100{ESC}\\";
        }
        else
        {
            var total = tasks.Sum(i => i.MaxValue);
            var done = tasks.Sum(i => i.Value);
            var percent = (int)(done / total * 100);
            escapeSequence = $"{ESC}]]9;4;1;{percent}{ESC}\\";
        }

        var middleContent = new Grid().AddColumns(new GridColumn(), new GridColumn().Width(20));
        middleContent.AddRow(renderable, new FigletText(tasks.Count(i => i.IsFinished == false).ToString()));

        return new Rows(header, middleContent, footer, new ControlCode(escapeSequence));
    }

    private static List<(ProgressTask Task, int Delay)> CreateTasks(ProgressContext progress, Random random)
    {
        var tasks = new List<(ProgressTask, int)>();
        while (tasks.Count < 5)
        {
            if (DescriptionGenerator.TryGenerate(out var name))
            {
                tasks.Add((progress.AddTask(name), random.Next(2, 10)));
            }
        }

        return tasks;
    }

    private static void WriteLogMessage()
    {
        AnsiConsole.MarkupLine(
            "[grey]LOG:[/] " +
            DescriptionGenerator.Generate() +
            "[grey]...[/]");
    }
}

public static class DescriptionGenerator
{
    private static readonly string[] _verbs = new[] { "Downloading", "Rerouting", "Retriculating", "Collapsing", "Folding", "Solving", "Colliding", "Measuring" };
    private static readonly string[] _nouns = new[] { "internet", "splines", "space", "capacitators", "quarks", "algorithms", "data structures", "spacetime" };

    private static readonly Random _random;
    private static readonly HashSet<string> _used;

    static DescriptionGenerator()
    {
        _random = new Random(DateTime.Now.Millisecond);
        _used = new HashSet<string>();
    }

    public static bool TryGenerate(out string name)
    {
        var iterations = 0;
        while (iterations < 25)
        {
            name = Generate();
            if (!_used.Contains(name))
            {
                _used.Add(name);
                return true;
            }

            iterations++;
        }

        name = Generate();
        return false;
    }

    public static string Generate()
    {
        return _verbs[_random.Next(0, _verbs.Length)]
            + " " + _nouns[_random.Next(0, _nouns.Length)];
    }
}
