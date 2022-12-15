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
		automaton.Day = 3;
		// provides test data (optional)
		var dataSample = @"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw";
		automaton.RegisterTestDataAndResult(dataSample, 157, 1);
		automaton.RegisterTestDataAndResult(dataSample, 70, 2);
	}

	// compute the answer to the first part
	public object GetAnswer1(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		
		var score = 0;
		
		foreach (var line in lines)
		{
			var shared = line.Take(line.Length / 2).Intersect(line.Skip(line.Length / 2)).Single();
			if ('a' <= shared && shared <= 'z') {
				score += shared - 'a' + 1;
			}
			else
			{
				score += shared - 'A' + 27;
			}
		}
	    return score;
	}

	// compute the answer to the second part
	public object GetAnswer2(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		
		var score = 0;
		
		while (lines.Any())
		{
			var sublines = lines.Take(3).ToArray();			
			
			var first = sublines[0].ToCharArray();
			var second = sublines[1].ToCharArray();
			var third = sublines[2].ToCharArray();
			
			var shared = first.Intersect(second).Intersect(third).Single();
			if ('a' <= shared && shared <= 'z') {
				score += shared - 'a' + 1;
			}
			else
			{
				score += shared - 'A' + 27;
			}
			
			lines = lines.Skip(3).ToArray();
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