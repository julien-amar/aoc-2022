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
		automaton.Day = 2;
		// provides test data (optional)
		var dataSample = @"A Y
B X
C Z";
		automaton.RegisterTestDataAndResult(dataSample, 15, 1);
		automaton.RegisterTestDataAndResult(dataSample, 12, 2);
	}

	// compute the answer to the first part
	public object GetAnswer1(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		
		var score = 0;
		
		foreach (var line in lines)
		{
			var plays = line.Replace(" ", "");

			// outcome
			switch (plays)
			{
				// draw
				case "AX":
				case "BY":
				case "CZ":
					score += 3;
					break;
				// loose
				case "AZ":
				case "CY":
				case "BX":
					score += 0;
					break;
				default:
					score += 6;
					break;
			}

			// shape selected
			switch (plays[1])
			{
				case 'X':
					score += 1;
					break;
				case 'Y':
					score += 2;
					break;
				default:
					score += 3;
					break;
			}
		}
	    return score;
	}

	// compute the answer to the second part
	public object GetAnswer2(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		
		var score = 0;
		
		var wins= new Dictionary<char, string> {
			['A'] = "AY",
			['B'] = "BZ",
			['C'] = "CX",
		};
		var draw = new Dictionary<char, string>
		{
			['A'] = "AX",
			['B'] = "BY",
			['C'] = "CZ",
		};
		var loose  = new Dictionary<char, string>
		{
			['A'] = "AZ",
			['B'] = "BX",
			['C'] = "CY",
		};
		foreach (var line in lines)
		{
			var plays = line.Replace(" ", "");

			switch (plays[1])
			{
				case 'X': // need to loose
					plays = $"{loose[plays[0]]}";
					break;
				case 'Y': // need to draw
					plays = $"{draw[plays[0]]}";
					break;
				default: // need to win
					plays = $"{wins[plays[0]]}";
					break;
			}

			// outcome
			switch (plays)
			{
				// draw
				case "AX":
				case "BY":
				case "CZ":
					score += 3;
					break;
				// loose
				case "AZ":
				case "CY":
				case "BX":
					score += 0;
					break;
				default:
					score += 6;
					break;
			}

			// shape selected
			switch (plays[1])
			{
				case 'X':
					score += 1;
					break;
				case 'Y':
					score += 2;
					break;
				default:
					score += 3;
					break;
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