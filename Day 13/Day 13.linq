<Query Kind="Program">
  <NuGetReference>AoC</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>AoC</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
</Query>

internal class PacketComparer : IComparer<object>
{
	private const int RIGHT_ORDER = -1;
	private const int NOT_RIGHT_ORDDER = 1;
	private const int CONTINUE_COMPARISON = 0;
	
	public int Compare(object first, object second)
	{
		// If both values are integers, the lower integer should come first.
		// - If the left integer is lower than the right integer, the inputs are in the right order
		// - If the left integer is higher than the right integer, the inputs are not in the right order.
		// - Otherwise, the inputs are the same integer; continue checking the next part of the input.
		if (first is JValue && second is JValue)
		{
			var firstValue = first as JValue;
			var secondValue = second as JValue;

			if (Convert.ToInt32(firstValue.Value) < Convert.ToInt32(secondValue.Value)) return RIGHT_ORDER;
			if (Convert.ToInt32(firstValue.Value) > Convert.ToInt32(secondValue.Value)) return NOT_RIGHT_ORDDER;
			
			return CONTINUE_COMPARISON;
		}

		// If both values are lists, compare the first value of each list, then the second value, and so on.
		// - If the left list runs out of items first, the inputs are in the right order
		// - If the right list runs out of items first, the inputs are not in the right order.
		// - If the lists are the same length and no comparison makes a decision about the order, continue checking the next part of the input.
		if (first is JArray && second is JArray)
		{
			var firstArray = first as JArray;
			var secondArray = second as JArray;
			for (var i = 0; i < Math.Min(firstArray.Count, secondArray.Count); ++i)
			{
				var orderComparison = Compare(firstArray[i], secondArray[i]);
				if (orderComparison != 0) return orderComparison;
			}

			if (firstArray.Count < secondArray.Count) return RIGHT_ORDER;
			if (firstArray.Count > secondArray.Count) return NOT_RIGHT_ORDDER;

			return CONTINUE_COMPARISON;
		}

		// If exactly one value is an integer, convert the integer to a list which contains that integer as its only value, then retry the comparison.
		// For example, if comparing[0, 0, 0] and 2, convert the right value to [2] (a list containing 2); the result is then found by instead comparing [0, 0, 0] and [2].
		if (first is JValue) return Compare(new JArray(first), second);
		if (second is JValue) return Compare(first, new JArray(second));

		throw new NotImplementedException();
	}
}

internal class TheSolver : ISolver
{
	// provides the puzzle data
	public void SetupRun(Automaton automaton)
	{
		// set the day number (mandatory)
		automaton.Day = 13;

		// provides test data (optional)
		var dataSample = @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]";
		automaton.RegisterTestDataAndResult(dataSample, 13, 1);
		automaton.RegisterTestDataAndResult(dataSample, 140, 2);
	}
	
	// compute the answer to the first part
	public object GetAnswer1(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
		var score = 0;
		var comparer = new PacketComparer();
		for (int i = 0; i < lines.Length; i += 2) {
			var first = JArray.Parse(lines[i]);
			var second = JArray.Parse(lines[i + 1]);
			score += comparer.Compare(first, second) < 0 ? (i / 2 + 1) : 0;
		}
		return score;
	}

	// compute the answer to the second part
	public object GetAnswer2(string data)
	{
		var firstDeviderPacket = "[[2]]";
		var secondDeviderPacket = "[[6]]";

		var packets = data
			.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
			.Concat(new[] { firstDeviderPacket, secondDeviderPacket })
			.Select(x => JArray.Parse(x))
			.Cast<object>()
			.ToList();

		packets.Sort(new PacketComparer());
		
		var packetsStream = packets.Select(x => JsonConvert.SerializeObject(x)).ToList();
		
		return (packetsStream.IndexOf(firstDeviderPacket) + 1) * (packetsStream.IndexOf(secondDeviderPacket) + 1);
	}
}

void Main()
{
	var automaton = new Automaton();
	automaton.RunDay<TheSolver>();
}
