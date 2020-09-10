using SimpleEditorLogParser.Parser;
using System;

namespace SimpleEditorLogParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var parser = new SimpleParser();
            ParseArgs(args, parser);
            parser.BeginParsing();
            parser.OutputCSVFile();
        }

        private static void ParseArgs(string[] args, SimpleParser parser)
        {
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
        }
    }
}
