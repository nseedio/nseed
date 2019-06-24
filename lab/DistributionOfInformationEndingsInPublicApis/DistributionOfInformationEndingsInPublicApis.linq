<Query Kind="Program" />

void Main()
{
	var expectedEndings = new []
	{
		"Info",
		"Information",
		"Description",
		"Descriptor"
	};
	
	var directoriesToScan = new []
	{
		@"c:\Windows\Microsoft.NET\assembly"
	};

	directoriesToScan
		.Select(directory => new DirectoryInfo(directory))
		.SelectMany(directory => directory.EnumerateFiles("*.dll", SearchOption.AllDirectories))
		.Distinct(CompareFilesByName.Default)
		.AsParallel()
		.Select(file => TryLoadAssembly(file.FullName))
		.Where(assembly => assembly != null)
		.SelectMany(assembly => GetPublicClassesThatEndWithExpectedEndings(assembly, expectedEndings))
		.OrderBy(result => result.Ending)
		.ThenBy(result => result.ClassAssemblyName)
		.ThenBy(result => result.ClassName)
		.GroupBy(result => result.Ending)		
		.Dump();
}

class Result
{
	public string Ending { get; set; }
	public string ClassName { get; set; }
	public string ClassAssemblyName { get; set; }
}

IEnumerable<Result> GetPublicClassesThatEndWithExpectedEndings(Assembly assembly, string[] expectedEndings)
{
	try
	{
		return assembly
			.GetExportedTypes()
			.Where(type => expectedEndings.Any(ending => type.Name.EndsWith(ending)))
			.Select(type => new Result
			{
				Ending = expectedEndings.First(ending => type.Name.EndsWith(ending)),
				ClassName = type.Name,
				ClassAssemblyName = type.Assembly.FullName
			});
	}
	catch
	{
		return Array.Empty<Result>();
	}
}

Assembly TryLoadAssembly(string fileName)
{
	try
	{
		return Assembly.LoadFile(fileName);
	}
	catch
	{
		return null;
	}
}

class CompareFilesByName : IEqualityComparer<FileInfo>
{
	public static CompareFilesByName Default = new CompareFilesByName();

	public bool Equals(FileInfo first, FileInfo second)
	{
		return string.Equals(first.Name, second.Name);
	}

	public int GetHashCode(FileInfo fileInfo)
	{
		return fileInfo.Name.GetHashCode();
	}
}