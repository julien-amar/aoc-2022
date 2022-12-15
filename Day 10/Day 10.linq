<Query Kind="Program">
  <NuGetReference>AoC</NuGetReference>
  <Namespace>AoC</Namespace>
</Query>

internal class TheSolver : ISolver
{
	// provides the puzzle data
	public void SetupRun(Automaton automaton)
	{
		// set the day number (mandatory)
		automaton.Day = 10;
		// provides test data (optional)
		var largeSample = @"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop";
		automaton.RegisterTestDataAndResult(largeSample, 13140, 1);
		automaton.RegisterTestDataAndResult(largeSample, "FPGPHFGH", 2);
	}

	// compute the answer to the first part
	public object GetAnswer1(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		var execution = new List<int>();
		int x = 1;
		execution.Add(x);
		foreach (var line in lines) {
			var lineInfo = line.Split(' ');

			switch (lineInfo[0])
			{
				case "noop":
					execution.Add(x);
					break;
				case "addx":
					execution.Add(x);
					execution.Add(x);
					x += Convert.ToInt32(lineInfo[1]);
					break;
			};
		}
		
		var signalStrenght = 0;
		for (var cycle = 20; cycle <= 220; cycle += 40) {
			// execution[cycle].Dump(cycle.ToString());
			signalStrenght += cycle * execution[cycle];
		}

		return signalStrenght;
	}

	// compute the answer to the second part
	public object GetAnswer2(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		var execution = new List<int>();
		int x = 1;
		execution.Add(x);
		foreach (var line in lines)
		{
			var lineInfo = line.Split(' ');

			switch (lineInfo[0])
			{
				case "noop":
					execution.Add(x);
					break;
				case "addx":
					execution.Add(x);
					execution.Add(x);
					x += Convert.ToInt32(lineInfo[1]);
					break;
			};
		}

		for (var row = 0; row < 6; ++row)
		{
			for (var column = 0; column < 40; ++column)
			{
				var cycle = ((row * 40) + column) + 1;
				var pizelIndex = column + 1;
				
				x = execution[cycle];
				
				var drawn = x <= pizelIndex && pizelIndex <= x + 2 ? "#" : ".";
				Console.Write(drawn);
				/*
				new
				{
					Cycle = cycle,
					Pixel = cycle,
					SpritePosition = new[] { x, x + 1, x + 2 },
					Drawn = x <= pizelIndex && pizelIndex <= x + 2 ? "#" : "."
				}.Dump();
				*/
			}
			Console.WriteLine();
		}
		return "FPGPHFGH";
	}
}

void Main()
{
	var automaton = new Automaton();
	automaton.RunDay<TheSolver>();
}
