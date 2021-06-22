using System;
using System.Linq;
using System.IO;

class Program {

	static void Main(string[] args) {

		const bool APPEND = true;

		double start = DateTime.Now.Ticks;
		double time = 0;

		string[] terms = new string[100];
		long[] termCount = new long[100];

		long matches = 0;
		long numFiles = 0;

		for (int i = 0; i < args.Length; i++) {
			terms[i] = args[i];
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

		Console.WriteLine("\nSearch Terms: " + String.Join(", ", args) + "\n");
		sw.WriteLine("\nSearch Terms: " + String.Join(", ", args) + "\n");

		foreach (string file in files) {

			numFiles++;

			string lines = File.ReadAllText(file);

			string[] aLines = lines.Split('\n');

			if (aLines.Length == 0) {
				continue;
			}

			for (int j = 0; j < aLines.Length; j++) {

				for (int i = 0; i < args.Length; i++) {

					if ((aLines[j].ToLower().Contains(args[i].ToLower()))
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

						sw.WriteLine("File: " + file + " Line: " + j + " Found: " + args[i]);
						Console.WriteLine("File: " + file + " Line: " + j + " Found: " + args[i]);

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

		for (int i = 0; i < args.Length; i++) {
			if (terms[i] != "") {
				Console.WriteLine(terms[i] + " = " + termCount[i].ToString());
				sw.WriteLine(terms[i] + " = " + termCount[i].ToString());
			}
		}

		sw.Flush();
		sw.Close();
		sw.Dispose();

	}
}

