// See https://aka.ms/new-console-template for more information
using System.Globalization;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

internal class Program
{
	private static void Main(string[] args)
	{
		Console.Write("Enter the FULL directory of where your unsorted photos are: ");
		var fileDirectory = Console.ReadLine();

		if (!System.IO.Directory.Exists(fileDirectory))
		{
			Console.WriteLine("Not a valid directory");
			return;
		}
		Console.Write("Enter the FULL directory of where you want the sorted items to be: ");

		var workingDirectory = Console.ReadLine();
		if (!System.IO.Directory.Exists(workingDirectory))
		{
			Console.WriteLine("Not a valid directory");
			return;
		}

		Console.Write("Do you want to replace duplicates? (y/n): ");
		var replaceDuplicateOption = Console.ReadLine();
        bool replaceDuplicates;
        switch (replaceDuplicateOption)
		{
			case "y":
				replaceDuplicates = true;
				break;
			case "n":
				replaceDuplicates = false;
				break;
			default:
				Console.WriteLine("Invalid Option.");
				Console.ReadKey();
				return;
		}

		Console.WriteLine("Discovering Directories...");

		var allFiles = ReturnAllFiles(fileDirectory);
		Console.WriteLine($"Found {allFiles.Count} files. Press any key to continue with the move.");
		Console.ReadKey();
		var unsortedDirectory = $@"{workingDirectory}\unsorted_items";
		var filesWithIssues = $@"{workingDirectory}\files_with_issues";


		System.IO.Directory.CreateDirectory(unsortedDirectory);
		System.IO.Directory.CreateDirectory(filesWithIssues);

		Console.WriteLine($"Discovered {allFiles.Count} Files");
		foreach (var file in allFiles)
		{
			var fileName = Path.GetFileName(file);
			var finalFileDirectory = "";

			try
			{
				var directories = ImageMetadataReader.ReadMetadata(file);
				var success = ReturnDateTime(directories, out var date);

				if (success)
				{
					var directoryFriendlyDate = date.ToString("dd-MM-yyyy");
					var parentDirectory = $@"{workingDirectory}\{directoryFriendlyDate}";
					System.IO.Directory.CreateDirectory(parentDirectory);
					finalFileDirectory = $@"{parentDirectory}\{fileName}";
				}
				else
				{
					Console.WriteLine($"File {fileName} has no date");
					finalFileDirectory = $@"{unsortedDirectory}\{fileName}";
				}

				
			}
			catch (ImageProcessingException)
			{
				Console.WriteLine($"File {fileName} has an unrecognised format");
				finalFileDirectory = $@"{filesWithIssues}\{fileName}";
			}
			catch (IndexOutOfRangeException)
			{
				Console.WriteLine($"File {fileName} has an unrecognised format");
				finalFileDirectory = $@"{filesWithIssues}\{fileName}";
			}

			if (File.Exists(finalFileDirectory)) {
				if (replaceDuplicates) {
					Console.WriteLine($"File {fileName} is a duplicate. Replacing...");
				} else {
					var guid = Guid.NewGuid().ToString();
					Console.WriteLine($"File {fileName} is a duplicate. Appending {guid} to the start of filename");
					finalFileDirectory = $@"{filesWithIssues}\{guid}-{fileName}";
				}
			}
			File.Move(file, finalFileDirectory, replaceDuplicates);
		}

		Console.WriteLine("Done!");
		Console.ReadKey();
		bool ReturnDateTime(IReadOnlyList<MetadataExtractor.Directory> directories, out DateOnly date)
		{

			date = DateOnly.MinValue;
			foreach (var item in directories)
			{
				foreach (var tag in item.Tags)
				{
					if (tag.Type == ExifDirectoryBase.TagDateTimeOriginal)
					{
						date = DateOnly.FromDateTime(DateTime.ParseExact(tag.Description!, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture));
						return true;
					}
				}
			}

			return false;
		}

		List<string> ReturnAllFiles(string path)
		{
			var finalList = new List<string>();
			var folders = System.IO.Directory.EnumerateFileSystemEntries(path);
			foreach (var file in folders)
			{
				if (System.IO.Directory.Exists(file))
				{
					finalList.AddRange(ReturnAllFiles(file));
					continue;
				}
				if (File.Exists(file))
				{
					finalList.Add(file);
				}
			}
			return finalList;
		}
	}
}