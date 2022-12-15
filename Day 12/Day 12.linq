<Query Kind="Program">
  <NuGetReference>AoC</NuGetReference>
  <Namespace>AoC</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

internal class TheSolver : ISolver
{
	// provides the puzzle data
	public void SetupRun(Automaton automaton)
	{
		// set the day number (mandatory)
		automaton.Day = 12;

		// provides test data (optional)

		var dataSample = @"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi";
		automaton.RegisterTestDataAndResult(dataSample, 31, 1);
		automaton.RegisterTestDataAndResult(dataSample, 29, 2);
	}
	
	// compute the answer to the first part
	public object GetAnswer1(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
		
		Point start = Point.Empty;
		Point end = Point.Empty;
		for (var row = 0; row < lines.Length; ++row)
			for (var col = 0; col < lines[row].Length; ++col)
			{
				if (lines[row][col] == 'S')	start = new Point(col, row);
				if (lines[row][col] == 'E')	end = new Point(col, row);
			}

		return FindShortestPathDistance(start, end, lines);
	}

	// compute the answer to the second part
	public object GetAnswer2(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
		
		var starts = new List<Point>();
		Point end = Point.Empty;
		for (var row = 0; row < lines.Length; ++row)
			for (var col = 0; col < lines[row].Length; ++col)
			{
				if (lines[row][col] == 'S' || lines[row][col] == 'a') starts.Add(new Point(col, row));
				if (lines[row][col] == 'E') end = new Point(col, row);
			}

		return starts
			.Select(start => FindShortestPathDistance(start, end, lines))
			.Where(distance => distance.HasValue)
			.OrderBy(x => x.Value)
			.First()
			.Value;
	}

	private int GetMapLevel(string[] map, Point position)
	{
		var height = map[position.Y][position.X];
		return (height) switch
		{
			'S' => 0,
			'E' => 25,
			(char a) => a - 'a'
		};
	}

	private int? FindShortestPathDistance(Point current, Point end, string[] lines)
	{
		var visited = new Dictionary<Point, int>();
		var toVisist = new Queue<(Point position, int distance)>();

		var directions = new (int xOffset, int yOffset)[] {
			(-1, 0),
			(1, 0),
			(0, -1),
			(0, 1),
		};

		toVisist.Enqueue((current, 0));
		while (toVisist.Any())
		{
			(current, var distance) = toVisist.Dequeue();

			if (visited.ContainsKey(current)) continue;

			visited.Add(current, distance);

			foreach (var direction in directions)
			{
				if (current.X + direction.xOffset < 0) continue;
				if (current.Y + direction.yOffset < 0) continue;
				if (current.X + direction.xOffset >= lines[0].Length) continue;
				if (current.Y + direction.yOffset >= lines.Length) continue;

				var nextPoint = new Point(current.X + direction.xOffset, current.Y + direction.yOffset);

				if (GetMapLevel(lines, nextPoint) <= GetMapLevel(lines, current) + 1)
				{
					if (nextPoint == end)
					{
						return distance + 1;
					}

					toVisist.Enqueue((nextPoint, distance + 1));
				}
			}

		}

		return null;
	}
}

void Main()
{
	var automaton = new Automaton();
	automaton.RunDay<TheSolver>();
}
