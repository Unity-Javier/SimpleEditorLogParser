using System;

namespace SimpleEditorLogParser.Parser
{
    public class ParserUtils
    {
        public static readonly SupportedExtensions kDefaultSupportedExtensions = new SupportedExtensions();

        public static string GetExtension(string assetPath, SupportedExtensions supported)
        {
            if (assetPath.IndexOf(".") == -1)
                return "folder";

            var supportedExtensions = supported.GetSupportedExtensions();
            var start = assetPath.LastIndexOf(".");

            if (start == -1)
            {
                return "unknown";
            }

            var possibleExtension = assetPath.Substring(start, assetPath.Length - start);
            possibleExtension = possibleExtension.ToLower();

            if (!supportedExtensions.Contains(possibleExtension))
                return "unknown";

            return possibleExtension;
        }

        public static string CategorizeExtension(string extension, SupportedExtensions supported)
        {
            var category = supported.GetCategory(extension);
            return category;
        }
    }
}
