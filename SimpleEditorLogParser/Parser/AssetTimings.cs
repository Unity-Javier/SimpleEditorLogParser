namespace SimpleEditorLogParser.Parser
{
    public class AssetTimings
    {
        string assetPath;
        string seconds;
        string extension;
        string categorizedExtension;

        public AssetTimings(string pathToAsset, string importSeconds)
        {
            assetPath = pathToAsset;
            seconds = importSeconds;
            extension = ParserUtils.GetExtension(assetPath, ParserUtils.kDefaultSupportedExtensions);
            categorizedExtension = ParserUtils.CategorizeExtension(extension, ParserUtils.kDefaultSupportedExtensions);
        }

        public override string ToString()
        {
            return $"{assetPath},{extension},{categorizedExtension},{seconds}";
        }
    }
}
