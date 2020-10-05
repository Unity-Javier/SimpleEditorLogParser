# SimpleEditorLogParser
These sample Scripts can be used to Parse an Editor.Log file and output a CSV to analyze import times for the Unity Editor

# How to use:

Using Visual Studio, open up the Solution File in this project and edit the Debug command line settings

Alternatively, via command line use:
`dotnet SimpleLogParser.dll --path C:\Projects\Game01\Editor.log --output C:\Projects\Game01\CategorizedLog.csv`

# Parameters:

*--path*

This is the full path to the log file you have

For example `C:\Projects\SimpleEditorLogParser\SampleLogs\Tanks_2019_4.log`

*--output*

The location of where the output file should be made.
The output format is a CSV file with the following fields:

*Path,Extension,Category,Time (ms)*

For example `C:\Projects\SimpleEditorLogParser\SampleLogs\Tanks_2019_4.csv`

