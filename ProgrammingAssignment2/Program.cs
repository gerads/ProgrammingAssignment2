
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProgrammingAssignment2
{
    class Program
    {
        static readonly string FILE_FORMAT = "{0,-7} {1,10} {2,20} {3,-50}";

        static bool Recursive = false;
        static string InputPath = Directory.GetCurrentDirectory();

        static Dictionary<string, int> Summary = new Dictionary<string, int>()
        {
            { "Files", 0 },
            { "Directories", 0 },
            { "Entries", 0 }
        };

        static int Main(string[] args)
        {
            ParseArgs(args);

            if (File.Exists(InputPath))
            {
                PrintFile(new FileInfo(InputPath));
                Summary["Files"]++;
            }
            else if (Directory.Exists(InputPath))
            {
                Traverse(InputPath);
            }
            else
            {
                return 1;
            }

            PrintSummary();

//For testing in VS - can remove if unwanted.
#if DEBUG
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
#endif
            return 0;
        }

        static void ParseArgs(string[] args)
        {
            foreach (var arg in args)
            {
                if (arg.Equals("-r", StringComparison.InvariantCultureIgnoreCase))
                {
                    Recursive = true;
                }
                else
                {
                    InputPath = arg;
                }
            }
        }

        static void Traverse(string path)
        {
            Queue<DirectoryInfo> directories = new Queue<DirectoryInfo>();

            directories.Enqueue(new DirectoryInfo(path));

            while (directories.Count > 0)
            {
                var currentDirectory = directories.Dequeue();
                PrintDirectory(currentDirectory);

                try
                {
                    foreach (var file in currentDirectory.GetFiles())
                    {
                        PrintFile(file);
                    }
                }
                catch
                {
                    //can't do it
                }

                try
                {
                    if (Recursive)
                    {
                        foreach (var dir in currentDirectory.GetDirectories())
                        {
                            directories.Enqueue(dir);
                        }
                    }
                }
                catch
                {

                }
            }
        }

        static void PrintDirectory(FileSystemInfo file)
        {
            var line = string.Format(FILE_FORMAT,
                file.Attributes.GetHashCode(),
                string.Empty,
                file.CreationTimeUtc,
                file.Name);
            Console.WriteLine(line);
            Summary["Directories"]++;
        }

        static void PrintFile(FileSystemInfo file)
        {
            var line = string.Format(FILE_FORMAT,
                file.Attributes.GetHashCode(),
                ((FileInfo)file)?.Length.ToString() ?? "-",
                file.CreationTimeUtc,
                file.Name);
            Console.WriteLine(line);
            Summary["Files"]++;
        }

        static void PrintSummary()
        {
            Summary["Entries"] = Summary.Where(c => !c.Key.Equals("Entries", StringComparison.InvariantCultureIgnoreCase)).Sum(c => c.Value);

            var summary = new StringBuilder();

            foreach (var category in Summary)
            {
                summary.AppendLine(string.Format("{0,-20} {1,10}", string.Format("Total {0}:", category.Key), category.Value));
            }

            Console.WriteLine(summary.ToString());
        }
    }
}
