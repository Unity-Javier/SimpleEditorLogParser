using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            var fileInfo = new FileInfo(EditorLogPath);

            if(fileInfo.Length == 0)
            {
                Console.WriteLine($"File at path {EditorLogPath} was empty");
                return;
            }

            var doneImportingLines = GetAllLines(EditorLogPath, "Done importing asset:");

            if (doneImportingLines.Length != 0)
            {
                Timings = GenerateTimings(doneImportingLines);
            }
            else
            { 
                //Must be a newer version
                doneImportingLines = GetAllLines(EditorLogPath, "Start importing ");

                if(doneImportingLines.Length == 0)
                {
                    Console.WriteLine("Log is not in a supported format. Please implement new parsing for new format present in this log file.");
                    return;
                }

                Timings = GenerateTimings_2020_2_OR_NEWER(doneImportingLines);
            }            
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

        string[] GetAllLines(string editorLogFile, string keyword)
        {
            var matchingLines = File.ReadLines(editorLogFile).Where(line => line.Contains(keyword));
            return matchingLines.ToArray();
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

        private static AssetTimings[] GenerateTimings_2020_2_OR_NEWER(string[] doneImportingLines)
        {
            Regex pathRegex = new Regex("importing .*using Guid", RegexOptions.Compiled);
            Regex secondsRegex = new Regex(@" [0-9]+\.[0-9]+ ", RegexOptions.Compiled);

            List<AssetTimings> timings = new List<AssetTimings>();
            foreach (var curLine in doneImportingLines)
            {
                var pathMatch = pathRegex.Match(curLine);
                var secondsMatch = secondsRegex.Match(curLine);

                if (pathMatch.Success && secondsMatch.Success)
                {
                    var pathCleaned = pathMatch.ToString().Replace("importing ", string.Empty);
                    pathCleaned = pathCleaned.Replace(" using Guid", string.Empty);
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