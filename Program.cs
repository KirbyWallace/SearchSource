using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

class Program {

	static void Main(string[] args) {

		const bool APPEND = true;

		double start = DateTime.Now.Ticks;
		double time = 0;

		string[] terms = new string[100];
		long[] termCount = new long[100];

		long matches = 0;
		long numFiles = 0;

		if (args.Length == 0) {

			// started by dbl-click in windows explorer.  Prompt for args

			int x = 0;
			Console.WriteLine("Enter search terms.  Enter blank line to continue.");

			while (true) {

				Console.Write("Search Term: ");
				string y = Console.ReadLine();

				if (y != "") {
					terms[x++] = y;
				} else {
					break;
				}
			}

		} else {

			// pick up off the command line that was used to start the program

			for (int i = 0; i < args.Length; i++) {
				terms[i] = args[i];
			}
		}



		if (args.Length == 0 && terms.FirstNull() == -1) {
			Console.WriteLine("Will search from where the .exe is located, recursively through all subfolders, and search");
			Console.WriteLine("through all .txt, .log, .csv, .cs, .css, .js, .asp, .vb, .vbs, .ts, .html, and .sql files,");
			Console.WriteLine("looking for your search terms.\n");
			Console.WriteLine("It is NOT case sensitive.\n");
			Console.WriteLine("It will show results on the console, but also create a “SearchSource.txt” file in the same");
			Console.WriteLine("location as the .exe file.\n");
			Console.WriteLine("USAGE:\n");
			Console.WriteLine("SearchSource.exe arg1, arg2, ... , arg100\n");

			return;
		}

		// clean up the terms array.  Crop all nulls leaving only actual search terms.  Don't want to compare all 100 elements
		// when 97 of them are null.

		if (terms.FirstNull() > -1) {
			terms = terms.NonNullElements();
		} else {
			// If no search terms entered, nothing to do.
			System.Environment.Exit(0);
		}



		try {
			File.Delete("SearchSource.txt");
		} catch {
			// Fine!  Do nothing.
		}

		var files = Directory.GetFiles(".\\", "*.*", SearchOption.AllDirectories)
			.Where(s => s.EndsWith(".cs") ||
						s.EndsWith(".css") ||
						s.EndsWith(".js") ||
						s.EndsWith(".asp") ||
						s.EndsWith(".vb") ||
						s.EndsWith(".vbs") ||
						s.EndsWith(".ts") ||
						s.EndsWith(".txt") ||
						s.EndsWith(".html") ||
						s.EndsWith(".sql") ||
						s.EndsWith(".log") ||
						s.EndsWith(".csv"));

		StreamWriter sw = new StreamWriter("SearchSource.txt", APPEND);

		Console.WriteLine("\nSearch Terms: " + String.Join(", ", terms) + "\n");
		sw.WriteLine("\nSearch Terms: " + String.Join(", ", terms) + "\n");

		foreach (string file in files) {

			numFiles++;

			string lines = File.ReadAllText(file);

			string[] aLines = lines.Split('\n');

			if (aLines.Length == 0) {
				continue;
			}

			for (int j = 0; j < aLines.Length; j++) {

				for (int i = 0; i < terms.Length; i++) {

					if ((aLines[j].ToLower().Contains(terms[i].ToLower()))
						&& (file.ToLower().IndexOf("jquery") == -1)
						&& (file.ToLower().IndexOf("datatables") == -1)
						&& (file.ToLower().IndexOf("chosen") == -1)
						&& (file.ToLower().IndexOf("shadowbox") == -1)
						&& (file.ToLower().IndexOf("tinymce") == -1)
						&& (file.ToLower().IndexOf("datejs") == -1)
						&& (file.ToLower().IndexOf("_api") == -1)
						) {

						matches++;

						termCount[i] = termCount[i] + 1;

						sw.WriteLine("File: " + file + " Line: " + j + " Found: " + terms[i]);
						sw.WriteLine(aLines[j].Trim() + "\n");

						Console.WriteLine("File: " + file + " Line: " + j + " Found: " + terms[i]);
						Console.WriteLine(aLines[j].Trim() + "\n");

					}
				}
			}
		}

		time = DateTime.Now.Ticks - start;

		sw.WriteLine("\nFIN.\n");

		sw.WriteLine(numFiles + " Files Searched.");
		sw.WriteLine((time / (double)10000000).ToString() + " seconds.");
		sw.WriteLine(matches + " Matches Found.\n");

		Console.WriteLine("\nFIN.\n");

		Console.WriteLine(numFiles + " Files Searched.");
		Console.WriteLine((time / (double)10000000).ToString() + " seconds.");
		Console.WriteLine(matches + " Matches Found.\n");

		for (int i = 0; i < terms.Length; i++) {
			if (terms[i] != "") {
				Console.WriteLine(terms[i] + " = " + termCount[i].ToString());
				sw.WriteLine(terms[i] + " = " + termCount[i].ToString());
			}
		}

		sw.Flush();
		sw.Close();
		sw.Dispose();


		// if no args were passed, then the program was probably started by dbl-click on file in 
		// windows explorer, and search terms entered one at a time.  So, keep window open at end
		// to see results.  Not necessary of args.length > 0 because that means probably started
		// at a command line with args.

		if (args.Length == 0) {
			Console.WriteLine("\nPress any key to exit.");
			Console.ReadKey();
		}

	}

}

public static class ExtentionMethods {

	public static int FirstNull(this string[] sourceArray) {

		for (int i = 0; i < sourceArray.Length; i++) {
			if (sourceArray[i] == null) {
				return i;
			}
		}

		return -1;
	}

	public static string[] NonNullElements(this string[] sourceArray) {

		List<string> x = new List<string>();

		for (int i = 0; i < sourceArray.Length; i++) {
			if (sourceArray[i] != null) {
				x.Add(sourceArray[i]);
			} else {
				break;
			}
		}

		return x.ToArray();
	}

}
