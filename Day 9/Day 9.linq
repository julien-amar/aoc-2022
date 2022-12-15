<Query Kind="Program">
  <NuGetReference>AoC</NuGetReference>
  <Namespace>AoC</Namespace>
</Query>

internal class Point
{
	public int X { get; set; }
	public int Y { get; set; }
	public Point(int x, int y)
	{
		X = x;
		Y = y;
	}
}

internal class TheSolver : ISolver
{
	// provides the puzzle data
	public void SetupRun(Automaton automaton)
	{
		// set the day number (mandatory)
		automaton.Day = 9;
		// provides test data (optional)
		var dataSample = @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2";
		var largeSample = @"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20";
		automaton.RegisterTestDataAndResult(dataSample, 13, 1);
		automaton.RegisterTestDataAndResult(largeSample, 36, 2);
	}

	// compute the answer to the first part
	public object GetAnswer1(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		var H = new Point(0, 0);
		var T = new Point(0, 0);
		var positions = new HashSet<(int x, int y)>();
		foreach (var line in lines) {
			var lineInfo = line.Split(' ');
			var direction = lineInfo[0];
			var iteration = Convert.ToInt32(lineInfo[1]);
			for (int i = 0; i < iteration; ++i) {
				switch (direction) {
					case "U":
						H.Y++;
						break;
					case "D":
						H.Y--;
						break;
					case "L":
						H.X--;
						break;
					case "R":
						H.X++;
						break;
				}

				var target = (H.X - T.X, H.Y - T.Y) switch
				{
					( > 1, > 1) => new Point(H.X - 1, H.Y - 1),
					( > 1, < -1) => new Point(H.X - 1, H.Y + 1),
					( < -1, > 1) => new Point(H.X + 1, H.Y - 1),
					( < -1, < -1) => new Point(H.X + 1, H.Y + 1),
					( > 1, _) => new Point(H.X - 1, H.Y),
					( < -1, _) => new Point(H.X + 1, H.Y),
					(_, > 1) => new Point(H.X, H.Y - 1),
					(_, < -1) => new Point(H.X, H.Y + 1),
					_ => new Point(T.X, T.Y),
				};
				
				T = target;
				positions.Add((T.X, T.Y));
			}
		}
		return positions.Count;
	}

	// compute the answer to the second part
	public object GetAnswer2(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		var points = new Point[10];
		for (int i = 0; i < points.Length; ++i)
			points[i] = new Point(0,0);
		var positions = new HashSet<(int x, int y)>();
		foreach (var line in lines)
		{
			var lineInfo = line.Split(' ');
			var direction = lineInfo[0];
			var iteration = Convert.ToInt32(lineInfo[1]);
			for (int i = 0; i < iteration; ++i)
			{
				var Head = points[0];

				switch (direction)
				{
					case "U":
						Head.Y--;
						break;
					case "D":
						Head.Y++;
						break;
					case "L":
						Head.X--;
						break;
					case "R":
						Head.X++;
						break;
				}
				
				for (int p = 0; p < points.Length - 1; ++p) {
					var H = points[p];
					var T = points[p + 1];

					var target = (H.X - T.X, H.Y - T.Y) switch
					{
						( > 1, > 1) => new Point(H.X - 1, H.Y - 1),
						( > 1, < -1) => new Point(H.X - 1, H.Y + 1),
						( < -1, > 1) => new Point(H.X + 1, H.Y - 1),
						( < -1, < -1) => new Point(H.X + 1, H.Y + 1),
						( > 1, _) => new Point(H.X - 1, H.Y),
						( < -1, _) => new Point(H.X + 1, H.Y),
						(_, > 1) => new Point(H.X, H.Y - 1),
						(_, < -1) => new Point(H.X, H.Y + 1),
						_ => new Point(T.X, T.Y),
					};

					points[p + 1] = target;
				}

				positions.Add((points.Last().X, points.Last().Y));
			}
		}
		return positions.Count;
	}
}

void Main()
{
	var automaton = new Automaton();
	automaton.RunDay<TheSolver>();
}
