<Query Kind="Program">
  <NuGetReference>AoC</NuGetReference>
  <Namespace>AoC</Namespace>
</Query>

internal class Monkey
{
	public int Id;
	public Queue<ulong> Items;
	public string Operator;
	public string OperatorValue;
	public ulong Test;
	public int TestSuccess;
	public int TestFailure;

	public Monkey(int monkeyId, ulong[] monkeyItems, string monkeyOperator, string monkeyOperatorValue, ulong monkeyTest, int monkeyTestSuccess, int monkeyTestFailure)
	{
		this.Id = monkeyId;
		this.Items = new Queue<ulong>(monkeyItems);
		this.Operator = monkeyOperator;
		this.OperatorValue = monkeyOperatorValue;
		this.Test = monkeyTest;
		this.TestSuccess = monkeyTestSuccess;
		this.TestFailure = monkeyTestFailure;
	}

	public void Compute(List<Monkey> monkeys, Func<ulong, ulong> fatigue)
	{
		while (Items.Any()) {
			var item = Items.Dequeue();
			var value = OperatorValue == "old" ? item : Convert.ToUInt64(OperatorValue);
			ulong worryLevel;
			switch (Operator) {
				case "*":
					worryLevel = item * value;
					break;
				case "+":
					worryLevel = item + value;
					break;
				default:
					throw new NotImplementedException();
			}
			
			worryLevel = fatigue(worryLevel);

			if (worryLevel % Test == 0)
			{
				monkeys[TestSuccess].Send(worryLevel);
			}
			else
			{
				monkeys[TestFailure].Send(worryLevel);
			}
		}
	}

	private void Send(ulong item)
	{
		Items.Enqueue(item);
	}
}

internal class TheSolver : ISolver
{
	// provides the puzzle data
	public void SetupRun(Automaton automaton)
	{
		// set the day number (mandatory)
		automaton.Day = 11;

		// provides test data (optional)

		var dataSample = @"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1";
		automaton.RegisterTestDataAndResult(dataSample, 10605, 1);
		automaton.RegisterTestDataAndResult(dataSample, (ulong)2713310158, 2);
	}

	private List<Monkey> ParseInput(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.TrimEntries);
		var monkeys = new List<Monkey>();

		while (lines.Any())
		{
			var monkeyData = lines.Take(7).ToArray();

			var monkeyId = Convert.ToInt32(Regex.Match(monkeyData[0], @"Monkey (\d)+:").Groups[1].Value);
			var monkeyItems = Regex.Match(monkeyData[1], @"Starting items: (.*)").Groups[1].Value.Replace(" ", string.Empty).Split(",").Select(x => Convert.ToUInt64(x)).ToArray();

			var operationInfo = Regex.Match(monkeyData[2], @"Operation: new = old (.) (.*)").Groups;

			var monkeyOperator = operationInfo[1].Value;
			var monkeyOperatorValue = operationInfo[2].Value;

			var monkeyTest = Convert.ToUInt64(Regex.Match(monkeyData[3], @"Test: divisible by (\d+)").Groups[1].Value);

			var monkeyTestSuccess = Convert.ToInt32(Regex.Match(monkeyData[4], @"If true: throw to monkey (\d+)").Groups[1].Value);
			var monkeyTestFailure = Convert.ToInt32(Regex.Match(monkeyData[5], @"If false: throw to monkey (\d+)").Groups[1].Value);

			var monkey = new Monkey(monkeyId, monkeyItems, monkeyOperator, monkeyOperatorValue, monkeyTest, monkeyTestSuccess, monkeyTestFailure);

			monkeys.Add(monkey);
			lines = lines.Skip(7).ToArray();
		}
		
		return monkeys;
	}
	
	// compute the answer to the first part
	public object GetAnswer1(string data)
	{
		var monkeys = ParseInput(data);
		var score = new int[monkeys.Count];
		for (int round = 0; round < 20; ++round)
		{
			foreach (var monkey in monkeys)
			{
				score[monkey.Id] += monkey.Items.Count;
				monkey.Compute(monkeys, worryLevel => worryLevel / 3);
			}
		}

		var monkeyBusiness = score
			.OrderByDescending(x => x)
			.Take(2)
			.Aggregate(1, (a, b) => a * b);
	    return monkeyBusiness;
	}

	// compute the answer to the second part
	public object GetAnswer2(string data)
	{
		var monkeys = ParseInput(data);
		var capWorryLevelValue = monkeys.Aggregate((ulong)1, (a, b) => a * b.Test);
		var score = new ulong[monkeys.Count];
		for (int round = 0; round < 10000; ++round)
		{
			foreach (var monkey in monkeys)
			{
				score[monkey.Id] += (ulong)monkey.Items.Count;
				monkey.Compute(monkeys, worryLevel => worryLevel % capWorryLevelValue);
			}
		}

		var monkeyBusiness = score
			.OrderByDescending(x => x)
			.Take(2)
			.Aggregate((ulong)1, (a, b) => a * b);
		return monkeyBusiness;
	}
}

void Main()
{
	var automaton = new Automaton();
	automaton.RunDay<TheSolver>();
}
