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
		automaton.Day = 6;
		// provides test data (optional)
		automaton.RegisterTestDataAndResult(@"bvwbjplbgvbhsrlpgdmjqwftvncz", 5, 1);
		automaton.RegisterTestDataAndResult(@"nppdvjthqldpwncqszvftbrmjlhg", 6, 1);
		automaton.RegisterTestDataAndResult(@"nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10, 1);
		automaton.RegisterTestDataAndResult(@"zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11, 1);

		automaton.RegisterTestDataAndResult(@"mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19, 2);
		automaton.RegisterTestDataAndResult(@"bvwbjplbgvbhsrlpgdmjqwftvncz", 23, 2);
		automaton.RegisterTestDataAndResult(@"nppdvjthqldpwncqszvftbrmjlhg", 23, 2);
		automaton.RegisterTestDataAndResult(@"nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29, 2);
		automaton.RegisterTestDataAndResult(@"zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26, 2);
	}

	// compute the answer to the first part
	public object GetAnswer1(string data)
	{
		data = data.Trim();

		for (int i = 0; i < data.Length - 4; ++i)
		{
			var subStr = data.Substring(i, 4);
			var hasDuplicate = subStr
				.GroupBy(x => x)
				.Any(x => x.Count() > 1);
			if (!hasDuplicate) {
				return i + 4;
			}
		}
		return -1;
	}

	// compute the answer to the second part
	public object GetAnswer2(string data)
	{
		data = data.Trim();

		for (int i = 0; i < data.Length - 14; ++i)
		{
			var subStr = data.Substring(i, 14);
			var hasDuplicate = subStr
				.GroupBy(x => x)
				.Any(x => x.Count() > 1);
			if (!hasDuplicate)
			{
				return i + 14;
			}
		}
		return -1;
	}
}

void Main()
{
	var automaton = new Automaton();
	automaton.RunDay<TheSolver>();
}
