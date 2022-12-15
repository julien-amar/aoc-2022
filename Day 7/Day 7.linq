<Query Kind="Program">
  <NuGetReference>AoC</NuGetReference>
  <Namespace>AoC</Namespace>
</Query>

public class FileEntry
{
	private FileEntry Root { get; set; }
	private FileEntry Parent { get; set; }
	private int Size { get; set; }

	public string Name { get; private set; }
	public List<FileEntry> Child { get; private set; }

	public Lazy<int> RecursiveSize => new Lazy<int>(() => Child.Select(x => x.Size + x.RecursiveSize.Value).Sum());
	
	private FileEntry(string name, int size)
	{
		Name = name;
		Size = size;
		Child = new List<UserQuery.FileEntry>();
	}
	
	private FileEntry(FileEntry root, FileEntry parent, string name, int size)
		: this(name, size)
	{
		Root = root;
		Parent = parent;
	}

	public FileEntry ChangeDirectory(string child) {
		if (child == "/") {
			return Root;
		}
		if (child == "..") {
			return Parent;
		}
		return Child.Single(x => x.Name == child);
	}
	
	public FileEntry CreateFolder(string name) {
		var directory = new FileEntry(Root, this, name, 0);
		Child.Add(directory);
		return directory;
	}

	public void CreateFile(string name, int size)
	{
		Child.Add(new FileEntry(Root, this, name, size));
	}

	public static FileEntry CreateRoot()
	{
		var root = new FileEntry("/", 0);
		root.Root = root;
		return root;
	}
}

internal class TheSolver : ISolver
{
	// provides the puzzle data
	public void SetupRun(Automaton automaton)
	{
		// set the day number (mandatory)
		automaton.Day = 7;
		// provides test data (optional)
		var dataSample = @"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k";
		automaton.RegisterTestDataAndResult(dataSample, 95437, 1);
		automaton.RegisterTestDataAndResult(dataSample, 24933642, 2);
	}

	private const string SPACE = " ";
	private const string PROMPT = "$" + SPACE;
	private const string COMMAND_CD = "cd";
	private const string COMMAND_LS = "ls";
	private const string DIRECTORY = "dir";

	private (FileEntry root, List<FileEntry> directories) ComputeInput(string data)
	{
		var directories = new List<FileEntry>();
		var root = FileEntry.CreateRoot();
		
		var currentNode = root;
		var lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < lines.Length; ++i)
		{
			var line = lines[i];
			var command = line.Substring(PROMPT.Length);

			if (command.StartsWith(COMMAND_CD + SPACE))
			{
				var folder = command.Substring(COMMAND_CD.Length + SPACE.Length);
				currentNode = currentNode.ChangeDirectory(folder);
			}
			else if (command == COMMAND_LS)
			{
				i++;
				while (i < lines.Length && !lines[i].StartsWith(PROMPT))
				{
					var fileInfo = lines[i].Split(" ");
					if (fileInfo[0] == DIRECTORY)
					{
						var directory = currentNode.CreateFolder(fileInfo[1]);
						directories.Add(directory);
					}
					else
					{
						currentNode.CreateFile(fileInfo[1], Convert.ToInt32(fileInfo[0]));
					}
					i++;
				}
				i--;
			}
		}
		return (root, directories);
	}
	
	// compute the answer to the first part
	public object GetAnswer1(string data)
	{
		var (_, directories) = ComputeInput(data);
		
		return directories.Select(x => x.RecursiveSize.Value).Where(x => x <= 100000).Sum();
	}

	// compute the answer to the second part
	public object GetAnswer2(string data)
	{
		var (root, directories) = ComputeInput(data);

		var rootSize = root.RecursiveSize.Value;
		return directories
			.Select(x => x.RecursiveSize.Value)
			.OrderBy(x => x)
			.Where(x => 70000000 - rootSize + x >= 30000000)
			.First();
	}
}

void Main()
{
	var automaton = new Automaton();
	automaton.RunDay<TheSolver>();
}

// You can define other methods, fields, classes and namespaces here