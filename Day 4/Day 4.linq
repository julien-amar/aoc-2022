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
		automaton.Day = 4;
		// provides test data (optional)
		var dataSample = @"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8";
		automaton.RegisterTestDataAndResult(dataSample, 2, 1);
		automaton.RegisterTestDataAndResult(dataSample, 4, 2);
	}

	// compute the answer to the first part
	public object GetAnswer1(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

		var score = 0;
		
		foreach (var line in lines)
		{
			var subLine= line.Split(',');
			
			var first = subLine[0].Split("-").Select(x => Convert.ToInt32(x)).ToArray();
			var second = subLine[1].Split("-").Select(x => Convert.ToInt32(x)).ToArray();

			if (first[0] <= second[0] && second[1] <= first[1]) {
				score++;
			}
			else if (second[0] <= first[0] && first[1] <= second[1])
			{
				score++;
			}
		}
	    return score;
	}

	// compute the answer to the second part
	public object GetAnswer2(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

		var score = 0;
		
		foreach (var line in lines)
		{
			var subLine= line.Split(',');
			
			var first = subLine[0].Split("-").Select(x => Convert.ToInt32(x)).ToArray();
			var second = subLine[1].Split("-").Select(x => Convert.ToInt32(x)).ToArray();

			if (first[0] <= second[0] && second[0] <= first[1]) {
				score++;
			}
			else if (first[0] <= second[1] && second[1] <= first[1])
			{
				score++;
			}
			else if (second[0] <= first[0] && first[0] <= second[1])
			{
				score++;
			}
			else if (second[0] <= first[1] && first[1] <= second[1])
			{
				score++;
			}
		}
		return score;
	}
}

void Main()
{
	var automaton = new Automaton();
	automaton.RunDay<TheSolver>();
}

// You can define other methods, fields, classes and namespaces here