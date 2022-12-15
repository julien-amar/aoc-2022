<Query Kind="Program">
  <NuGetReference>AoC</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>AoC</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
</Query>

internal class TheSolver : ISolver
{
	// provides the puzzle data
	public void SetupRun(Automaton automaton)
	{
		// set the day number (mandatory)
		automaton.Day = 14;

		// provides test data (optional)
		var dataSample = @"498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9";

		automaton.RegisterTestDataAndResult(dataSample, 24, 1);
		automaton.RegisterTestDataAndResult(dataSample, 93, 2);
	}

	HashSet<Point> ParseInput(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToArray();

		var rocks = new HashSet<Point>();
		foreach (var line in lines)
		{
			var edges = line.Split(" -> ");
			for (int i = 0; i < edges.Length - 1; ++i)
			{
				var fromInfo = edges[i].Split(",").Select(x => Convert.ToInt32(x)).ToArray();
				var toInfo = edges[i + 1].Split(",").Select(x => Convert.ToInt32(x)).ToArray();

				var from = new Point(fromInfo[0], fromInfo[1]);
				var to = new Point(toInfo[0], toInfo[1]);

				var orientation = new Point(
					(from.X - to.X) switch { < 0 => 1, > 0 => -1, _ => 0 },
					(from.Y - to.Y) switch { < 0 => 1, > 0 => -1, _ => 0 });

				rocks.Add(from);
				while (from != to)
				{
					from.Offset(orientation);
					rocks.Add(from);
				}
			}
		}
		return rocks;
	}

	// compute the answer to the first part
	public object GetAnswer1(string data)
	{
		var start = new Point(500, 0);
		var rocks = ParseInput(data);

		var capHeight = rocks.Max(x => x.Y) + 1;
		
		return Simulate(start, rocks, sand => sand.Y == capHeight);
	}

	// compute the answer to the second part
	public object GetAnswer2(string data)
	{
		var start = new Point(500, 0);
		var rocks = ParseInput(data);

		// Add floor
		var capHeight = rocks.Max(x => x.Y) + 2;
		var capFloorFrom = start.X - capHeight - 1;
		var capFloorTo = start.X + capHeight + 1;

		for (var i = capFloorFrom; i <= capFloorTo; ++i)
			rocks.Add(new Point(i, capHeight));

		return Simulate(start, rocks, sand => false);
	}
	
	int Simulate(Point start, HashSet<Point> rocks, Func<Point, bool> simulationPredicate)
	{
		var score = 0;
		while (true)
		{
			var sand = start;

			while (true)
			{
				var bottom = new Point(sand.X, sand.Y + 1);
				var bottomLeft = new Point(sand.X - 1, sand.Y + 1);
				var bottomRight = new Point(sand.X + 1, sand.Y + 1);

				if (!rocks.Contains(bottom))
				{
					sand = bottom;
				}
				else if (!rocks.Contains(bottomLeft))
				{
					sand = bottomLeft;
				}
				else if (!rocks.Contains(bottomRight))
				{
					sand = bottomRight;
				}
				else if (!rocks.Contains(sand))
				{
					rocks.Add(sand);
					score++;
					break;
				}
				else
				{
					return score;
				}
				
				if (simulationPredicate(sand))
				{
					return score;
				}
			}
		}
	}
}

void Main()
{
	var automaton = new Automaton();
	automaton.RunDay<TheSolver>();
}
