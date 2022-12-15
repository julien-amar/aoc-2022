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
		automaton.Day = 5;
		// provides test data (optional)
		var dataSample = @"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2";
		automaton.RegisterTestDataAndResult(dataSample, "CMZ", 1);
		automaton.RegisterTestDataAndResult(dataSample, "MCD", 2);
	}

	// compute the answer to the first part
	public object GetAnswer1(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

		var crates = new List<List<char>>();
		
		var parseCrates = true;
		foreach (var line in lines)
		{
			if (parseCrates)
			{
				if (line == string.Empty)
				{
					parseCrates = false;
					continue;
				}
				
				var remainingCrate = line;
				var crateIndex = 0;
				while (remainingCrate.Any()) {
					var currentCrate = remainingCrate[1];
					if (currentCrate == '1') break;
					
					if (crateIndex >= crates.Count)
						crates.Add(new List<char>());
					if (currentCrate != ' ')
						crates[crateIndex].Add(currentCrate);
					crateIndex++;
					remainingCrate = remainingCrate.Substring(remainingCrate.Length >= 4 ? 4 : 3);
				}
			} else if (line != String.Empty) {
				var match = Regex.Match(line, @"move (\d+) from (\d+) to (\d+)");
				
				var count = Convert.ToInt32(match.Groups[1].Value);
				var from = Convert.ToInt32(match.Groups[2].Value) - 1;
				var to = Convert.ToInt32(match.Groups[3].Value) - 1;

				for (int i = 0 ; i < count; i++)
				{
					crates[to].Insert(0, crates[from][0]);
					crates[from].RemoveAt(0);
				}
			}
		}
		
		var result = crates.Select(x => x[0]).ToArray();
	    return string.Join(String.Empty, result);
	}

	// compute the answer to the second part
	public object GetAnswer2(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

		var crates = new List<List<char>>();

		var parseCrates = true;
		foreach (var line in lines)
		{
			if (parseCrates)
			{
				if (line == string.Empty)
				{
					parseCrates = false;
					continue;
				}

				var remainingCrate = line;
				var crateIndex = 0;
				while (remainingCrate.Any())
				{
					var currentCrate = remainingCrate[1];
					if (currentCrate == '1') break;

					if (crateIndex >= crates.Count)
						crates.Add(new List<char>());
					if (currentCrate != ' ')
						crates[crateIndex].Add(currentCrate);
					crateIndex++;
					remainingCrate = remainingCrate.Substring(remainingCrate.Length >= 4 ? 4 : 3);
				}
			}
			else if (line != String.Empty)
			{
				var match = Regex.Match(line, @"move (\d+) from (\d+) to (\d+)");

				var count = Convert.ToInt32(match.Groups[1].Value);
				var from = Convert.ToInt32(match.Groups[2].Value) - 1;
				var to = Convert.ToInt32(match.Groups[3].Value) - 1;

				for (int i = 0; i < count; i++)
				{
					crates[to].Insert(i, crates[from][0]);
					crates[from].RemoveAt(0);
				}
			}
		}

		var result = crates.Select(x => x[0]).ToArray();
		return string.Join(String.Empty, result);
	}
}

void Main()
{
	var automaton = new Automaton();
	automaton.RunDay<TheSolver>();
}

// You can define other methods, fields, classes and namespaces here