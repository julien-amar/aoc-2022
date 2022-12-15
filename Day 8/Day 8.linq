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
		automaton.Day = 8;
		// provides test data (optional)
		var dataSample = @"30373
25512
65332
33549
35390";
		automaton.RegisterTestDataAndResult(dataSample, 21, 1);
		automaton.RegisterTestDataAndResult(dataSample, 8, 2);
	}

	bool CheckVisibileTree(string[] lines, int x, int y, int offsetX, int offsetY)
	{
		var currentHeight = lines[y][x];
		while (true)
		{
			x += offsetX;
			y += offsetY;

			if (x < 0 || x >= lines[0].Length) return true;
			if (y < 0 || y >= lines.Length) return true;

			if (lines[y][x] >= currentHeight) return false;
		}
	}
	
	// compute the answer to the first part
	public object GetAnswer1(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		
		var score = lines.Length * 2 + (lines[0].Length - 2) * 2;
		for (int y = 1; y < lines.Length - 1; ++y)
			for (int x = 1; x < lines[0].Length - 1; ++x)
			{
				if (CheckVisibileTree(lines, x, y, 1, 0) ||
					CheckVisibileTree(lines, x, y, -1, 0) ||
					CheckVisibileTree(lines, x, y, 0, 1) ||
					CheckVisibileTree(lines, x, y, 0, -1)) {
						score++;
				}
			} 
		return score;
	}

	int CheckScenicScore(string[] lines, int x, int y, int offsetX, int offsetY)
	{
		var currentHeight = lines[y][x];
		var scenicScore = 0;
		while (true)
		{
			x += offsetX;
			y += offsetY;

			if (x < 0 || x >= lines[0].Length) return scenicScore;
			if (y < 0 || y >= lines.Length) return scenicScore;

			scenicScore++;
			if (lines[y][x] >= currentHeight) return scenicScore;
		}
	}
	
	// compute the answer to the second part
	public object GetAnswer2(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

		var score = 0;
		for (int y = 1; y < lines.Length - 1; ++y)
			for (int x = 1; x < lines[0].Length - 1; ++x)
			{
				var currentScenicScore =
					CheckScenicScore(lines, x, y, 1, 0) *
					CheckScenicScore(lines, x, y, -1, 0) *
					CheckScenicScore(lines, x, y, 0, 1) *
					CheckScenicScore(lines, x, y, 0, -1);

				if (score < currentScenicScore) score = currentScenicScore;
			}
		return score;
	}
}

void Main()
{
	var automaton = new Automaton();
	automaton.RunDay<TheSolver>();
}
