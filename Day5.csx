#!/usr/bin/env dotnet-script
#nullable enable

using System.Text.RegularExpressions;

public record Point(int X, int Y);

public record Line(Point Start, Point End)
{
    public bool IsHorizontal => Start.Y == End.Y;

    public bool IsVertical => Start.X == End.X;

    public List<Point> GetAllPoints() 
    {
        var points = new List<Point>() { Start };
        var currentPoint = Start;

        for (var i = 1; currentPoint != End; i++)
        {
            var xIncreasing = (End.X - Start.X) >= 0;
            var yIncreasing = (End.Y - Start.Y) >= 0;

            currentPoint = IsHorizontal
                ? new (xIncreasing ? Start.X + i : Start.X - i, Start.Y)
                : IsVertical
                    ? new (Start.X, yIncreasing ? Start.Y + i : Start.Y - i)
                    : new (xIncreasing ? Start.X + i : Start.X - i,
                           yIncreasing ? Start.Y + i : Start.Y - i);
            points.Add(currentPoint);
        }
        return points;
    }

    public static Line Parse(string line)
    {
        var regex = new Regex(@"(\d+),(\d+) (?:->) (\d+),(\d+)");
        var match = regex.Match(line);
        return new (
            new (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)),
            new (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)));
    }
}

var data = await File.ReadAllLinesAsync("Data/Day5.txt");
var lines = data.Select(Line.Parse);
var overlappingPoints = lines
                          //.Where(l => l.IsHorizontal || l.IsVertical) // Part 1
                          .SelectMany(l => l.GetAllPoints())
                          .GroupBy(p => p)
                          .Count(g => g.Count() > 1);

Console.WriteLine("Day 5: Hydrothermal Venture");
Console.Write($"{overlappingPoints}");
