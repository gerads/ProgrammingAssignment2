﻿
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
        static bool Recursive = false;
        static string InputPath = Directory.GetCurrentDirectory();

        static int TotalFiles = 0;
        static int TotalDirectories = 0;
        static int TotalLinks = 0;
        static int TotalOther = 0;
        static int TotalEntries = 0;

        static void Main(string[] args)
        {
            parseArgs(args);

            if (File.Exists(InputPath))
            {
                printFile(new FileInfo(InputPath));
            }
            else if (Directory.Exists(InputPath))
            {
                traverse(InputPath);
            }

            printSummary();
        }

        static void parseArgs(string[] args)
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

        static void traverse(string path)
        {
            var currentDir = new DirectoryInfo(path);
            var files = currentDir.GetFileSystemInfos();
            foreach (var file in files)
            {
                if (file.Attributes.HasFlag(FileAttributes.Directory))
                {
                    printDirectory(file);

                    if (Recursive)
                    {
                        traverse(file.FullName);
                    }
                }
                else
                {
                    printFile(file);
                }
            }
        }

        static void printDirectory(FileSystemInfo fileSystemInfo)
        {
            DirectoryInfo dir = (DirectoryInfo)fileSystemInfo;
            Console.WriteLine(dir.Name);
        }

        static void printFile(FileSystemInfo fileSystemInfo)
        {
            FileInfo file = (FileInfo)fileSystemInfo;
            Console.WriteLine(file.Name);
        }

        static void printSummary()
        {
            
        }
    }
}
