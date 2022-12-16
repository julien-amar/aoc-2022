<Query Kind="Program">
  <NuGetReference>AoC</NuGetReference>
  <Namespace>AoC</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

public class Interval
{
	public int FromLongitude { get; set; }
	public int ToLongitude { get; set; }
}

internal class TheSolver : ISolver
{
	bool _isTestingAnswer1 = false;
	bool _isTestingAnswer2 = false;
	
	// provides the puzzle data
	public void SetupRun(Automaton automaton)
	{
		// set the day number (mandatory)
		automaton.Day = 15;

		// provides test data (optional)
		var dataSample = @"Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3";

		if (_isTestingAnswer1)
			automaton.RegisterTestDataAndResult(dataSample, 26, 1);
		if (_isTestingAnswer2)
			automaton.RegisterTestDataAndResult(dataSample, (ulong)56000011, 2);
	}

	// compute the answer to the first part
	public object GetAnswer1(string data)
	{
		var latitude = _isTestingAnswer1 ? 10 : 2000000;
		
		var input = ParseInput(data);
		
		var beaconsLongitudesAtLatitude = input
			.Where(x => x.Beacon.Y == latitude)
			.Select(x => x.Beacon.X)
			.Distinct()
			.ToArray();
		
		return GetSignalIntervalsAtLatitude(input, latitude)
			.SelectMany(signalInterval => Enumerable.Range(signalInterval.FromLongitude, signalInterval.ToLongitude - signalInterval.FromLongitude + 1))
			.Except(beaconsLongitudesAtLatitude)
			.Count();
	}

	// compute the answer to the second part
	public object GetAnswer2(string data)
	{
		var minLongitude = 0;
		var maxLongitude = _isTestingAnswer2 ? 20 : 4000000;
		
		var input = ParseInput(data);
		
		var tunningFrequency = Enumerable.Range(minLongitude, maxLongitude - minLongitude + 1)
			.Select(latitude =>
			{
				var signalIntervalsAtLatitude = GetSignalIntervalsAtLatitude(input, latitude).ToArray();
					
				foreach (var signalInterval in signalIntervalsAtLatitude)
				{
					if (signalInterval.FromLongitude > minLongitude)
						return (ulong)(signalInterval.FromLongitude - 1) * 4000000 + (ulong)latitude;
					if (signalInterval.ToLongitude < maxLongitude)
						return (ulong)(signalInterval.ToLongitude + 1) * 4000000 + (ulong)latitude;
				}
				return (ulong?)null;
			})
			.Where(tunningFrequency => tunningFrequency.HasValue)
			.Select(tunningFrequency => tunningFrequency.Value)
			.OrderByDescending(tunningFrequency => tunningFrequency)
			.First();

		return tunningFrequency;
	}

	IEnumerable<(Point Sensor, Point Beacon)> ParseInput(string data)
	{
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToArray();

		foreach (var line in lines)
		{
			var match = Regex.Match(line, @"Sensor at x=([-\d]+), y=([-\d]+): closest beacon is at x=([-\d]+), y=([-\d]+)");

			if (match.Groups.Count != 5) throw new ArgumentException();

			var sensor = new Point(
				Convert.ToInt32(match.Groups[1].Value),
				Convert.ToInt32(match.Groups[2].Value));

			var beacon = new Point(
				Convert.ToInt32(match.Groups[3].Value),
				Convert.ToInt32(match.Groups[4].Value));

			yield return (sensor, beacon);
		}
	}
	
	private IEnumerable<Interval> GetSignalIntervalsAtLatitude(IEnumerable<(Point Sensor, Point Beacon)> input, int latitude)
	{
		var transmissionsAtLatitude = input
			// Filter out all sensors that don't cover latitude
			.Where(signal =>
			{
				var signalReach = Math.Abs(signal.Sensor.X - signal.Beacon.X) + Math.Abs(signal.Sensor.Y - signal.Beacon.Y);
				return signal.Sensor.Y - signalReach <= latitude && latitude <= signal.Sensor.Y + signalReach;
			})
			// Retrieve sensor's signal longitude coverage for the latitude
			.Select(signal =>
			{
				var signalReach = Math.Abs(signal.Sensor.X - signal.Beacon.X) + Math.Abs(signal.Sensor.Y - signal.Beacon.Y);
				var signalLoss = Math.Abs(latitude - signal.Sensor.Y);
				return new Interval
				{
					FromLongitude = signal.Sensor.X - signalReach + signalLoss,
					ToLongitude = signal.Sensor.X + signalReach - signalLoss,
				};
			})
			.OrderBy(x => x.FromLongitude)
			.ToArray();

		// Merge all sensor coverage at latitude
		var mergeTransmissionIntervals = new Stack<Interval>();
		mergeTransmissionIntervals.Push(transmissionsAtLatitude[0]);
		for (int i = 1; i < transmissionsAtLatitude.Length; i++)
		{
			Interval top = mergeTransmissionIntervals.Peek();
			if (top.ToLongitude < transmissionsAtLatitude[i].FromLongitude)
				mergeTransmissionIntervals.Push(transmissionsAtLatitude[i]);
			else if (top.ToLongitude < transmissionsAtLatitude[i].ToLongitude)
				top.ToLongitude = transmissionsAtLatitude[i].ToLongitude;
		}
		while (mergeTransmissionIntervals.Any())
		{
			yield return mergeTransmissionIntervals.Pop();
		}
	}
}

void Main()
{
	var automaton = new Automaton();
	automaton.RunDay<TheSolver>();
}
