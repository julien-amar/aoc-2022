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
		automaton.Day = 1;
		// provides test data (optional)
		var dataSample = @"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000";
		automaton.RegisterTestDataAndResult(dataSample, 24000, 1);
		automaton.RegisterTestDataAndResult(dataSample, 45000, 2);
	}

	// compute the answer to the first part
	public object GetAnswer1(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.TrimEntries);
		
		var result = new Stack<int>();
		result.Push(0);
		
		foreach (var line in lines)
		{
			if (line == String.Empty) { 
				result.Push(0);
			} else {
				result.Push(result.Pop() + Convert.ToInt32(line));
			}
		}
	    return result.Max();
	}

	// compute the answer to the second part
	public object GetAnswer2(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.TrimEntries);

		var result = new Stack<int>();
		result.Push(0);

		foreach (var line in lines)
		{
			if (line == String.Empty)
			{
				result.Push(0);
			}
			else
			{
				result.Push(result.Pop() + Convert.ToInt32(line));
			}
		}
		return result
			.OrderByDescending(x => x)
			.Take(3)
			.Sum();
	}
}

void Main()
{
	var automaton = new Automaton();
	automaton.RunDay<TheSolver>();
}
