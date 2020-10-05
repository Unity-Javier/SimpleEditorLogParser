using SimpleEditorLogParser.Parser;
using System;

namespace SimpleEditorLogParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var parser = new SimpleParser();
            if (ParseArgs(args, parser))
            {
                parser.BeginParsing();
                parser.OutputCSVFile();
            }
        }

        private static bool ParseArgs(string[] args, SimpleParser parser)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("");
                Console.WriteLine("Usage:");
                Console.WriteLine("--path:      Specifies path to a Unity generated Log file, which will be the file to parse.");
                Console.WriteLine("--output:    Specifies path to where the CSV containing the categorized entries should be written to.");
                Console.WriteLine("");
                Console.WriteLine(@"Sample command line: dotnet SimpleLogParser.dll --path C:\Projects\Game01\Editor.log --output C:\Projects\Game01\CategorizedLog.csv");
                Console.WriteLine("");
                return false;
            }

            for (int i = 0; i < args.Length; ++i)
            {
                if (args[i] == "--path")
                {
                    parser.EditorLogPath = args[i + 1];
                }
                else if (args[i] == "--output")
                {
                    parser.OutputPath = args[i + 1];
                }
            }

            if (parser.EditorLogPath == null)
            {
                Console.WriteLine("Error: Path to log file must be specified. Please use --path followed by the path to the file.");
                return false;
            }

            if (parser.OutputPath == null)
            {
                Console.WriteLine("Error: Path to where categorized CSV file should be written to was not specified. Please add --output followed by the path to where this file should be written.");
                return false;
            }

            return true;
        }
    }
}
