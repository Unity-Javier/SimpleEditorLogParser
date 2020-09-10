using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SimpleEditorLogParser.Parser
{
    public class SimpleParser
    {
        public string EditorLogPath { get; set; }
        public string OutputPath { get; set; }
        public AssetTimings[] Timings { get; set; }

        public SimpleParser() {}

        public void BeginParsing()
        {
            if(!File.Exists(EditorLogPath))
            {
                Console.WriteLine($"Path does not exist: {EditorLogPath}");
                return;
            }

            var fileContents = File.ReadAllText(EditorLogPath);

            if(fileContents.Length == 0)
            {
                Console.WriteLine($"File at path {EditorLogPath} was empty");
                return;
            }

            var doneImportingLines = GetAllLines(fileContents, "Done importing asset:");
            Timings = GenerateTimings(doneImportingLines);
        }

        public void OutputCSVFile()
        {
            if(Timings == null)
            {
                Console.WriteLine("Timings have not been created yet. Please call BeginParsing first");
                return;
            }

            StringBuilder csv = new StringBuilder();
            csv.Append("Path,Extension,Category,Import Time (s)");
            csv.AppendLine();

            foreach (var assetTiming in Timings)
            {
                csv.Append(assetTiming.ToString());
                csv.AppendLine();
            }

            File.WriteAllText(OutputPath, csv.ToString());

            Console.WriteLine($"CSV file has been created at {OutputPath}");
        }

        string[] GetAllLines(string file, string keyword)
        {
            var allLines = file.Split('\n');

            List<string> validLines = new List<string>();
            foreach (var curLine in allLines)
            {
                if (curLine.IndexOf(keyword) != -1)
                {
                    validLines.Add(curLine);
                }
            }

            return validLines.ToArray();
        }

        AssetTimings[] GenerateTimings(string[] doneImportingLines)
        {
            Regex pathRegex = new Regex("'.*' ", RegexOptions.Compiled);
            Regex secondsRegex = new Regex(@" [0-9]+\.[0-9]+ ", RegexOptions.Compiled);

            List<AssetTimings> timings = new List<AssetTimings>();
            foreach (var curLine in doneImportingLines)
            {
                var pathMatch = pathRegex.Match(curLine);
                var secondsMatch = secondsRegex.Match(curLine);

                if (pathMatch.Success && secondsMatch.Success)
                {
                    var pathCleaned = pathMatch.ToString().Replace('\'', ' ');

                    pathCleaned = pathCleaned.Trim();

                    if (pathCleaned.IndexOf(",") >= 0)
                        pathCleaned = "\"" + pathCleaned + "\"";

                    var timing = new AssetTimings(pathCleaned, secondsMatch.ToString().Trim());
                    timings.Add(timing);
                }
            }

            return timings.ToArray();
        }
    }
}